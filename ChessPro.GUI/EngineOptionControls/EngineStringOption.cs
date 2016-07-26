using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChessPro.Resources;

namespace ChessPro.GUI.EngineOptionControls
{
    public partial class EngineStringOption : UserControl, IEngineOptionControl
    {
        public UCIOption Option { get; private set; }
        public string ActualValue { get; private set; }
        public event OptionChangedDelegate OptionChangedEvent;

        public EngineStringOption(UCIStringOption option)
        {
            InitializeComponent();

            this.Option = option;
            this.optionValueTextBox.Text = option.ActualValue;
            this.optionName.Text = Option.Name;
            
            optionValueTextBox.TextChanged += TextBoxValueChanged;
        }

        private void TextBoxValueChanged(object sender, EventArgs e)
        {
            this.ActualValue = optionValueTextBox.Text;
            OptionChangedEvent?.Invoke(Option, ActualValue);
        }
        public void SetWidth(int value)
        {
            this.Width = value;
            this.optionValueTextBox.Width = value - 6;
        }
    }
}
