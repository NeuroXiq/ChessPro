using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class UCISpinOption : UCIOption
    {
        public int MinValue { get; private set; }
        public int MaxValue { get; private set; }
        public readonly int DefaultValue;
        public int ActualValue { get; private set; }

        public UCISpinOption(string name,int minValue,int maxValue,int defaultValue,int actualValue) : base(name, UCIOption.Type.Spin,actualValue.ToString())
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.DefaultValue = defaultValue;
            this.ActualValue = actualValue;
        }

        public override void SetOption(string newValue)
        {
            int parsed;
            bool parsingResult = int.TryParse(newValue, out parsed);
            if (parsingResult)
            {
                if (parsed > MaxValue || parsed < MinValue)
                {
                    throw new IndexOutOfRangeException($"newValue is out range {MinValue} - {MaxValue}");
                }
                else
                {
                    ActualValue = parsed;
                    base.ActualValue = parsed.ToString();
                }

            }
            else throw new InvalidCastException("Cannot parse newValue into int type !");

        }
    }
}
