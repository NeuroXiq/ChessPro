using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.GUI
{
    public interface IEngineCommunicationViewPresenter
    {
        void AddLine(string line);
        void ClearWindow();
    }
}
