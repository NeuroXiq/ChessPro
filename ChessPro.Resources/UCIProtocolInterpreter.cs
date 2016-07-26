using System;
using System.Collections.Generic;

namespace ChessPro.Resources
{
    public class UCIProtocolInterpreter
    {
        public UCIOption[] EngineOptions { get; private set; }
        public string[] ID { get; private set; }

        public bool EngineIsReady { get; private set; }

        private EngineProcess engineProcess;

        public UCIProtocolInterpreter(EngineProcess engineProcess)
        {
            this.engineProcess = engineProcess;
            this.EngineIsReady = false;
        }

        public void PrepareEngine()
        {
            engineProcess.RunEngineProcess();
            engineProcess.SendString("uci"); // first command to engine - must be sended.

            //now should appear 'id' and 'option' messages.
            List<string> startLines = new List<string>();
            string line = "";

            engineProcess.WaitForSingleStandardOutputString();

            while (engineProcess.ReadStandartOutput(ref line))
            {
                startLines.Add(line);
                if (line == "uciok")
                {
                    break;
                }
                else engineProcess.WaitForSingleStandardOutputString();
            }

            PrepareClassInstance(startLines.ToArray());

            engineProcess.SendString("isready");
            engineProcess.WaitForSingleStandardOutputString();
            string outLine = "";
            engineProcess.ReadStandartOutput(ref outLine);

            if (outLine == "readyok")
            {
                EngineIsReady = true;
            }
            else throw new Exception("engine is not ready (readyok output missing)");

        }

        private void PrepareClassInstance(string[] lines)
        {
            List<string> id = new List<string>();
            List<string> options = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("id"))
                {
                    id.Add(lines[i].Substring(3));
                }
                else if (lines[i].StartsWith("option"))
                    options.Add(lines[i]);
            }
            this.ID = id.ToArray();
            this.EngineOptions = ParseOptions(options.ToArray());
        }

        public void CloseEngine()
        {
            engineProcess.SendString("quit");
            engineProcess.QuitEngineProcess();
            EngineIsReady = false;
        }

        public void SetOption(UCIOption option, string newValue)
        {
            option.SetOption(newValue);
            string setOptionString = string.Empty;

            if (option is UCIButtonOption)
            {
                setOptionString = string.Format("setoption name {0}", option.Name);
            }
            else
            {
                setOptionString = string.Format("setoption name {0} value {1}", option.Name, newValue);
            }
            engineProcess.SendString(setOptionString);
        }

        public void ForceEngineToMoveNow()
        {
            if (EngineIsReady)
            {
                engineProcess.SendString("stop");
            }
        }

        public PieceMove DoMove(string FENnotationString,GoCommandInfo searchingInfo)
        {
            engineProcess.SendString($"position fen {FENnotationString}");
            //DEBUG
            Console.WriteLine("IN => "+FENnotationString);
            //ENDDEBUG
            engineProcess.SendString(searchingInfo.BuildCommandString());
            bool bestMoveReached = false;
            string line = "";
            PieceMove bestMove = null;

            while (!bestMoveReached)
            {
                if (!engineProcess.ReadStandartOutput(ref line))
                {
                    engineProcess.WaitForSingleStandardOutputString();
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("bestmove"))
                {
                    bestMoveReached = true;
                    bestMove = ParseBestMoveLine(line);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("OUT =>"+line);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                        
            }
            return bestMove;
        }

        private PieceMove ParseBestMoveLine(string rawOutputLine)
        {
            /*          [------------MUST BE ----------------]|[--------------CAN BE---------] 
             *          |bestmove<space><char><int><char><int>|<space>ponder<space><char><int>\n
             *          |      -8-     9     10    11   12    | 
             *          |                                     |
             *    OR:   |bestmove<space><char><int><char><int>|<char><space>ponder<space><char><int><char><int>\n
             *          |       -8-     9     10    11   12   | 13[X]
             *  [X] char -> q=queen 
             *  example:
             *  bestmove e2e4 ponder e7e5 / bestmove e2e4
             *  OR:
             *  bestmove e7e8q ponder d3f4 / bestmove ee7e8q
             * * * */

            PieceMove move = null;

            char charIndexStart = rawOutputLine[9];
            int numberIndexStart = (int)char.GetNumericValue(rawOutputLine[10]);
            char charIndexEnd = rawOutputLine[11];
            int numberIndexEnd = (int)char.GetNumericValue(rawOutputLine[12]);


            if (rawOutputLine.Length < 14 || rawOutputLine[13]==' ')
            { // MUST BE 
                move = new PieceMove(
                    new PieceLocation() {CharIndex = charIndexStart, NumberIndex = numberIndexStart },
                    new PieceLocation() {CharIndex = charIndexEnd, NumberIndex= numberIndexEnd});
            }
            else
            {
                Piece.PieceType promotionType = Piece.PieceType.Queen;
                try
                {
                   promotionType  = GetPromotionPiece(rawOutputLine[13]);
                }
                catch
                {
                    if (rawOutputLine == "bestmove (none)")
                        throw new Exception("bestmove (none)");
                    throw new Exception("problems with promotion");
                }
                move = new PieceMove(
                    new PieceLocation() { CharIndex = charIndexStart, NumberIndex = numberIndexStart },
                    new PieceLocation() { CharIndex = charIndexEnd, NumberIndex = numberIndexEnd },
                    true,
                    promotionType);
            }
            

            return move;
        }

        private Piece.PieceType GetPromotionPiece(char promotionChar)
        {
            switch (promotionChar)
            {
                case 'q':
                    return Piece.PieceType.Queen;
                case 'r':
                    return Piece.PieceType.Rook;
                case 'n':
                    return Piece.PieceType.Knight;
                case 'b':
                    return Piece.PieceType.Bishop;
                default:
                    throw new Exception("cannot find corrent char for promotion!");
                         
            }
        }

        private UCIOption[] ParseOptions(string[] rawOutputLines)
        {
            List<UCIOption> options = new List<UCIOption>();
            UCIOptionParser optionsParser = new UCIOptionParser();

            for (int i = 0; i < rawOutputLines.Length; i++)
            {
                if(rawOutputLines[i].StartsWith("option"))
                    options.Add(optionsParser.ParseOption(rawOutputLines[i]));
            }

            return options.ToArray(); 
        }

        
    }
}



//--------- REFLECTION LOOP ON UCIOption[] array to see all properties-------------
//UCIOption[] o = EngineOptions;
//for (int i = 0; i < o.Length; i++)
//{
//    Type t = o[i].GetType();
//    Console.WriteLine("Name: "+t.Name);
//    Console.WriteLine("Properties: ");
//    var prop = t.GetProperties();
//    foreach (var item in prop)
//    {
//        Console.WriteLine(item.Name + " => " + item.GetValue(o[i],null));
//    }
//    Console.WriteLine("------------------------------------");
//    Console.WriteLine();
//}

//private UCIOption ParseOption(string rawOutputLine)
//{
//    //|               //-- MUST BE --\\                      -- can be --  |  |  // -- can be -- \\                                |
//    //option name [some option name...] type [some type...] default [defValue] '[( min & max value) or (var XXX var XXX var XXX var) ]'

//    int lineLenght = rawOutputLine.Length;
//    int nameIndex = rawOutputLine.IndexOf("name");
//    int typeIndex = rawOutputLine.IndexOf("type");
//    int defaultIndex = rawOutputLine.IndexOf("default");


//    string optionName = rawOutputLine.Substring(nameIndex + 5, // name.Lenght + single spacebar
//        typeIndex - "option name ".Length);
//    string type;

//    UCIOption readyOption = null;

//    if (defaultIndex != -1)
//    {
//        type = rawOutputLine.Substring(typeIndex + 5, defaultIndex - (typeIndex + 6));
//        string defaultValue;
//        if (type == "combo")
//        {
//            int firstVarIndex = rawOutputLine.IndexOf("var");
//            defaultValue = rawOutputLine.Substring(defaultIndex + 8, firstVarIndex - (defaultIndex + 8));
//            string varOnly = rawOutputLine.Substring(firstVarIndex + 4, rawOutputLine.Length - (firstVarIndex + 4));
//            string[] vars = varOnly.Split(new string[] { "var" }, StringSplitOptions.None);

//            readyOption = new UCIComboOption(optionName,  defaultValue, vars,defaultValue);
//        }
//        else if (type == "spin")
//        {
//            int minIndex = rawOutputLine.IndexOf("min");
//            int maxIndex = rawOutputLine.IndexOf("max");
//            defaultValue = rawOutputLine.Substring(defaultIndex + 8, minIndex - (defaultIndex + 8));
//            string min = rawOutputLine.Substring(minIndex + 4, maxIndex - (minIndex + 5));
//            string max = rawOutputLine.Substring(maxIndex + 4, rawOutputLine.Length - (maxIndex + 4));

//            readyOption = new UCISpinOption(optionName, 
//                int.Parse(min), int.Parse(max), int.Parse(defaultValue),int.Parse(defaultValue));
//        }
//        else if (type == "check")
//        {
//            defaultValue = rawOutputLine.Substring(defaultIndex + 8, rawOutputLine.Length - (defaultIndex + 8));

//            readyOption = new UCICheckOption(optionName,  bool.Parse(defaultValue),bool.Parse(defaultValue));
//        }
//        else if (type == "string")
//        {
//            defaultValue = rawOutputLine.Substring(defaultIndex + 8, rawOutputLine.Length - (defaultIndex + 8));

//            readyOption = new UCIStringOption(optionName,  defaultValue, defaultValue);
//        }
//        else
//            throw new Exception("Parsing error !");
//    }
//    else
//    {
//        // here option end on 'type' string.
//        type = rawOutputLine.Substring(typeIndex + 5, rawOutputLine.Length - (typeIndex + 5));
//        if (type == "button")
//            readyOption = new UCIButtonOption(optionName);
//    }

//    return readyOption;
//}