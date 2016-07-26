using ChessPro.Resources;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ChessPro.GUI
{
    public class MainViewPresenter : IMainViewPresenter
    {
        private IMainView view;
        private ChessGame game;
        private HumanPlayer humanPlayer;
        private CPUPlayer cpuPlayer;
        private UCIProtocolInterpreter gameEngineProtocolInterpreter;
        private UCIProtocolInterpreter hintEngineProtocolInterpreter;
        private EngineProcess engineProcess;
        private ChessEnginesPE enginesFiles;
        private bool gameStarted;

        private EngineCommunicationViewPresenter communicationViewPresenter;
        private EngineCommunicationWindow communicationWindow;

        private string loadedEngineName;

        //add rigt (assemby location) path in eginesPE1!!!

        public MainViewPresenter()
        {
            enginesFiles = new ChessEnginesPE(GetAssemblyLoadDirectory()+"\\Engines");
            gameStarted = false;
        }
      
        #region IMainViewPresenter

        public void NewGame()
        {
            if (!EngineIsReady())
            {
                view.ShowInformationBox("Please load engine first !");
            }

            Piece.Color humanPlayerColor = view.GetPlayerColor() == PieceComponent.Color.White ? Piece.Color.White : Piece.Color.Black;
            Piece.Color cpuColor = humanPlayerColor == Piece.Color.White ? Piece.Color.Black : Piece.Color.White;

            cpuPlayer = new CPUPlayer(cpuColor,gameEngineProtocolInterpreter);
            cpuPlayer.SetGoCommandInfo(GetGoCommandInfo());
            humanPlayer = new HumanPlayer(humanPlayerColor);

            game = new ChessGame(humanPlayer, cpuPlayer);
            game.PlayerMovedEvent += PlayerMovedInChessGame;
            game.GameEndedEvent += (status) =>
            {
                view.ShowInformationBox(status.ToString());
                gameStarted = false;
            };

            view.RemoveAllPieces();
            SetDefaultPosition();

            Task.Factory.StartNew(() =>
            {
                game.StartNewGame();
            });

            gameStarted = true;
        }

        public bool UserTryMove(PieceComponent piece, char newCharIndex, int newNumberIndex)
        {
            if (!gameStarted)
                return false;

            PieceMove move = new PieceMove(
                    new PieceLocation() { NumberIndex = piece.NumberIndex, CharIndex = piece.CharIndex },
                    new PieceLocation() { NumberIndex = newNumberIndex, CharIndex = newCharIndex });
            Piece movingPiece = ConvertPieceComponentToPiece(piece);

            bool userCanMove = game.CanMove(move) && movingPiece.PieceColor == humanPlayer.Color;
            if (userCanMove)
            {
                humanPlayer.PushMove(move,movingPiece);
            }
            else
            {
                string message = string.Format("Cannot do this move! ({0}{1} to {2}{3})",
                    move.Start.CharIndex, move.Start.NumberIndex,
                    move.End.CharIndex, move.End.NumberIndex);

                view.ShowGameInformation(message);
            }

            return userCanMove;
        }

        public void LoadEngine(string engineName)
        {
            try
            {
                if (engineProcess != null)
                {
                    if (engineProcess.ProcessRunning)
                        gameEngineProtocolInterpreter.CloseEngine();
                }

                engineProcess = enginesFiles.GetEngineProcess(engineName);
                engineProcess.EngineInputSended += (s) => ShowLineInEngineCommunicationView($" {s}");
                engineProcess.EngineOutputReceived += ShowLineInEngineCommunicationView;

                gameEngineProtocolInterpreter = new UCIProtocolInterpreter(engineProcess);
                gameEngineProtocolInterpreter.PrepareEngine();

                view.SetEngineOptions(gameEngineProtocolInterpreter.EngineOptions);
                view.SetEngineLoadStatus("OK");
                view.SetEngineID(gameEngineProtocolInterpreter.ID);
                loadedEngineName = engineName;
            }
            catch (Exception e)
            {
                view.ShowErrorMessage("Load Engine Error!", e.Message);
                view.SetEngineLoadStatus("ERROR");
                loadedEngineName = string.Empty;
            }
        }

        public void GetHint()
        {
            if (!gameStarted)
                return;

            if (hintEngineProtocolInterpreter != null)
            {
                if (!hintEngineProtocolInterpreter.EngineIsReady)
                    hintEngineProtocolInterpreter.PrepareEngine();
            }
            else
            {
                hintEngineProtocolInterpreter = CreateHintUCIProtocoloInterpreter();
                hintEngineProtocolInterpreter.PrepareEngine();
            }

            ShowHintInView();
            
        }

        public void ShowEngineCommunicationView()
        {
            if (communicationWindow != null)
            {
                if (communicationWindow.IsDisposed)
                {
                    OpenNewCommunicationView();
                }
            }
            else
            {
                OpenNewCommunicationView();
            }
        }

        public void UpdateEngineOption(UCIOption option, string newValue)
        {
            gameEngineProtocolInterpreter.SetOption(option, newValue);
        }

        public void SetSearchProperties(SearchProperties properties)
        {
            if (cpuPlayer != null)
            {
                cpuPlayer.SetGoCommandInfo(GetGoCommandInfo());
            }
        }

        public void MoveNow()
        {
            if (!gameStarted)
                return;

            if (gameEngineProtocolInterpreter != null)
            {
                if (gameEngineProtocolInterpreter.EngineIsReady)
                {
                    gameEngineProtocolInterpreter.ForceEngineToMoveNow();
                }
            }
        }

        #endregion

        private string GetAssemblyLoadDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        private bool EngineIsReady()
        {
            bool isReady;
            if (engineProcess != null)
            {
                isReady = engineProcess.ProcessRunning;
            }
            else
            {
                isReady = false;
            }

            return isReady;
        }

        private void ShowHintInView()
        {
            string[] hintMessages = new string[] { "maybe", "try", "good idea is", "probably" };
            string hintMessage = hintMessages[(new Random()).Next(4)];

            FENNotationConverter converter = new FENNotationConverter();
            string fenBoardPositon = converter.ConvertToFEN(game.ActualBoardState, game.ActualGameInfoState);
            PieceMove hintMove = hintEngineProtocolInterpreter.DoMove(fenBoardPositon, new GoCommandInfo() { SearchTime = 1000 });

            string messageToView = string.Format("{4} {0}{1} to {2}{3} ",
                hintMove.Start.CharIndex, hintMove.Start.NumberIndex,
                hintMove.End.CharIndex, hintMove.End.NumberIndex,
                hintMessage);

            view.ShowGameInformation(messageToView);
        }

        private UCIProtocolInterpreter CreateHintUCIProtocoloInterpreter()
        {
            EngineProcess engineProcess = enginesFiles.GetEngineProcess(loadedEngineName);
            UCIProtocolInterpreter protocoloIntepreter = new UCIProtocolInterpreter(engineProcess);
            return protocoloIntepreter;
        }

        private void OpenNewCommunicationView()
        {
             
            Thread communcationWindowThread = new Thread(() =>
            {
                this.communicationViewPresenter = new EngineCommunicationViewPresenter();
                communicationWindow = new EngineCommunicationWindow(communicationViewPresenter);
                communicationViewPresenter.SetView(communicationWindow);
                System.Windows.Forms.Application.Run(communicationWindow);
            });

            communcationWindowThread.SetApartmentState(ApartmentState.STA);
            communcationWindowThread.IsBackground = true;
            communcationWindowThread.Start();
        }

        private void ShowLineInEngineCommunicationView(string line)
        {
            if (communicationWindow != null)
            {
                if (!communicationWindow.IsDisposed)
                {
                    communicationViewPresenter.AddLine(line);
                }
            }
        }

        private void PlayerMovedInChessGame(MoveInfo moveInformations)
        {
            //this method is invoked from another thread (chess game task)
            MoveInfo.MoveType moveType = moveInformations.Type;
            PieceComponent pieceInView = ConvertPieceToPieceComponent(moveInformations);
            int moveEndNumber = moveInformations.Move.End.NumberIndex;
            char moveEndChar = moveInformations.Move.End.CharIndex;

            switch (moveType)
            {
                case MoveInfo.MoveType.ToEmptyFiled:
                    MoveToEmptyField(pieceInView, moveEndChar, moveEndNumber);
                    break;
                case MoveInfo.MoveType.TakeOponentPiece:
                    MoveAndTakeOponentPiece(pieceInView, moveEndChar, moveEndNumber);
                    break;
                case MoveInfo.MoveType.EnPassant:
                    DoEnPassantMove(pieceInView, moveEndChar, moveEndNumber);
                    break;
                case MoveInfo.MoveType.CastlingQueenSide:
                    CastleQueenSide(pieceInView);
                    break;
                case MoveInfo.MoveType.CastlingKingSide:
                    CastleKingSide(pieceInView);
                    break;
                case MoveInfo.MoveType.PawnPromotion:
                    PawnPromotionMove(moveInformations);
                    break;
                default:
                    break;
            }
        }

        private void DoEnPassantMove(PieceComponent pieceInView, char moveEndChar, int moveEndNumber)
        {
            int lineBeforeEndPosition = pieceInView.PieceColor == PieceComponent.Color.White ? moveEndNumber - 1 : moveEndNumber + 1;
            view.RemovePieceAsync(moveEndChar, lineBeforeEndPosition);
            view.MovePieceComponentAsync(pieceInView, moveEndChar, moveEndNumber);
        }

        private void PawnPromotionMove(MoveInfo info)
        {


            //create new PieceComponent specify for promotion type
            PieceComponent.Type type;
            switch (info.Move.PromotionType)
            {
                case Piece.PieceType.Rook:
                    type = PieceComponent.Type.Rook;
                    break;
                case Piece.PieceType.Knight:
                    type = PieceComponent.Type.Knight;
                    break;
                case Piece.PieceType.Queen:
                    type = PieceComponent.Type.Queen;
                    break;
                case Piece.PieceType.Bishop:
                    type = PieceComponent.Type.Bishop;
                    break;
                default:
                    throw new Exception("cannot do promotion to specify promotionType");
                    
            }
            PieceComponent pawnComponentOnBoard = ConvertPieceToPieceComponent(info);
            PieceComponent promotedPiece =
                new PieceComponent(
                    pawnComponentOnBoard.PieceColor,
                    type,
                    info.Move.End.CharIndex,
                    info.Move.End.NumberIndex);
            //remove pawn from board and insert promoted piece
            view.RemovePieceAsync(info.Move.Start.CharIndex, info.Move.Start.NumberIndex);
            //remove piece on end location (pawn can takie piece and get promotion in same time is taking is on end line)
            view.RemovePieceAsync(info.Move.End.CharIndex, info.Move.End.NumberIndex);
            view.AddPieceAsync(promotedPiece);
        }

        private void CastleKingSide(PieceComponent pieceInView)
        {
            int numberIndex = pieceInView.PieceColor == PieceComponent.Color.White ? 1 : 8;
            PieceComponent rook = new PieceComponent(pieceInView.PieceColor, PieceComponent.Type.Rook, 'h', numberIndex);
            view.MovePieceComponentAsync(pieceInView, 'g', numberIndex);
            view.MovePieceComponentAsync(rook, 'f', numberIndex);
        }

        private void CastleQueenSide(PieceComponent pieceInView)
        {
            int numberIndex = pieceInView.PieceColor == PieceComponent.Color.White ? 1 : 8;
            PieceComponent rook = new PieceComponent(pieceInView.PieceColor, PieceComponent.Type.Rook, 'a', numberIndex);
            view.MovePieceComponentAsync(pieceInView, 'c', numberIndex);
            view.MovePieceComponentAsync(rook, 'd', numberIndex);
        }

        private void MoveAndTakeOponentPiece(PieceComponent pieceInView, char moveEndChar, int moveEndNumber)
        {
            view.RemovePieceAsync(moveEndChar, moveEndNumber);
            MoveToEmptyField(pieceInView, moveEndChar, moveEndNumber);
        }

        private void MoveToEmptyField(PieceComponent pieceInView, char moveEndChar, int moveEndNumber)
        {
            view.MovePieceComponentAsync(pieceInView, moveEndChar, moveEndNumber);
        }

        private PieceComponent ConvertPieceToPieceComponent(MoveInfo info)
        {
            PieceComponent.Color color = info.Player.Color == Piece.Color.White ? PieceComponent.Color.White : PieceComponent.Color.Black;

            // PieceComponent.Type & Piece.Type have same string values. 
            // Converting Piece.Type to PieceComponent.Type: 
            string stringPieceType = Enum.GetName(typeof(Piece.PieceType), info.MovingPiece.Type);
            PieceComponent.Type type = (PieceComponent.Type)(Enum.Parse(typeof(PieceComponent.Type), stringPieceType));
            int numberIndex = info.Move.Start.NumberIndex;
            char charIndex = info.Move.Start.CharIndex;

            PieceComponent converted = new PieceComponent(color, type, charIndex, numberIndex);

            return converted;
        }

        private Piece ConvertPieceComponentToPiece(PieceComponent pieceComponent)
        {
            Piece.PieceType type = (Piece.PieceType)Enum.Parse(
                typeof(Piece.PieceType),
                Enum.GetName(typeof(PieceComponent.Type),
                pieceComponent.PieceType));
            Piece.Color color = pieceComponent.PieceColor == PieceComponent.Color.White ? Piece.Color.White : Piece.Color.Black;

            return new Piece(color, type);
        }

        private void SetDefaultPosition()
        {
            view.AddPiece(new PieceComponent(PieceComponent.Color.White, PieceComponent.Type.Rook,'a',1));
            view.AddPiece(new PieceComponent(PieceComponent.Color.White, PieceComponent.Type.Knight, 'b', 1));
            view.AddPiece(new PieceComponent(PieceComponent.Color.White, PieceComponent.Type.Bishop, 'c', 1));
            view.AddPiece(new PieceComponent(PieceComponent.Color.White, PieceComponent.Type.Queen, 'd', 1));
            view.AddPiece(new PieceComponent(PieceComponent.Color.White, PieceComponent.Type.King, 'e', 1));
            view.AddPiece(new PieceComponent(PieceComponent.Color.White, PieceComponent.Type.Bishop, 'f', 1));
            view.AddPiece(new PieceComponent(PieceComponent.Color.White, PieceComponent.Type.Knight, 'g', 1));
            view.AddPiece(new PieceComponent(PieceComponent.Color.White, PieceComponent.Type.Rook, 'h', 1));
            for (char c = 'a'; c <= 'h'; c++)
            {
                view.AddPiece(new PieceComponent(PieceComponent.Color.White, PieceComponent.Type.Pawn, c, 2));
            }
            view.AddPiece(new PieceComponent(PieceComponent.Color.Black, PieceComponent.Type.Rook, 'a', 8));
            view.AddPiece(new PieceComponent(PieceComponent.Color.Black, PieceComponent.Type.Knight, 'b', 8));
            view.AddPiece(new PieceComponent(PieceComponent.Color.Black, PieceComponent.Type.Bishop, 'c', 8));
            view.AddPiece(new PieceComponent(PieceComponent.Color.Black, PieceComponent.Type.Queen, 'd', 8));
            view.AddPiece(new PieceComponent(PieceComponent.Color.Black, PieceComponent.Type.King, 'e', 8));
            view.AddPiece(new PieceComponent(PieceComponent.Color.Black, PieceComponent.Type.Bishop, 'f', 8));
            view.AddPiece(new PieceComponent(PieceComponent.Color.Black, PieceComponent.Type.Knight, 'g', 8));
            view.AddPiece(new PieceComponent(PieceComponent.Color.Black, PieceComponent.Type.Rook, 'h', 8));
            for (char c = 'a'; c <= 'h'; c++)
            {
                view.AddPiece(new PieceComponent(PieceComponent.Color.Black, PieceComponent.Type.Pawn, c, 7));
            }
        }

        public void SetView(IMainView view)
        {
            this.view = view;

            string[] availableEngines = enginesFiles.GetAvailableChessEnginesNames();
            view.ShowAvailableEngines(availableEngines);
        }

        private GoCommandInfo GetGoCommandInfo()
        {
            SearchProperties properties = view.GetSearchProperties();
            GoCommandInfo goCommand = new GoCommandInfo()
            {
                SearchInfinite = properties.SearchInfinity,
                SearchTime = properties.SearchTime,
                Depth = properties.Depth,
                Nodes = properties.Nodes
            };

            return goCommand;
        }
    }
}
