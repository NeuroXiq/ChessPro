using ChessPro.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.GUI.EngineOptionControls
{
    public delegate void OptionChangedDelegate(UCIOption option, string newValue);

    interface IEngineOptionControl
    {
        event OptionChangedDelegate OptionChangedEvent;

        UCIOption Option { get;}
        string ActualValue { get; }

        void SetWidth(int value); 
    }
}
