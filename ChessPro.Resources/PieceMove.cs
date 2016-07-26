using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class PieceMove
    {
        public PieceLocation Start { get; private set; }
        public PieceLocation End { get; private set; }
        public bool Promotion { get; private set; }
        public Piece.PieceType PromotionType { get; private set; }

        public PieceMove(PieceLocation start, PieceLocation end,bool promotion,Piece.PieceType promotionType)
        {
            Start = start;
            End = end;
            Promotion = promotion;
            PromotionType = promotionType;
        }
        public PieceMove(PieceLocation start, PieceLocation end) : this(start,end,false,Piece.PieceType.Pawn)
        {

        }
    }
}
