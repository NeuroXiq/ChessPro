using System;
using System.Windows.Forms;

namespace ChessPro.GUI
{
    class ControlPiecePair
    {
        public delegate void SelectedPieceDelegate(ControlPiecePair controlPiecePair, object sender, EventArgs e);
        public event SelectedPieceDelegate SelectedPieceControl;

        public PictureBox Control { get; private set; }
        public PieceComponent Piece { get; private set; }

        public ControlPiecePair(PictureBox control, PieceComponent piece)
        {
            this.Control = control;
            this.Piece = piece;
            this.Control.Click += PictureBoxClicked;
        }

        private void PictureBoxClicked(object sender, EventArgs e)
        {
            SelectedPieceControl?.Invoke(this, sender, e);
        }
    }
}
