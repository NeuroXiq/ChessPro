using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class GameInfo
    {
        public Piece.Color ActiveColor;
        public PieceLocation EnPassantSquare;
        public bool CanWhiteCastleKingSide;
        public bool CanWhiteCastleQueenSide;
        public bool CanBlackCastleKingSide;
        public bool CanBlackCastleQueenSide;
        public bool EnPassantSquareExist;
        public int HalfMoveClock;
        public int FullMoveClock;
    }
}
