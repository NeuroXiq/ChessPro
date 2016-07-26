using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public abstract class UCIOption
    {
        public enum Type
        {
            Check,
            Spin,
            Combo,
            Button,
            String
         };

        public string Name { get; private set; }
        public string ActualValue { get; protected set; }
        public readonly Type OptionType;

        public UCIOption(string name, Type type,string actualValue)
        {
            this.Name = name;
            this.OptionType = type;
            this.ActualValue = actualValue;
        }

        public abstract void SetOption(string newValue);
    }
}
