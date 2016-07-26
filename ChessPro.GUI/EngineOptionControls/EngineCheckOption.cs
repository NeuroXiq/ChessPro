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
    public partial class EngineCheckOption : UserControl, IEngineOptionControl
    {
        public UCIOption Option { get; private set; }
        public string ActualValue { get; private set; }
        public event OptionChangedDelegate OptionChangedEvent;

        public EngineCheckOption(UCICheckOption option)
        {
            InitializeComponent();

            this.Option = option;
            this.optionCheckBox.Text = option.Name;
            this.optionCheckBox.Checked = option.ActualValue;
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            this.ActualValue = this.optionCheckBox.Checked.ToString();
            OptionChangedEvent?.Invoke(Option, ActualValue.ToString());
        }

        public void SetWidth(int value)
        {
            this.Width = value;
        }
    }
}
