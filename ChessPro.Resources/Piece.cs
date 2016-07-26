using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class Piece
    {
        public PieceType Type { get; private set; }
        public Color PieceColor { get; private set; }

        public enum PieceType
        {
            King,Rook,Pawn,Knight,Queen,Bishop
        }
        public enum Color
        {
            Black,
            White
        }

        public Piece( Color color, PieceType type)
        {
            this.Type = type;
            this.PieceColor = color;
        }

        public override bool Equals(object obj)
        {

            if (obj is Piece)
            {
                Piece p = obj as Piece;
                return p.PieceColor == this.PieceColor &&
                    p.Type == this.Type;
            }
            else return false;
        }
    }
}
