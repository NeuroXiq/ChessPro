using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class HumanPlayer : Player
    {
        private EventWaitHandle threadWait;
        private PieceMove moveToDo;
        private Piece movingPiece;

        public HumanPlayer(Piece.Color color) : base(color)
        {
            this.threadWait = new EventWaitHandle(false, EventResetMode.ManualReset);
        }
        
        public override PieceMove GetMove(string fenGameState)
        {
            threadWait.WaitOne();
            threadWait.Reset();

            int promotionLine = base.Color == Piece.Color.White ? 8 : 1;
            if (promotionLine == moveToDo.End.NumberIndex && movingPiece.Type == Piece.PieceType.Pawn)
            {
                moveToDo = new PieceMove(moveToDo.Start, moveToDo.End, true, Piece.PieceType.Queen);
            }
            return moveToDo;
        }

        public void PushMove(PieceMove move,Piece movingPiece)
        {
            moveToDo = move;
            this.movingPiece = movingPiece;
            threadWait.Set();
        }
        
    }
}
