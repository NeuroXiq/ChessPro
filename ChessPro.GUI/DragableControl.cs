using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessPro.GUI
{
    class DragableControl
    {
        private Control control;
        private Control parentControl;
        private bool invokeDropEvent;
        private Point mousePositionOnControl;
        private Point dropPosition;

        public delegate void ControlDroppedDelegate(Control control, Point mousePosition);
        public event ControlDroppedDelegate ControlDropped;

        public DragableControl(Control controlToDrag,Control parentControl)
        {
            this.control = controlToDrag;
            this.parentControl = parentControl;
            this.invokeDropEvent = false;
            control.MouseMove += MouseMovedOverControl;
        }

        private void MouseMovedOverControl(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mouseLocation = parentControl.PointToClient(Cursor.Position);
                int xDifferent = mousePositionOnControl.X;
                int yDifferent = mousePositionOnControl.Y;

                int newX = mouseLocation.X - xDifferent;
                int newY = mouseLocation.Y - yDifferent;
                Point newLocation = new Point(newX, newY);
                control.Location = newLocation;
                dropPosition = mouseLocation;
                invokeDropEvent = true;
            }
            else
            {
                if (invokeDropEvent)
                {
                    ControlDropped?.Invoke(control, dropPosition);
                    invokeDropEvent = false;
                }
                control.BringToFront();
                mousePositionOnControl = e.Location;
            }
        }
    }
}
