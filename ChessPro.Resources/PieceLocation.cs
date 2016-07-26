using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class PieceLocation
    {
        public int NumberIndex;
        public char CharIndex;

        public override bool Equals(object obj)
        {
            if (obj is PieceLocation)
            {
                PieceLocation p = obj as PieceLocation;
                return p.CharIndex == this.CharIndex &&
                    p.NumberIndex == this.NumberIndex;
            }
            else return false;
        }
    }
}
