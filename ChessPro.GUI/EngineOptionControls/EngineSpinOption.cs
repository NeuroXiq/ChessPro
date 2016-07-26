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
    public partial class EngineSpinOption : UserControl, IEngineOptionControl
    {
        public UCIOption Option { get; private set; }
        public string ActualValue { get; private set; }
        public event OptionChangedDelegate OptionChangedEvent;

        bool trackBarClicked;

        public EngineSpinOption(UCISpinOption option)
        {
            InitializeComponent();

            trackBarClicked = false;
            Option = option;

            this.trackBar.Minimum = option.MinValue;
            this.trackBar.Maximum = option.MaxValue > 10000 ? 10000 : option.MaxValue; 
            this.trackBar.Value = option.ActualValue;

            this.trackBar.MouseDown += (sender, args) => trackBarClicked = args.Button == MouseButtons.Left ? true : false;
            this.trackBar.ValueChanged += (sender, args) =>
            {
                ActualValue = trackBar.Value.ToString();
                this.actualScrollBarValue.Text = ActualValue;
            };
            this.trackBar.MouseUp += (sender, args) =>
            {
                if (trackBarClicked)
                {
                    trackBarClicked = false;
                    OptionChangedEvent?.Invoke(Option, ActualValue);
                }
            };

            this.optionName.Text = option.Name;
            this.actualScrollBarValue.Text = option.ActualValue.ToString();
        }

       
        

        public void SetWidth(int value)
        {
            this.Width = value;
            this.trackBar.Width = value - 3;
        }
    }
}
