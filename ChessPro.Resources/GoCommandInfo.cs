using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class GoCommandInfo
    {
        public string SearchMoves = default(string);
        public bool Ponder = default(bool);
        public int WhiteTimeLeft = default(int);
        public int BlackTimeLeft = default(int);
        public int WhiteIncrementPerMove = default(int);
        public int BlackIncrementPerMove = default(int);
        public int MovesToGo = default(int);
        public int Depth = default(int);
        public int Nodes = default(int);
        public int MateIn = default(int);
        public int SearchTime = default(int);
        public bool SearchInfinite = default(bool);

        public string BuildCommandString()
        {
            string command = "go ";
            if (SearchMoves != default(string))
                command += SearchMoves;
            if (Ponder)
                command += Ponder.ToString();
            if (WhiteTimeLeft != 0)
                command += $"wtime {WhiteTimeLeft} ";
            if (BlackTimeLeft != 0)
                command += $"btime {BlackTimeLeft} ";
            if (WhiteIncrementPerMove != 0)
                command += $"winc {WhiteIncrementPerMove} ";
            if (BlackIncrementPerMove != 0)
                command += $"binc {BlackIncrementPerMove} ";
            if (MovesToGo != 0)
                command += $"movestogo {MovesToGo} ";
            if (Depth > 0)
                command += $"depth {Depth} ";
            if (Nodes > 0)
                command += $"nodes {Nodes} ";
            if (MateIn > 0)
                command += $"mate {MateIn} ";
            if (SearchTime > 0 && !SearchInfinite)
                command += $"movetime {SearchTime} ";
            if (SearchInfinite)
                command += "infinite";

            return command;
        }
    }
}
