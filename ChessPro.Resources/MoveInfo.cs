using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class MoveInfo
    {
        public enum MoveType
        {
            ToEmptyFiled,
            TakeOponentPiece,
            EnPassant,
            CastlingQueenSide,
            CastlingKingSide,
            PawnPromotion
        }
        public PieceMove Move { get; private set; }
        public Piece MovingPiece { get; private set; }
        public Piece TakenPiece { get; private set; }
        public Player Player { get; private set; }
        public MoveType Type { get; private set; }

        public MoveInfo(PieceMove move, Piece movingPiece, Piece takenPiece, Player player, MoveType moveType)
        {
            this.Move = move;
            this.MovingPiece = movingPiece;
            this.TakenPiece = takenPiece;
            this.Player = player;
            this.Type = moveType;
        }
    }
}
