using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class UCIButtonOption : UCIOption
    {
        public UCIButtonOption(string name) : base(name, UCIOption.Type.Button,string.Empty)
        {
        }

        [Obsolete("Using this method is meaningless")]
        public override void SetOption(string newValue)
        {
            
        }
    }
}
