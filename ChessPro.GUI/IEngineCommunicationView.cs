using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.GUI
{
    public interface IEngineCommunicationView
    {
        void Clear();
        void AddLine(string line);
    }
}
