using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class UCIComboOption : UCIOption
    {
        public string[] ValuesList { get; private set; }
        public readonly string DefaultValue;
        public string ActualValue { get; private set; }


        public UCIComboOption(string name,string defaultValue,string[] possibleValues,string actualValue) : base(name, UCIOption.Type.Combo,actualValue)
        {
            this.ValuesList = possibleValues;
            this.DefaultValue = defaultValue;
            this.ActualValue = actualValue;
        }

        public override void SetOption(string newValue)
        {
            if (ValuesList.Contains(newValue))
            {
                this.ActualValue = newValue;
                base.ActualValue = newValue;
            }
            else throw new InvalidOperationException("Cannot find value in ValuesList !");
        }
    }
}
