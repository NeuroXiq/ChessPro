using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class UCIStringOption : UCIOption
    {
        public string ActualValue { get; private set; }
        public readonly string DefaultValue;

        public UCIStringOption(string name,  string actualValue, string defaultValue): base(name,UCIOption.Type.String,actualValue)
        {
            this.ActualValue = actualValue;
            this.DefaultValue = defaultValue;
        }

        public override void SetOption(string newValue)
        {
            this.ActualValue = newValue;
            base.ActualValue = newValue;
        }
    }
}
