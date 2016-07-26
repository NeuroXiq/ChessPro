using System.Drawing;
using System.Windows.Forms;

namespace ChessPro.GUI
{
    class DragableControlPiecePair
    {
        private DragableControl dragableControl;
        private ControlPiecePair controlPiecePair;

        public delegate void ControlPiecePairDroppedDelegate(ControlPiecePair controlPiecePair, Point dropLocationOnParent);
        public event ControlPiecePairDroppedDelegate ControlPiecePairDropped;

        public DragableControlPiecePair(ControlPiecePair controlPiecePair, Control parentControl)
        {
            this.dragableControl = new DragableControl(controlPiecePair.Control, parentControl);
            this.dragableControl.ControlDropped += (control, locationOnParent) =>
            {
                ControlPiecePairDropped?.Invoke(controlPiecePair, locationOnParent);
            };
        }
    }
}
