using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class UCICheckOption : UCIOption
    {
        public bool ActualValue { get; private set; }
        public readonly bool DefalutValue;
        
        public UCICheckOption(string name, bool defaultValue,bool actualValue) : base(name, UCIOption.Type.Check,actualValue.ToString())
        {
            this.DefalutValue = defaultValue;
            this.ActualValue = actualValue;
        }

        public override void SetOption(string newValue)
        {
            bool parsed = false;
            bool parsingResult = bool.TryParse(newValue, out parsingResult);
            if (parsingResult)
            {
                ActualValue = parsed;
                base.ActualValue = parsed.ToString();
            }
            else throw new InvalidCastException("newValue is not a boolean type !");
        }
    }
}
