using System;

namespace ChessPro.GUI
{
    public class PieceComponent
    {
        public enum Color { Black,White };
        public enum Type { Rook, Knight, Bishop, Queen, King, Pawn };

        public Color PieceColor { get; private set; }
        public Type PieceType { get; private set; }

        public char CharIndex { get; private set; }
        public int NumberIndex { get; private set; }

        public PieceComponent(Color color, Type type,char charIndex,int numberIndex)
        {
            this.PieceColor = color;
            this.PieceType = type;
            this.CharIndex = charIndex;
            this.NumberIndex = numberIndex;
        }

        public void SetLocation(char charIndex, int numberIndex)
        {
            this.NumberIndex = numberIndex;
            this.CharIndex = charIndex;
        }

        public override bool Equals(object obj)
        {
            if (obj is PieceComponent)
            {
                PieceComponent piece = obj as PieceComponent;
                return piece.PieceColor == this.PieceColor &&
                    piece.PieceType == this.PieceType &&
                    piece.NumberIndex == this.NumberIndex &&
                    piece.CharIndex == this.CharIndex;
            }
            else return false;
        }

        public override int GetHashCode()
        {
           return  Convert.ToInt32(
                Convert.ToString((byte)CharIndex) +
                Convert.ToString(NumberIndex) +
                Convert.ToString((int)PieceColor) +
                Convert.ToString((int)PieceType));
        }
    }
}
