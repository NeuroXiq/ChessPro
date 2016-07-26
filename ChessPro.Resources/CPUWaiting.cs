using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class CPUWaiting : Player
    {
        private UCIProtocolInterpreter protocolInterpreter;
        private bool ready;
        private EventWaitHandle threadWait;
        private PieceMove moveToDo;

        public CPUWaiting(Piece.Color color, UCIProtocolInterpreter protocolInterpreter) : base(color)
        {
            this.protocolInterpreter = protocolInterpreter;
            threadWait = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        public override PieceMove GetMove(string fenGameState)
        {
            if (!ready)
            {
                protocolInterpreter.PrepareEngine();
                ready = true;
            }
            threadWait.WaitOne();
            threadWait.Reset();
            
            return protocolInterpreter.DoMove(fenGameState, new GoCommandInfo()
            {
                SearchTime = 600
            });
        }
        public void PushMove()
        {
            threadWait.Set();
        }
    }
}
