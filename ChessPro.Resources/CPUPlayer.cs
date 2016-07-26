using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class CPUPlayer : Player
    {
        private GoCommandInfo goCommandInfo;
        private UCIProtocolInterpreter protocolInterpreter;
        private int searchingTime=500;

        public CPUPlayer(Piece.Color color,UCIProtocolInterpreter protocolInterpreter) : base(color)
        {
            this.protocolInterpreter = protocolInterpreter;
        }

        public override PieceMove GetMove(string fenGameState)
        {
            if (!protocolInterpreter.EngineIsReady)
                protocolInterpreter.PrepareEngine();

            return protocolInterpreter.DoMove(fenGameState, goCommandInfo);
        }

        public void SetGoCommandInfo(GoCommandInfo goCommandInfo)
        {
            this.goCommandInfo = goCommandInfo;

        }
    }
}
