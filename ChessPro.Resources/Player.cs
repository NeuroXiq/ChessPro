using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public abstract class Player
    {
        
        public Piece.Color Color { get; private set; }

        public Player(Piece.Color color)
        {
            this.Color = color;
        }

        public abstract PieceMove GetMove(string fenGameState);
    }
}
