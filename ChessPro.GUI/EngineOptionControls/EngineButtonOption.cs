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
    public partial class EngineButtonOption : UserControl, IEngineOptionControl
    {

        public UCIOption Option { get; }
        public string ActualValue { get { return string.Empty; } }//only for button is empty
        public event OptionChangedDelegate OptionChangedEvent;

        public EngineButtonOption(UCIButtonOption optionInstance)
        {
            InitializeComponent();
            this.Option = optionInstance;
            this.optionNameLabel.Text = optionInstance.Name;
        }

        

        private void SetOptionButtonClick(object sender, EventArgs e)
        {
            OptionChangedEvent?.Invoke(Option, "");
        }

        public void SetWidth(int value)
        {
            this.Width = value;
            //number 3 is constant margin number
            this.setOptionButton.Width = value - 3;
        }
    }
}
