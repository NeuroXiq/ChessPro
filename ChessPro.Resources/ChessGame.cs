using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    /* TO DO LIST : */
    /* !PAWN PROMOTION
     *   OK - MOveINfo.Type (add promotion)
     *   OK - private enum PawnMoveType (add promotion)
     *   OK - in MovePawn(PieceMove move) method add promotion logic
     */
    // Is draw -> must be update.


    // 1.Update GameInfo fields: 
    /*     OK ---public Piece.Color ActiveColor;       (StartNewGame)
     *     OK ---public PieceLocation EnPassantSquare; (MovePawnTwoFieldsForward,ProcessPeiceMove)
     *     OK ----public bool CanWhiteCastleKingSide;  (MoveKing,MoveRook)
     *     OK ----public bool CanWhiteCastleQueenSide; (MoveKing,MoveRook)
     *     OK ----public bool CanBlackCastleKingSide;  (MoveKing,MoveRook)
     *     OK ----public bool CanBlackCastleQueenSide; (MoveKing,MoveRook)
     *     OK ----public bool EnPassantSquareExist;    (MovePawnTwoFieldsForward,ProcessPieceMove)
     *     OK+--->public int HalfMoveClock; This is the number of halfmoves since the last capture or pawn advance.
     *       |                              This is used to determine if a draw can be claimed under the fifty-move rule.
     *       |  
     *       +-- (StartNewGame, MovePawn (set to 0)) 
     *         
     *     OK ---public int FullMoveClock; (StartNewGame)
     * * * */

    public class ChessGame
    {
        private enum CastlingType
        {
            QueenSide,
            KingSide,
            None
        };
        private enum PawnMoveType
        {
            TwoFieldsForward,
            OneFieldForward,
            EnPassant,
            TakeOponentPiece,
            Promotion
        }

        private GameInfo gameInfo;
        private Player whitePlayer;
        private Player blackPlayer;
        private FENNotationConverter fenConverter;
        private ChessBoard board;
        private bool gameEnded;
        private PieceLocation whiteKingLocation;
        private PieceLocation blackKingLocation;

        public enum EndGameStatus
        {
            WhiteWon,
            BlackWon,
            Stalemate,
            Draw
        }

        public delegate void GameEndedDelegate(EndGameStatus status);
        public delegate void PlayerMovedDelegate(MoveInfo moveInformations);
        public event PlayerMovedDelegate PlayerMovedEvent;
        public event GameEndedDelegate GameEndedEvent;
        public ChessBoard ActualBoardState { get { return board; } }
        public GameInfo ActualGameInfoState { get { return gameInfo; } }

        public ChessGame(Player p1, Player p2)
        {
            this.whitePlayer = p1.Color == Piece.Color.White ? p1 : p2;
            this.blackPlayer = p2.Color == Piece.Color.Black ? p2 : p1;

            this.fenConverter = new FENNotationConverter();
            this.board = ChessBoard.GetStartPositionGameBoard();
            this.gameEnded = false;

            this.gameInfo = new GameInfo()
            {
                ActiveColor = whitePlayer.Color,
                CanBlackCastleKingSide = true,
                CanBlackCastleQueenSide = true,
                CanWhiteCastleKingSide = true,
                CanWhiteCastleQueenSide = true,
                EnPassantSquare = null,
                EnPassantSquareExist = false,
                FullMoveClock = 0,
                HalfMoveClock = 0
            };

        }

        public void StartNewGame()
        {
            PieceMove move;
            gameEnded = false;
            board = ChessBoard.GetStartPositionGameBoard();
            gameInfo.ActiveColor = Piece.Color.White;
            whiteKingLocation = new PieceLocation() { CharIndex = 'e', NumberIndex = 1 };
            blackKingLocation = new PieceLocation() { CharIndex = 'e', NumberIndex = 8 };

            while (!gameEnded)
            {
                ++gameInfo.FullMoveClock;
                ++gameInfo.HalfMoveClock;
                move = whitePlayer.GetMove(fenConverter.ConvertToFEN(board, gameInfo));
                ProcessMove(move);
                gameInfo.ActiveColor = Piece.Color.Black;

                if (gameEnded)
                    break;

                move = blackPlayer.GetMove(fenConverter.ConvertToFEN(board, gameInfo));
                ProcessMove(move);
                gameInfo.ActiveColor = Piece.Color.White;
            }
        }

        private void ProcessMove(PieceMove move)
        {
            if (!CanMove(move))
                throw new Exception("Use CanMove method before you do move !!! (error from process move)");
            Piece movingPiece = board[move.Start].PieceOn;

            /* If en passant move exist - after this move its cleared (pawn do en passant now or not - in both cases its should be cleared)  */
            gameInfo.EnPassantSquareExist = false;
            gameInfo.EnPassantSquare = null;

            // Methods under invoke PlayerMovedEvent with specify MoveInfo and update gameInfo fields.
            // Methods did not do any validation - only do move for piece, invoke event and update gameInfo,
            // validation must be done before.
            switch (movingPiece.Type)
            {
                case Piece.PieceType.King:
                    MoveKing(move);
                    break;
                case Piece.PieceType.Rook:
                    MoveRook(move);
                    break;
                case Piece.PieceType.Pawn:
                    MovePawn(move);
                    break;
                case Piece.PieceType.Knight:
                    MoveKnight(move);
                    break;
                case Piece.PieceType.Queen:
                    MoveQueen(move);
                    break;
                case Piece.PieceType.Bishop:
                    MoveBishop(move);
                    break;
                default:
                    break;
            }
            Piece.Color nextMovePlayerColor = gameInfo.ActiveColor == Piece.Color.White ? Piece.Color.Black : Piece.Color.White;

            bool invokeEndGame = false;
            EndGameStatus state = EndGameStatus.Draw;
            if (IsCheckMate(nextMovePlayerColor))
            {
                state = nextMovePlayerColor == Piece.Color.White ? EndGameStatus.BlackWon : EndGameStatus.WhiteWon;
                invokeEndGame = true;
            }
            else if (IsStaleMate(nextMovePlayerColor))
            {
                state = EndGameStatus.Stalemate;
                invokeEndGame = true;
            }
            else if (IsDraw())
            {
                invokeEndGame = true;
                state = EndGameStatus.Draw;
            }

            if (invokeEndGame)
            {
                this.GameEndedEvent?.Invoke(state);
                gameEnded = true;
            }
        
        }

        private bool IsDraw()
        {
            /*
             * 50 moves rule
             */
            if (gameInfo.HalfMoveClock > 49)
                return true;
            else return false;
        }

        private Piece[] GetPiecesOnBoard()
        {
            List<Piece> pieces = new List<Piece>();

            for (char i = 'a'; i <= 'h'; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    if (!board[i, j].IsEmpty)
                    {
                        pieces.Add(board[i, j].PieceOn);
                    }
                }
            }

            return pieces.ToArray();
        }

        #region Moving Piece & update GameInfo methods

        private void MovePawn(PieceMove move)
        {
            /* Pawn moves types: 
             * 1. Move 2 fields forward (from start postion)
             * 2. Move single field forward.
             * 3. Take oponent piece
             * 4. En passant
             * 5. Promotion
             * 
             * If pawn move 2 fields forward its need to check 
             * for possible en passant move for oponent and if exists 
             * update gameInfo.EnPassantExist/gameinfo.EnPassantField.
             * 
             * If pawn move half move clock must be cleared.
             * 
             */

            PawnMoveType type = GetPawnMoveType(move);
            Piece movingPiece = board[move.Start].PieceOn;
            Piece takenPiece = null;
            MoveInfo.MoveType moveType = MoveInfo.MoveType.CastlingKingSide; //right update under
            Player player = movingPiece.PieceColor == Piece.Color.White ? whitePlayer : blackPlayer;


            switch (type)
            {
                case PawnMoveType.TwoFieldsForward:
                    ProcessPawnMoveTwoFieldsForward(move);
                    moveType = MoveInfo.MoveType.ToEmptyFiled;
                    break;
                case PawnMoveType.OneFieldForward:
                    MovePawnOneFieldForward(move);
                    moveType = MoveInfo.MoveType.ToEmptyFiled;
                    break;
                case PawnMoveType.EnPassant:
                    takenPiece = ProcessPawnMoveEnPassant(move);
                    moveType = MoveInfo.MoveType.EnPassant;
                    break;
                case PawnMoveType.TakeOponentPiece:
                    takenPiece = ProcessPawnTakeOponentPiece(move);
                    moveType = MoveInfo.MoveType.TakeOponentPiece;
                    break;
                case PawnMoveType.Promotion:
                    ProcessPawnPromotionMove(move);
                    moveType = MoveInfo.MoveType.PawnPromotion;
                    break;
                default:
                    break;
            }
            MoveInfo moveInformations = new MoveInfo(move, movingPiece, takenPiece, player, moveType);
            PlayerMovedEvent?.Invoke(moveInformations);

            //clearing half move clock
            gameInfo.HalfMoveClock = 0;
        }

        private Piece ProcessPawnTakeOponentPiece(PieceMove move)
        {
            Piece takenPiece = board[move.End].IsEmpty ? null : board[move.End].PieceOn;
            board[move.End] = board[move.Start];
            board[move.Start] = ChessBoard.Field.Empty;
            return takenPiece;
        }

        private void MovePawnOneFieldForward(PieceMove move)
        {
            //takenPiece = board[move.End].IsEmpty ? null : board[move.End].PieceOn;
            board[move.End] = board[move.Start];
            board[move.Start] = ChessBoard.Field.Empty;
        }

        private void ProcessPawnPromotionMove(PieceMove move)
        {
            Piece.Color color = board[move.Start].PieceOn.PieceColor;
            board[move.Start] = ChessBoard.Field.Empty;
            board[move.End] = new ChessBoard.Field(new Piece(color, move.PromotionType));
        }

        private Piece ProcessPawnMoveEnPassant(PieceMove move)
        {
            int numberIndexAdd = board[move.Start].PieceOn.PieceColor == Piece.Color.White ? -1 : 1;
            Piece takenPawn = board[move.End.CharIndex, move.End.NumberIndex + numberIndexAdd].PieceOn;
            board[move.End.CharIndex, move.End.NumberIndex + numberIndexAdd] = ChessBoard.Field.Empty;
            board[move.End] = board[move.Start];
            board[move.Start] = ChessBoard.Field.Empty;

            return takenPawn;
        }

        private void ProcessPawnMoveTwoFieldsForward(PieceMove move)
        {
            /*
             * if pawn move 2 fields forward, en passant status must be update here (gameInfo).
             */

            int enPassantFieldAdd = board[move.Start].PieceOn.PieceColor == Piece.Color.Black ? 1 : -1;

            //left side
            char charEnd = move.End.CharIndex;
            if (charEnd != 'a')
            {
                Piece pieceOnLeft = board[charEnd - 1, move.End.NumberIndex].IsEmpty ? null : board[charEnd - 1, move.End.NumberIndex].PieceOn;
                if (pieceOnLeft != null)
                {
                    if (pieceOnLeft.PieceColor != board[move.Start].PieceOn.PieceColor && pieceOnLeft.Type == Piece.PieceType.Pawn)
                    {
                        gameInfo.EnPassantSquare = new PieceLocation() { CharIndex = move.End.CharIndex, NumberIndex = move.End.NumberIndex + enPassantFieldAdd };
                        gameInfo.EnPassantSquareExist = true;
                    }
                }
            }
            //right side
            if (charEnd != 'h')
            {
                Piece pieceOnLeft = board[charEnd + 1, move.End.NumberIndex].IsEmpty ? null : board[charEnd + 1, move.End.NumberIndex].PieceOn;
                if (pieceOnLeft != null)
                {
                    if (pieceOnLeft.PieceColor != board[move.Start].PieceOn.PieceColor && pieceOnLeft.Type == Piece.PieceType.Pawn)
                    {
                        gameInfo.EnPassantSquare = new PieceLocation() { CharIndex = move.End.CharIndex, NumberIndex = move.End.NumberIndex + enPassantFieldAdd };
                        gameInfo.EnPassantSquareExist = true;
                    }
                }
            }

            board[move.End] = board[move.Start];
            board[move.Start] = ChessBoard.Field.Empty;
        }

        private PawnMoveType GetPawnMoveType(PieceMove move)
        {
            char charStart = move.Start.CharIndex;
            int numberStart = move.Start.NumberIndex;
            char charEnd = move.End.CharIndex;
            int numberEnd = move.End.NumberIndex;
            Piece.Color playerColor = board[move.Start].PieceOn.PieceColor;

            int startEndNumberDifferent = Math.Abs(numberEnd - numberStart);
            int startPositionNumber = playerColor == Piece.Color.White ? 2 : 7;
            int promotionLine = playerColor == Piece.Color.White ? 8 : 1;

            //pawn goes to last line (promotion) ?
            if (move.End.NumberIndex == promotionLine)
                return PawnMoveType.Promotion;

            // move forward ?
            if (move.Start.CharIndex == move.End.CharIndex)
            {

                //two fields forward ?
                if (startEndNumberDifferent == 2 && numberStart == startPositionNumber)
                {
                    return PawnMoveType.TwoFieldsForward;
                }
                else if (startEndNumberDifferent == 1)
                {
                    return PawnMoveType.OneFieldForward;
                }
                else
                {

                    throw new Exception("Pawn move is incorrect");
                }

            }
            // take oponent piece ?
            int charStartEndDifferent = Math.Abs(charEnd - charStart);
            if (!board[move.End].IsEmpty && charStartEndDifferent == 1 && startEndNumberDifferent == 1) // move on diagonal
                return PawnMoveType.TakeOponentPiece;
            int lineBeforeAdd = board[move.Start].PieceOn.PieceColor == Piece.Color.White ? -1 : 1;

            Piece pieceOnLineBefore = board[move.End.CharIndex, move.End.NumberIndex + lineBeforeAdd].IsEmpty ?
                null : board[move.End.CharIndex, move.End.NumberIndex + lineBeforeAdd].PieceOn;
            // must be en passant
            if (!board[move.End].IsEmpty
                && charStartEndDifferent == 1
                && startEndNumberDifferent == 1
                && pieceOnLineBefore != null
                && pieceOnLineBefore.Type == Piece.PieceType.Pawn
                && pieceOnLineBefore.PieceColor != playerColor)
                return PawnMoveType.EnPassant;

            throw new Exception("Pawn move is incorrent");
            //return PawnMoveType.OneFieldForward;
        }

        private void MoveBishop(PieceMove move)
        {
            /* 
             * 1.take oponent piece
             * 2. move to empty field
             */

            Piece takingPiece = board[move.End].IsEmpty ? null : board[move.End].PieceOn;
            Player movingPlayer = board[move.Start].PieceOn.PieceColor == Piece.Color.White ? whitePlayer : blackPlayer;
            MoveInfo.MoveType moveType = takingPiece == null ? MoveInfo.MoveType.ToEmptyFiled : MoveInfo.MoveType.TakeOponentPiece;
            board[move.End] = board[move.Start];
            board[move.Start] = ChessBoard.Field.Empty;
            MoveInfo informations = new MoveInfo(move, board[move.End].PieceOn, takingPiece, movingPlayer, moveType);

            this.PlayerMovedEvent?.Invoke(informations);
        }

        private void MoveQueen(PieceMove move)
        {
            /* 
             * 1.take oponent piece
             * 2. move to empty field
             */

            Piece takingPiece = board[move.End].IsEmpty ? null : board[move.End].PieceOn;
            Player movingPlayer = board[move.Start].PieceOn.PieceColor == Piece.Color.White ? whitePlayer : blackPlayer;
            MoveInfo.MoveType moveType = takingPiece == null ? MoveInfo.MoveType.ToEmptyFiled : MoveInfo.MoveType.TakeOponentPiece;
            board[move.End] = board[move.Start];
            board[move.Start] = ChessBoard.Field.Empty;
            MoveInfo informations = new MoveInfo(move, board[move.End].PieceOn, takingPiece, movingPlayer, moveType);

            this.PlayerMovedEvent?.Invoke(informations);
        }

        private void MoveKnight(PieceMove move)
        {
            /* -- same as MoveRook -- 
             * 1. Knight takes oponent piece 
             * 2. Knight move to empty field 
             *
             * * * * */

            Piece takingPiece = board[move.End].IsEmpty ? null : board[move.End].PieceOn;
            Player movingPlayer = board[move.Start].PieceOn.PieceColor == Piece.Color.White ? whitePlayer : blackPlayer;
            MoveInfo.MoveType moveType = takingPiece == null ? MoveInfo.MoveType.ToEmptyFiled : MoveInfo.MoveType.TakeOponentPiece;
            board[move.End] = board[move.Start];
            board[move.Start] = ChessBoard.Field.Empty;
            MoveInfo informations = new MoveInfo(move, board[move.End].PieceOn, takingPiece, movingPlayer, moveType);

            this.PlayerMovedEvent?.Invoke(informations);
        }

        private void MoveRook(PieceMove move)
        {
            /*
             * Rook can do 2 types of moves:
             * 1.Move to empty filed.
             * 2.Take oponent piece.
             * 
             * If rook move then castling on rook side can not be done !
             * * * * */

            Piece takingPiece = board[move.End].IsEmpty ? null : board[move.End].PieceOn; // rook takes some piece ? 
            board[move.End] = board[move.Start];
            board[move.Start] = ChessBoard.Field.Empty;//clear start field 
            MoveInfo.MoveType moveType = takingPiece == null ? MoveInfo.MoveType.ToEmptyFiled : MoveInfo.MoveType.TakeOponentPiece;
            Player movingPlayer = board[move.End].PieceOn.PieceColor == Piece.Color.White ? whitePlayer : blackPlayer;
            MoveInfo moveInformations = new MoveInfo(move, board[move.End].PieceOn, takingPiece, movingPlayer, moveType);

            this.PlayerMovedEvent?.Invoke(moveInformations);

            //----
            //set up castling possibility
            //----

            //rook stay on start position (left/right corner) ?
            int rookStartNumberIndex = movingPlayer.Color == Piece.Color.White ? 1 : 8;
            char rookStartCharIndexLeft = 'a'; //queen side
            char rookStartCharIndexRight = 'h'; //king side

            if (move.Start.NumberIndex == rookStartNumberIndex)
            {
                // rook stay on start position ?
                if (move.Start.CharIndex == rookStartCharIndexRight)
                {
                    // right side rook moved - king side castling imposible
                    if (movingPlayer.Color == Piece.Color.White)
                        gameInfo.CanWhiteCastleKingSide = false;
                    else gameInfo.CanBlackCastleKingSide = false;
                }
                else if (move.Start.CharIndex == rookStartCharIndexLeft)
                {
                    //left sdie rook moved - queen side castling imposible.
                    if (movingPlayer.Color == Piece.Color.White)
                        gameInfo.CanWhiteCastleQueenSide = false;
                    else gameInfo.CanBlackCastleQueenSide = false;
                }
            }
        }

        private void MoveKing(PieceMove move)
        {
            /* There can be few type of moves which king can do:
             * 1. Normal move (single square from start position).
             * 2. Castling king side.
             * 3. Castling queen side.
             * 4. Take oponent piece.
             * 
             * If king move, he cant do any castling.
             * * * */

            MoveInfo moveInformations = null;
            Player movingPlayer = gameInfo.ActiveColor == Piece.Color.White ? whitePlayer : blackPlayer;
            Piece.Color color = board[move.Start].PieceOn.PieceColor;
            CastlingType type = MoveIsCastling(move, color);

            if (type == CastlingType.None) // 1/4
            {
                moveInformations = DoSingleFieldKingMove(move, movingPlayer, color);
            }
            else // king move is castling. 3/4
            {
                moveInformations = ProcessKingCastling(move, movingPlayer, color, type);
            }
            //set up game info (after king move, he cant do any castling)
            UpdateCastlingPrivileges(color);
            UpdateKingPosition(move, color);

            PlayerMovedEvent?.Invoke(moveInformations);

        }

        private void UpdateKingPosition(PieceMove move, Piece.Color kingColor)
        {
            if (kingColor == Piece.Color.White)
            {
                this.whiteKingLocation = move.End;
            }
            else
            {
                this.blackKingLocation = move.End;
            }
        }

        private void UpdateCastlingPrivileges(Piece.Color color)
        {
            if (color == Piece.Color.Black)
            {
                gameInfo.CanBlackCastleKingSide =
                    gameInfo.CanBlackCastleQueenSide = false;
            }
            else
            {
                gameInfo.CanWhiteCastleKingSide =
                    gameInfo.CanWhiteCastleQueenSide = false;
            }
        }

        private MoveInfo ProcessKingCastling(PieceMove move, Player movingPlayer, Piece.Color color, CastlingType type)
        {
            MoveInfo moveInformations;
            CastleKing(type, color);

            MoveInfo.MoveType moveType = type == CastlingType.KingSide ?
                MoveInfo.MoveType.CastlingKingSide : MoveInfo.MoveType.CastlingQueenSide;

            moveInformations = new MoveInfo(move,
                new Piece(color, Piece.PieceType.King),
                null,
                movingPlayer,
                moveType);
            return moveInformations;
        }

        private MoveInfo DoSingleFieldKingMove(PieceMove move, Player movingPlayer, Piece.Color color)
        {
            MoveInfo moveInformations;
            Piece pieceWhichKingTakes = board[move.End].IsEmpty ? null : board[move.End].PieceOn;

            board[move.End] = board[move.Start];
            board[move.Start] = ChessBoard.Field.Empty;

            MoveInfo.MoveType moveType =
                pieceWhichKingTakes == null ? MoveInfo.MoveType.ToEmptyFiled : MoveInfo.MoveType.TakeOponentPiece;

            moveInformations =
                new MoveInfo(move, new Piece(color, Piece.PieceType.King), pieceWhichKingTakes, movingPlayer, moveType);
            return moveInformations;
        }

        private CastlingType MoveIsCastling(PieceMove move, Piece.Color color)
        {
            int numberStartIndex = color == Piece.Color.White ? 1 : 8;
            char charStartIndex = 'e';
            PieceLocation startLocation = new PieceLocation() { CharIndex = charStartIndex, NumberIndex = numberStartIndex };

            if (move.Start.Equals(startLocation))
            {
                bool queenSideCastling =
                    move.End.CharIndex == 'c' &&
                    move.End.NumberIndex == numberStartIndex;
                bool kingSideCastling =
                    move.End.CharIndex == 'g' &&
                    move.End.NumberIndex == numberStartIndex;

                if (queenSideCastling)
                    return CastlingType.QueenSide;
                else if (kingSideCastling)
                    return CastlingType.KingSide;
                else return CastlingType.None;
            }
            else return CastlingType.None;
        }

        private void CastleKing(CastlingType type, Piece.Color color)
        {
            if (type == CastlingType.None)
                throw new Exception("Castlign type is incorrect !");

            int kingNumberIndex = color == Piece.Color.White ? 1 : 8;
            char kingCharIndex = type == CastlingType.KingSide ? 'g' : 'c';
            char rookCharIndex = type == CastlingType.KingSide ? 'f' : 'd';

            //move king and rook to positions after castling
            board[kingCharIndex, kingNumberIndex] = new ChessBoard.Field(new Piece(color, Piece.PieceType.King));
            board[rookCharIndex, kingNumberIndex] = new ChessBoard.Field(new Piece(color, Piece.PieceType.Rook));

            //clean up king position and rook position
            board['e', kingNumberIndex] = ChessBoard.Field.Empty;
            if (type == CastlingType.KingSide)
                board['h', kingNumberIndex] = ChessBoard.Field.Empty;
            else board['a', kingNumberIndex] = ChessBoard.Field.Empty;
        }

        #endregion

        private bool IsStaleMate(Piece.Color activeColor)
        {
            PieceLocation kingLocation = activeColor == Piece.Color.White ? whiteKingLocation : blackKingLocation;
            return !CanDoAnyMoveWithoutCheck(kingLocation, activeColor);
        }

        private bool IsCheckMate(Piece.Color activeColor)
        {
            /*
             * 1. activeColor king is on check
             * 2. check all possible moves - if any can remove check return false. 
             * 
             * * * * * */
            PieceLocation kingLocation = activeColor == Piece.Color.White ? whiteKingLocation : blackKingLocation;
            if (IsCheck(kingLocation, board))
            {
                if (CanDoAnyMoveWithoutCheck(kingLocation, activeColor))
                    return false;
                return true;
            }
            else return false;

            
        }

        private bool CanDoAnyMoveWithoutCheck(PieceLocation kingLocation, Piece.Color activeColor)
        {
            // get all pieces with player color
            // try to do all possible moves for piece and check for check after move.
            ChessBoard afterMove = board.Clone();
            ChessBoard.Field fieldCopy;
            PieceLocation[] pieces = GetAllPiecesLocations(activeColor);
            for (int i = 0; i < pieces.Length; i++)
            {
                PieceMove[] possibleMoves = GetPossibleMoves(pieces[i]);
                
                for (int j = 0; j < possibleMoves.Length; j++)
                {
                    PieceMove move = possibleMoves[j];
                    fieldCopy = afterMove[move.End];
                    afterMove[move.End] = afterMove[move.Start];
                    afterMove[move.Start] = ChessBoard.Field.Empty;

                    if (afterMove[move.End].PieceOn.Type == Piece.PieceType.King)
                    {
                        if (!IsCheck(move.End, afterMove))
                            return true;
                    }
                    else
                    {
                        if (!IsCheck(kingLocation, afterMove))
                            return true;
                    }

                    afterMove[move.Start] = afterMove[move.End];
                    afterMove[move.End] = fieldCopy; 
                }

                //is check after move ?

                //undo move
            }
            return false;
        }

        #region Get status about pieces on board

        private PieceMove[] GetPossibleMoves(PieceLocation pieceLocation)
        {
            Piece pieceOnLocation = board[pieceLocation].PieceOn;

            switch (pieceOnLocation.Type)
            {
                case Piece.PieceType.King:
                    return GetPossibleMovesForKing(pieceLocation, pieceOnLocation.PieceColor);
                case Piece.PieceType.Rook:
                    return GetPossibleMovesForRook(pieceLocation, pieceOnLocation.PieceColor);
                case Piece.PieceType.Pawn:
                    return GetPossibleMovesForPawn(pieceLocation, pieceOnLocation.PieceColor);
                case Piece.PieceType.Knight:
                    return GetPossibleMovesForKnight(pieceLocation, pieceOnLocation.PieceColor);
                case Piece.PieceType.Queen:
                    return GetPossibleMovesForQueen(pieceLocation, pieceOnLocation.PieceColor);
                case Piece.PieceType.Bishop:
                    return GetPossibleMovesForBishop(pieceLocation, pieceOnLocation.PieceColor);
                default:
                    break;
            }
            throw new Exception("Inknow piece type");
        }

        private PieceMove[] GetPossibleMovesForBishop(PieceLocation pieceLocation, Piece.Color pieceColor)
        {
            List<PieceMove> locations = new List<PieceMove>();
            char c = pieceLocation.CharIndex;
            int n = pieceLocation.NumberIndex;

            //left up 
            locations.AddRange(GetLocationsToMove( 1, -1, pieceLocation,pieceColor));
            //left down
            locations.AddRange(GetLocationsToMove(-1, -1, pieceLocation, pieceColor));
            //right up 
            locations.AddRange(GetLocationsToMove( 1, 1, pieceLocation, pieceColor));
            //right down
            locations.AddRange(GetLocationsToMove( -1,  1, pieceLocation, pieceColor));

            return locations.ToArray();
        }

        private PieceMove[] GetLocationsToMove(int nIncrement, int cIncrement, PieceLocation start,Piece.Color pieceColor)
        {
            int n = start.NumberIndex;
            char c = start.CharIndex;
            List<PieceMove> possibleMoves = new List<PieceMove>();
            while (FieldExist(n+=nIncrement, c=(char)(c+cIncrement)))
            {
                PieceLocation loc = new PieceLocation() { CharIndex = c, NumberIndex = n };
                if (board[loc].IsEmpty)
                    possibleMoves.Add(new PieceMove(start,loc));
                else
                {
                    if (board[loc].PieceOn.PieceColor != pieceColor && board[loc].PieceOn.Type != Piece.PieceType.King)
                    {
                        possibleMoves.Add(new PieceMove(start, loc));
                        break;
                    }
                    break;
                }
            }
            return possibleMoves.ToArray();
        }

        private bool FieldExist(int numberIndex, int charIndex)
        {
            return numberIndex > 0 && numberIndex < 9 && charIndex >= 'a' && charIndex <= 'h';
        }

        private PieceMove[] GetPossibleMovesForQueen(PieceLocation pieceLocation, Piece.Color pieceColor)
        {
            List<PieceMove> locations = new List<PieceMove>();
            char c = pieceLocation.CharIndex;
            int n = pieceLocation.NumberIndex;

            //left up 
            locations.AddRange(GetLocationsToMove(1, -1, pieceLocation, pieceColor));
            //left down
            locations.AddRange(GetLocationsToMove(-1, -1, pieceLocation, pieceColor));
            //right up 
            locations.AddRange(GetLocationsToMove(1, 1, pieceLocation, pieceColor));
            //right down
            locations.AddRange(GetLocationsToMove(-1, 1, pieceLocation, pieceColor));

            //left  
            locations.AddRange(GetLocationsToMove(0, -1, pieceLocation, pieceColor));
            //up
            locations.AddRange(GetLocationsToMove(1, 0, pieceLocation, pieceColor));
            //right  
            locations.AddRange(GetLocationsToMove(0, 1, pieceLocation, pieceColor));
            //down
            locations.AddRange(GetLocationsToMove(-1, 0, pieceLocation, pieceColor));
            return locations.ToArray();
        }

        private PieceMove[] GetPossibleMovesForKnight(PieceLocation pieceLocation, Piece.Color pieceColor)
        {
            List<PieceMove> moves = new List<PieceMove>();
            var fieldsToCheckIterations = new[]
           {
                new { charChange = -1 , numberChange= -2 },
                new { charChange = -1 , numberChange= 2 },
                new { charChange = -2 , numberChange= -1 },
                new { charChange = -2 , numberChange= 1 },
                new { charChange = 1 , numberChange=  2 },
                new { charChange = 1 , numberChange= -2 },
                new { charChange = 2 , numberChange= -1 },
                new { charChange = 2 , numberChange= 1 },
            };

            for (int i = 0; i <8; i++)
            {
                char c = (char)(pieceLocation.CharIndex + fieldsToCheckIterations[i].charChange);
                int n = pieceLocation.NumberIndex + fieldsToCheckIterations[i].numberChange;
                PieceLocation location = new PieceLocation() { NumberIndex = n, CharIndex = c };
                if (FieldExist(n, c))
                {
                    if (board[location].IsEmpty)
                    {
                        moves.Add(new PieceMove(pieceLocation, location));
                    }
                    else
                    {
                        if (board[location].PieceOn.PieceColor != pieceColor && board[location].PieceOn.Type != Piece.PieceType.King)
                            moves.Add(new PieceMove(pieceLocation, location));
                    }
                }
            }
            return moves.ToArray();
        }

        private PieceMove[] GetPossibleMovesForPawn(PieceLocation pieceLocation, Piece.Color pieceColor)
        {

            List<PieceMove> moves = new List<PieceMove>();
            int nextLine = pieceColor == Piece.Color.White ? 1 : -1;
            int startLine = pieceColor == Piece.Color.White ? 2 : 7;
            char c = pieceLocation.CharIndex;
            int n = pieceLocation.NumberIndex;

            if (FieldExist( n + nextLine, c))
            {
                
                if (board[c, n + nextLine].IsEmpty)
                    moves.Add(new PieceMove(pieceLocation, new PieceLocation() { CharIndex = c, NumberIndex = n + nextLine }));
                if (pieceLocation.NumberIndex == startLine)
                {
                    if (board[c, n + (nextLine*2)].IsEmpty)
                        moves.Add(new PieceMove(pieceLocation, new PieceLocation() { CharIndex = c, NumberIndex = n + (nextLine * 2) }));
                }
            }
            //left across
            if (FieldExist(n + nextLine, c - 1))
            {
                //en passant
                if (board[ c - 1, n + nextLine].IsEmpty)
                {
                    if (gameInfo.EnPassantSquareExist)
                    {
                        char ec = gameInfo.EnPassantSquare.CharIndex;
                        int en = gameInfo.EnPassantSquare.NumberIndex;

                        if (ec == c - 1 && n + nextLine == en)
                           moves.Add(new PieceMove(pieceLocation, new PieceLocation() { CharIndex = (char)(c - 1), NumberIndex = n + nextLine }));
                    }
                    
                }
                else
                {
                    if (board[ c - 1, n + nextLine].PieceOn.PieceColor != pieceColor)
                    {
                        moves.Add(new PieceMove(pieceLocation, new PieceLocation() { CharIndex = (char)(c - 1), NumberIndex = n + nextLine }));
                    }
                }
            }
            //right across
            if (FieldExist(n + nextLine, c + 1))
            {
                if (board[ c + 1, n + nextLine].IsEmpty)
                {
                    if (gameInfo.EnPassantSquareExist)
                    {
                        char ec = gameInfo.EnPassantSquare.CharIndex;
                        int en = gameInfo.EnPassantSquare.NumberIndex;

                        if (ec == c + 1 && n + nextLine == en)
                            moves.Add(new PieceMove(pieceLocation, new PieceLocation() { CharIndex = (char)(c + 1), NumberIndex = n + nextLine }));
                    }

                }
                else
                {
                    if (board[ c + 1,n + nextLine].PieceOn.PieceColor != pieceColor)
                    {
                        moves.Add(new PieceMove(pieceLocation, new PieceLocation() { CharIndex = (char)(c + 1), NumberIndex = n + nextLine }));
                    }
                }
            }
            return moves.ToArray();
            
            
        }

        private PieceMove[] GetPossibleMovesForRook(PieceLocation pieceLocation, Piece.Color pieceColor)
        {
            List<PieceMove> locations = new List<PieceMove>();
            char c = pieceLocation.CharIndex;
            int n = pieceLocation.NumberIndex;

            // up 
            locations.AddRange(GetLocationsToMove(1, 0,pieceLocation, pieceColor));
            // down
            locations.AddRange(GetLocationsToMove(-1, 0, pieceLocation, pieceColor));
            //right  
            locations.AddRange(GetLocationsToMove(0, 1, pieceLocation, pieceColor));
            //left
            locations.AddRange(GetLocationsToMove(0, -1, pieceLocation, pieceColor));

            return locations.ToArray();
        }

        private PieceMove[] GetPossibleMovesForKing(PieceLocation location, Piece.Color pieceColor)
        {
            var locations = new[]
            {
                new { charC=1,numberC=0 },
                new { charC=-1,numberC=0 },
                new { charC=0,numberC=1 },
                new { charC=0,numberC=-1 },

                new { charC=-1,numberC=-1 },
                new { charC=1,numberC=1 },
                new { charC=-1,numberC=1 },
                new { charC=1,numberC=-1 },
            };
            List<PieceMove> moves = new List<PieceMove>();

            char c = location.CharIndex;
            int n = location.NumberIndex;

            for (int i = 0; i < locations.Length; i++)
            {
                int lc = locations[i].charC;
                int ln = locations[i].numberC;

                if (FieldExist(n + ln,c+lc))
                {
                    PieceMove toAdd = null;
                    if (board[c + lc, n + ln].IsEmpty)
                        toAdd = new PieceMove(location, new PieceLocation() { CharIndex = (char)(c + lc), NumberIndex = (n + ln) });
                    else
                    {
                        if (board[c + lc, n + ln].PieceOn.PieceColor != pieceColor && board[c + lc, n + ln].PieceOn.Type != Piece.PieceType.King)
                        {
                            toAdd = new PieceMove(location, new PieceLocation() { CharIndex = (char)(c + lc), NumberIndex = (n + ln) });
                        }
                    }
                    if(toAdd != null)
                    {
                        moves.Add(toAdd);

                    }
                }
            }
            return moves.ToArray();
        }

        private PieceLocation[] GetAllPiecesLocations(Piece.Color color)
        {
            List<PieceLocation> pieces = new List<PieceLocation>(); 
            for (int i = 'a' ; i <= 'h'; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    if (!board[i, j].IsEmpty)
                    {
                        Piece pieceOn = board[i, j].PieceOn;
                        if (pieceOn.PieceColor == color)
                            pieces.Add(new PieceLocation() { CharIndex = (char)i, NumberIndex=j });
                    }
                }
            }
            return pieces.ToArray();
        }

        private bool IsCheck(PieceLocation kingLocation, ChessBoard gameStatus)
        {
            Piece.Color kingColor = gameStatus[kingLocation].PieceOn.PieceColor;
            Piece.Color oponentColor = kingColor == Piece.Color.White ? Piece.Color.Black : Piece.Color.White;

            return
                IsCheckFromKing(kingLocation, gameStatus, oponentColor) ||
                IsCheckFromRook(kingLocation, gameStatus, oponentColor) ||
                IsCheckFromBishop(kingLocation, gameStatus, oponentColor) ||
                IsCheckFromKnight(kingLocation, gameStatus, oponentColor) ||
                IsCheckFromQueen(kingLocation, gameStatus, oponentColor) ||
                IsCheckFromPawn(kingLocation, gameStatus, oponentColor);
        }

        private bool IsCheckFromKing(PieceLocation kingLocation, ChessBoard gameStatus, Piece.Color oponentColor)
        {
            var locations = new[]
           {
                new { charC=1,numberC=0 },
                new { charC=-1,numberC=0 },
                new { charC=0,numberC=1 },
                new { charC=0,numberC=-1 },

                new { charC=1,numberC=1 },
                new { charC=1,numberC=-1 },
                new { charC=-1,numberC=1 },
                new { charC=-1,numberC=-1 },
            };
            PieceLocation newLocation;
            
            char c;
            int n;
            for (int i = 0; i < locations.Length; i++)
            {
                c = (char)(kingLocation.CharIndex + locations[i].charC);
                n = locations[i].numberC + kingLocation.NumberIndex;

                if (c <= 'h' && c >= 'a' && n >= 1 && c < 9)
                {
                    newLocation = new PieceLocation() { CharIndex = c, NumberIndex = n };
                    if(!gameStatus[newLocation].IsEmpty)
                    {
                        Piece pieceOn = gameStatus[newLocation].PieceOn;
                        if (pieceOn.PieceColor == oponentColor && pieceOn.Type == Piece.PieceType.King)
                            return true;
                        
                    }
                }

            }
            return false;
        }

        private bool IsCheckFromRook(PieceLocation kingLocation,ChessBoard gameStatus, Piece.Color oponentColor)
        {
            /*
             * Method tries to find the first piece on the cross from kings location
             * If its rook with oponent color returns true else return false.
             */

            var iterations = new[]
            {
                new { charChange = -1, numberChange= 0 },//left side
                new { charChange =  1, numberChange= 0 },//right side
                new { charChange =  0, numberChange= 1 }, // up
                new { charChange =  0, numberChange= -1 },//down
            };

            for (int i = 0; i < iterations.Length; i++)
            {
                Piece firstOnDiagonal = FirsPieceFromLocation(iterations[i].charChange, iterations[i].numberChange, gameStatus, kingLocation);
                if (firstOnDiagonal != null)
                {
                    if (firstOnDiagonal.PieceColor == oponentColor && firstOnDiagonal.Type == Piece.PieceType.Rook)
                        return true;
                }
            }
            return false;
        }

        private bool IsCheckFromBishop(PieceLocation kingLocation, ChessBoard gameStatus, Piece.Color oponentColor)
        {
            /*
             * Method look for first piece on diagonals from kings location.
             * If its is bishop returs true else return false.
             */
            var iterations = new[]
            {
                new { charChange = -1, numberChange = -1 },//left down
                new { charChange =  -1, numberChange = 1 },//left up
                new { charChange =  1, numberChange= -1 }, // right down
                new { charChange =  1, numberChange= 1 },//right up
            };

            for (int i = 0; i < iterations.Length; i++)
            {
                Piece firstOnDiagonal = FirsPieceFromLocation(iterations[i].charChange, iterations[i].numberChange, gameStatus, kingLocation);
                if (firstOnDiagonal != null)
                {
                    if (firstOnDiagonal.PieceColor == oponentColor && firstOnDiagonal.Type == Piece.PieceType.Bishop)
                        return true;
                }
            }

            return false;

        }

        private bool IsCheckFromKnight(PieceLocation kingLocation, ChessBoard gameStatus, Piece.Color oponentColor)
        {
            var fieldsToCheckIterations = new[]
            {
                new { charChange = -1 , numberChange= -2 },
                new { charChange = -1 , numberChange= 2 },
                new { charChange = -2 , numberChange= -1 },
                new { charChange = -2 , numberChange= 1 },
                new { charChange = 1 , numberChange=  2 },
                new { charChange = 1 , numberChange= -2 },
                new { charChange = 2 , numberChange= -1 },
                new { charChange = 2 , numberChange= 1 },
            };

            for (int i = 0; i < fieldsToCheckIterations.Length; i++)
            {
                char charIndex = (char)(kingLocation.CharIndex + fieldsToCheckIterations[i].charChange);
                int numberIndex = kingLocation.NumberIndex + fieldsToCheckIterations[i].numberChange;

                if (charIndex >= 'a' && charIndex <= 'h' && numberIndex >= 1 && numberIndex <= 8)
                {
                    if (!gameStatus[charIndex, numberIndex].IsEmpty)
                    {
                        Piece pieceOnField = gameStatus[charIndex, numberIndex].PieceOn;
                        if (pieceOnField.PieceColor == oponentColor && pieceOnField.Type == Piece.PieceType.Knight)
                            return true;
                    }
                    else continue;
                }
            }
            return false;
        }

        private bool IsCheckFromQueen(PieceLocation kingLocation, ChessBoard gameStatus, Piece.Color oponentColor)
        {

            //cross and diagonals for queen (like rook & bishop)
            var iterations = new[]
           {
                new { charChange = -1 , numberChange= -1 },//diagonals
                new { charChange = -1 , numberChange= 1 },
                new { charChange = 1 , numberChange= -1 },
                new { charChange = 1 , numberChange= 1 },

                new { charChange = 0 , numberChange= -1 },//cross
                new { charChange = 0 , numberChange=  1 },
                new { charChange = -1 , numberChange= 0 },
                new { charChange = 1 , numberChange= 0 },

            };

            for (int i = 0; i < iterations.Length; i++)
            {
                Piece firstOnDiagonal = FirsPieceFromLocation(iterations[i].charChange, iterations[i].numberChange, gameStatus, kingLocation);
                if (firstOnDiagonal != null)
                {
                    if (firstOnDiagonal.PieceColor == oponentColor && firstOnDiagonal.Type == Piece.PieceType.Queen)
                        return true;
                }
            }
            return false;
        }

        private bool IsCheckFromPawn(PieceLocation kingLocation, ChessBoard gameStatus, Piece.Color oponentColor)
        {
            int pawnsLineChange = oponentColor == Piece.Color.White ? -1 : 1;
            char charIndex = kingLocation.CharIndex;
            int numberIndex = kingLocation.NumberIndex;

            for (int i = -1; i <= 1; i+=2)
            {
                if (charIndex + i >= 'a' && charIndex + i <= 'h' && numberIndex + pawnsLineChange < 9 && pawnsLineChange + numberIndex > 0)
                {
                    if (!gameStatus[charIndex + i, numberIndex + pawnsLineChange].IsEmpty)
                    {
                        Piece pieceOnLine = gameStatus[charIndex + i, numberIndex + pawnsLineChange].PieceOn;
                        if (pieceOnLine.PieceColor == oponentColor && pieceOnLine.Type == Piece.PieceType.Pawn)
                            return true;
                    }
                }
            }
            return false;
        }

        private Piece FirsPieceFromLocation(int charIterationChange, int numberIterationChange,ChessBoard gameStatus, PieceLocation startLocation)
        {
            char charIndex = (char)(startLocation.CharIndex + charIterationChange);
            int numberIndex = startLocation.NumberIndex + numberIterationChange; ;

            while (charIndex >= 'a' && numberIndex >= 1 && charIndex <= 'h' && numberIndex <= 8)
            {
                if (!gameStatus[charIndex, numberIndex].IsEmpty)
                {
                    Piece pieceOn = gameStatus[charIndex, numberIndex].PieceOn;
                    return pieceOn;
                }

                charIndex = (char)(charIndex + charIterationChange);
                numberIndex += numberIterationChange;
            }

            return null;
        }

        #endregion

        public bool CanMove(PieceMove move)
        {
            /*
             * 1. Move is valid for piece for actual board status.
             * 2. After move can not be check for moving player.
             * * * * * * */

            Piece movingPiece = board[move.Start].PieceOn;

            //move must be to another filed.
            if (move.Start.Equals(move.End))
                return false;
            //some piece should be on start field...
            if (board[move.Start].IsEmpty)
                return false;
            //on end field cannot be any player piece of course
            if (!board[move.End].IsEmpty)
            {
                if (board[move.End].PieceOn.PieceColor == movingPiece.PieceColor)
                    return false;
            }

            bool moveIsPossible = true;
            // Methods below didnt look for check for player which moving right now.
            // Searching for check after actual move must be done under this switch 
            // Methods expect valid position (eg. in board[move.Start] must be right piece)

            switch (movingPiece.Type)
            {
                case Piece.PieceType.King:
                    moveIsPossible = MovePossibleForKing(move);
                    break;
                case Piece.PieceType.Rook:
                    moveIsPossible = MovePossibleForRook(move);
                    break;
                case Piece.PieceType.Pawn:
                    moveIsPossible = MovePossibleForPawn(move);
                    break;
                case Piece.PieceType.Knight:
                    moveIsPossible = MovePossibleForKnight(move);
                    break;
                case Piece.PieceType.Queen:
                    moveIsPossible = MovePossibleForQueen(move);
                    break;
                case Piece.PieceType.Bishop:
                    moveIsPossible = MovePossibleForBishop(move);
                    break;
                default:
                    break;
            }

            if (!moveIsPossible)
                return false;

            //simulate move on board copy: 
            ChessBoard afterMoveBoard = board.Clone();
            Piece.Color color = gameInfo.ActiveColor;
            PieceLocation playerKingLocation = color == Piece.Color.White ? whiteKingLocation : blackKingLocation;



            //moving piece is king - need update
            if (move.Start.Equals(playerKingLocation))
                playerKingLocation = move.End;
            //do move on board clone 
            afterMoveBoard[move.End] = afterMoveBoard[move.Start];
            afterMoveBoard[move.Start] = ChessBoard.Field.Empty;


            if (IsCheck(playerKingLocation, afterMoveBoard))
                return false;

            return true;
        }

        #region Moves validation

        private bool MovePossibleForBishop(PieceMove move)
        {
            
            Piece.Color bishopColor = board[move.Start].PieceOn.PieceColor;
            //move must be on diagonal and bigger than 0;
            int startEndNumberDifferent = Math.Abs(move.End.NumberIndex - move.Start.NumberIndex);
            int startEndCharDifferent = Math.Abs(move.Start.CharIndex - move.End.CharIndex);

            if (startEndCharDifferent != startEndNumberDifferent || startEndNumberDifferent < 1)
                return false;
            //set up diagonal to check
            int charIterationDifferent = move.Start.CharIndex < move.End.CharIndex ? 1 : -1;
            int numberIterationDifferent = move.Start.NumberIndex < move.End.NumberIndex ? 1 : -1;
            //check only to field before - on targer field can be oponent piece.
            //PieceLocation singleFieldBefore = new PieceLocation()
            //{
            //    NumberIndex = move.End.NumberIndex - numberIterationDifferent,
            //    CharIndex = (char)(move.End.CharIndex - charIterationDifferent)
            //};

            //some piece on bishop way ?
            Piece pieceOnWay = FirstPieceFromLocation(charIterationDifferent, numberIterationDifferent, move.End, move.Start);
            if (pieceOnWay == null)
            {
                if (board[move.End].IsEmpty)
                    return true;
                else
                {
                    //on targer field can stay oponent piece but not king
                    Piece pieceOnEndField = board[move.End].PieceOn;
                    if (pieceOnEndField.PieceColor != bishopColor && pieceOnEndField.Type != Piece.PieceType.King)
                        return true;
                    else return false;

                }
            }
            else
            {
                return false;
            }
        }

        private bool MovePossibleForQueen(PieceMove move)
        {
            int charDifferent = Math.Abs(move.Start.CharIndex - move.End.CharIndex);
            int numberDifferent = Math.Abs(move.Start.NumberIndex - move.End.NumberIndex);
            int charIncrement = move.Start.CharIndex < move.End.CharIndex ? 1 : -1;
            int numberIncrement = move.Start.NumberIndex < move.End.NumberIndex ? 1 : -1;
            Piece.Color queenColor = board[move.Start].PieceOn.PieceColor;
            if (numberDifferent > 0 && charDifferent > 0)
            {
                if (numberDifferent != charDifferent)
                    return false;
            }
            // must move min 1 field
            if (numberDifferent == 0 && charDifferent == 0)
                return false;
            //set up where queen move (cross,diagonal,corss-diagonal)
            if (charDifferent == 0)
                charIncrement = 0;
            if (numberDifferent == 0)
                numberIncrement = 0;
            //check last field
            if (!board[move.End].IsEmpty)
            {
                // on end cannot be piece with same color & king
                if (board[move.End].PieceOn.PieceColor == queenColor)
                    return false;
                else if (board[move.End].PieceOn.Type == Piece.PieceType.King)
                    return false;
            }

            //check queen way

            Piece pieceOnWay = FirstPieceFromLocation(charIncrement, numberIncrement, move.End, move.Start);
            if (pieceOnWay == null)
            {
                return true;
            }
            else return false;
        }

        private bool MovePossibleForKnight(PieceMove move)
        {
            var kf = new[]
            {
                new { cChange = -1 ,nChange= -2 },
                new { cChange = -1 ,nChange= 2 },
                new { cChange = -2 ,nChange= -1 },
                new { cChange = -2 ,nChange= 1 },
                new { cChange = 1 , nChange=  2 },
                new { cChange = 1 , nChange= -2 },
                new { cChange = 2 , nChange= -1 },
                new { cChange = 2 , nChange= 1 },
            };
            Piece.Color knightColor = board[move.Start].PieceOn.PieceColor;
            if (!board[move.End].IsEmpty)
            {
                if (board[move.End].PieceOn.Type == Piece.PieceType.King || board[move.End].PieceOn.PieceColor == knightColor)
                    return false;
            }
            PieceLocation toCheck;
            for (int i = 0; i < kf.Length; i++)
            {
                toCheck = new PieceLocation()
                {
                    CharIndex = (char)(move.Start.CharIndex + kf[i].cChange),
                    NumberIndex = move.Start.NumberIndex + kf[i].nChange
                };
                if (toCheck.CharIndex > 'h' || toCheck.CharIndex < 'a' || toCheck.NumberIndex > 8 || toCheck.NumberIndex < 1)
                    continue;
                if (toCheck.Equals(move.End))
                {
                    return true;
                }
            }

            return false;
        }

        private bool MovePossibleForPawn(PieceMove move)
        {
            int movedFieldsForward = Math.Abs(move.Start.NumberIndex - move.End.NumberIndex);
            int movedFieldAcross = Math.Abs(move.Start.CharIndex - move.End.CharIndex);
            Piece.Color pawnColor = board[move.Start].PieceOn.PieceColor;

            if (movedFieldsForward == 1)
            {
                /*
                 * 1. en passant
                 * 2. take oponent piece
                 * 3. move forward
                 */

                //forward move
                if (move.End.CharIndex == move.Start.CharIndex)
                {
                    if (board[move.End].IsEmpty)
                        return true;
                    else return false;
                }
                else
                {
                    //pawn can goes left/right & forward.
                    if (movedFieldAcross == 1)
                    {
                        int nextLineAdd = pawnColor == Piece.Color.White ? 1 : -1;
                        if (move.End.NumberIndex == move.Start.NumberIndex + nextLineAdd)
                        {
                            // en passant 
                            if (board[move.End].IsEmpty)
                            {
                                if (gameInfo.EnPassantSquareExist)
                                {
                                    int enPassantLine = pawnColor == Piece.Color.White ? 5 : 4;
                                    if (move.End.NumberIndex - nextLineAdd != enPassantLine)
                                        return false;
                                    if (board[move.End.CharIndex, move.End.NumberIndex - nextLineAdd].IsEmpty)
                                        return false;
                                    else if (board[move.End.CharIndex, move.End.NumberIndex - nextLineAdd].PieceOn.PieceColor != pawnColor &&
                                        board[move.End.CharIndex, move.End.NumberIndex - nextLineAdd].PieceOn.Type == Piece.PieceType.Pawn)
                                    {
                                        return true;
                                    }
                                    else return false;
                                }
                                else return false;
                            }
                            else
                            {
                                if (board[move.End].PieceOn.PieceColor != pawnColor &&
                                    board[move.End].PieceOn.Type != Piece.PieceType.King)
                                {
                                    return true;
                                }
                                else return false;
                            }
                        }
                        else return false;
                    }
                }
            }
            else if (movedFieldsForward == 2)
            {
                if (movedFieldAcross == 0)
                {
                    //pawn must be on start position
                    int startLine = board[move.Start].PieceOn.PieceColor == Piece.Color.White ? 2 : 7;
                    int lineChange = startLine == 2 ? -1 : 1;
                    if (move.Start.NumberIndex == startLine)
                    {
                        //between start & end cannot be any pieces
                        if (board[move.End.CharIndex, move.End.NumberIndex + lineChange].IsEmpty)
                            return true;
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;

            return false;
        }

        private bool MovePossibleForRook(PieceMove move)
        {
            // 1. between start and end field can not be any piece.
            // 2. on end field can stay oponent piece but its cant be king.

            Piece.Color rookColor = board[move.Start].PieceOn.PieceColor;
            int startEndNumberDifferent = Math.Abs(move.End.NumberIndex - move.Start.NumberIndex);
            int startEndCharDifferent = Math.Abs(move.Start.CharIndex - move.End.CharIndex);

            //move is on cross and bigger than 1 ?
            if (!((startEndNumberDifferent > 0 && startEndCharDifferent == 0) ||
                (startEndCharDifferent > 0 && startEndNumberDifferent == 0)))
                return false;

            int numberIncrement = move.Start.NumberIndex < move.End.NumberIndex ? 1 : -1;
            int charIncrement = move.Start.CharIndex < move.End.CharIndex ? 1 : -1;
            charIncrement = startEndCharDifferent == 0 ? 0 : charIncrement;
            numberIncrement = startEndNumberDifferent == 0 ? 0 : numberIncrement;
           

            Piece pieceOnWay = FirstPieceFromLocation(charIncrement, numberIncrement, move.End, move.Start);
            //check piece on move.End field.
            if (pieceOnWay == null)
            {
                if (board[move.End].IsEmpty)
                    return true;
                else
                {
                    if (board[move.End].PieceOn.PieceColor != rookColor && board[move.End].PieceOn.Type != Piece.PieceType.King)
                        return true;
                    else return false;
                }
            }
            else return false;

        }

        private bool MovePossibleForKing(PieceMove move)
        {
            //this is a single field move ?
            int startEndNumberDifferent = Math.Abs(move.Start.NumberIndex - move.End.NumberIndex);
            int startEndCharDifferent = Math.Abs(move.Start.CharIndex - move.End.CharIndex);
            Piece king = board[move.Start].PieceOn;

            if (startEndNumberDifferent > 1 || startEndCharDifferent > 2)
                return false;
            
            //normal single field move
            if ((startEndCharDifferent == 1 || startEndCharDifferent == 0) &&
                (startEndNumberDifferent == 1 || startEndNumberDifferent == 0))
            {
                //if end field is empty - ok 
                if (board[move.End].IsEmpty)
                    return true;
                else // if not - on end field cannot be piece with same color.
                {
                    if (board[move.End].PieceOn.PieceColor != king.PieceColor)
                        return true;
                    else return false;
                }
            }

            //last option must be castling
            return CanKingCastle(move);
           
        }

        private bool CanKingCastle(PieceMove move)
        {
            Piece king = board[move.Start].PieceOn;
            CastlingType castling = MoveIsCastling(move, king.PieceColor);
            bool castlingPrivileges = false;

            if (castling == CastlingType.None)
                return false;
            else
            {
                if (king.PieceColor == Piece.Color.White)
                {
                    castlingPrivileges =
                        castling == CastlingType.KingSide ? gameInfo.CanWhiteCastleKingSide : gameInfo.CanWhiteCastleQueenSide;
                }
                else
                {
                    castlingPrivileges =
                        castling == CastlingType.KingSide ? gameInfo.CanBlackCastleKingSide : gameInfo.CanBlackCastleQueenSide;
                }
            }

            if (castlingPrivileges)
            {
                //between rook and king cannot be any piece
                if (castling == CastlingType.KingSide)
                {
                    Piece firstPiece = FirsPieceFromLocation(1, 0, board, move.Start);
                    if (firstPiece == null)
                        return false;
                    else if (firstPiece.PieceColor == king.PieceColor && firstPiece.Type == Piece.PieceType.Rook)
                        return true;
                    
                }
                else
                {
                    Piece firstPiece = FirsPieceFromLocation(-1, 0, board, move.Start);
                    if (firstPiece == null)
                        return false;
                    else if (firstPiece.PieceColor == king.PieceColor && firstPiece.Type == Piece.PieceType.Rook)
                        return true;
                    else return false;

                }
            }

            return false;
        }

        #endregion

        private Piece FirstPieceFromLocation(int charInterationChange, int numberIterationChange, PieceLocation endLocation, PieceLocation startLocation)
        {
            if (startLocation.Equals(endLocation))
                return null;

            char charIndex = (char)(startLocation.CharIndex + charInterationChange);
            int numberIndex = startLocation.NumberIndex + numberIterationChange;
            //while (charIndex >= 'a' && charIndex <= 'h' && numberIndex >= 1 && numberIndex <= 8)
            while
                (
                !(charIndex == endLocation.CharIndex &&
                numberIndex == endLocation.NumberIndex) &&
                charIndex >= 'a' && 
                charIndex <= 'h' && 
                numberIndex >= 1 &&
                numberIndex <= 8
                )
            {
                if (!board[charIndex, numberIndex].IsEmpty)
                    return board[charIndex, numberIndex].PieceOn;
                charIndex = (char)(charIndex + charInterationChange);
                numberIndex += numberIterationChange;
            }
            return null;
        }
    }
}

