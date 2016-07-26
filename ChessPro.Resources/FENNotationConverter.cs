using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class FENNotationConverter
    {
        public FENNotationConverter()
        {

        }

        public string ConvertToFEN(ChessBoard board, GameInfo gameInfo)
        {
           return string.Format("{0} {1} {2} {3} {4}",
                BuildPositionString(board),
                BuildActiveColorString(gameInfo),
                BuildCastlingString(gameInfo),
                BuildEnPassantString(gameInfo),
                BuildClockString(gameInfo));
        }

        //public ChessBoard ConvertFromFEN(string fenString)
        //{
        //    ChessBoard b = new ChessBoard();
        //    int emptyFieldsCount;
        //    int i = 0;
        //    for (; i < 32; i++)
        //    {
        //        if (int.TryParse(fenString[i].ToString(), out emptyFieldsCount))
        //        {

        //            i++;
        //        }
        //        else
        //        {
        //            char charIndex = (char)(i % 8 + 'a');
        //            int numberIndex = 8 - (int)(i / 8);   

        //        }
        //    }
        //}

        private string BuildPositionString(ChessBoard board)
        {
            string notation = "";
            string charIndexes = "ABCDEFGH";
            int emptyCount = 0;
            for (int i = 8; i > 0; i--)
            {
                emptyCount = 0;
                for (int j = 0;j < 8; j++)
                {

                    if (board[charIndexes[j], i].IsEmpty)
                    {
                        emptyCount++;
                    }
                    else
                    {
                        if (emptyCount > 0)
                        {
                            notation += emptyCount.ToString();
                            emptyCount = 0;
                        }
                        notation += GetFiledRepresentation(board[charIndexes[j], i]);
                    }
                }
                if (emptyCount > 0)
                    notation += emptyCount.ToString();

                notation = i == 1 ? notation : notation+"/";
            }

            return notation;
        }

        private string BuildActiveColorString(GameInfo gameInfo)
        {
            if (gameInfo.ActiveColor == Piece.Color.Black)
                return "b";
            else return "w";
        }

        private string BuildCastlingString(GameInfo gameInfo)
        {
            string castling = "";
            if (!(gameInfo.CanBlackCastleKingSide || gameInfo.CanBlackCastleQueenSide ||
                gameInfo.CanWhiteCastleKingSide || gameInfo.CanWhiteCastleQueenSide))
            {
                castling = "-";
            }
            else
            {
                if (gameInfo.CanWhiteCastleKingSide)
                    castling += "K";
                if (gameInfo.CanWhiteCastleQueenSide)
                    castling += "Q";
                if (gameInfo.CanBlackCastleKingSide)
                    castling += "k";
                if (gameInfo.CanBlackCastleQueenSide)
                    castling += "q";
            }

            return castling;
        }

        private string BuildEnPassantString(GameInfo gameInfo)
        {
            if (!gameInfo.EnPassantSquareExist)
            {
                return "-";
            }
            else
            {
                //char to lower
                return $"{gameInfo.EnPassantSquare.CharIndex}{gameInfo.EnPassantSquare.NumberIndex}";
            }
        }

        private string BuildClockString(GameInfo gameInfo)
        {
            return $"{gameInfo.HalfMoveClock} {gameInfo.FullMoveClock}";
        }

        private char GetFiledRepresentation(ChessBoard.Field field)
        {
            char representation = '-';
            if (field.IsEmpty)
                representation = '1';
            else
            {
                switch (field.PieceOn.Type)
                {
                    case Piece.PieceType.Pawn:
                        representation = 'p';
                        break;
                    case Piece.PieceType.Rook:
                        representation = 'r';
                        break;
                    case Piece.PieceType.Knight:
                        representation = 'n';
                        break;
                    case Piece.PieceType.Bishop:
                        representation = 'b';
                        break;
                    case Piece.PieceType.King:
                        representation = 'k';
                        break;
                    case Piece.PieceType.Queen:
                        representation = 'q';
                        break;
                    default:
                        break;
                }
            }

            if (field.PieceOn.PieceColor == Piece.Color.White)
                representation = (char)(representation - ('a' - 'A')); // to upper for white
            return representation;

        }
    }
}
