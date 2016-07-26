using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.GUI
{
    public class EngineCommunicationViewPresenter : IEngineCommunicationViewPresenter
    {
        private IEngineCommunicationView view;

        public EngineCommunicationViewPresenter()
        {

        }

        public void AddLine(string line)
        {
            view.AddLine(line);
        }

        public void ClearWindow()
        {
            view.Clear();
        }

        public void SetView(IEngineCommunicationView view)
        {
            this.view = view;
        }
    }
}
