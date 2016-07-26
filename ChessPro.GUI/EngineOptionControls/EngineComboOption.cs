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
    public partial class EngineComboOption : UserControl, IEngineOptionControl
    {
        public UCIOption Option { get; private set; }
        public string ActualValue { get;  private set; }
        public event OptionChangedDelegate OptionChangedEvent;

        public EngineComboOption(UCIComboOption option)
        {
            InitializeComponent();
            this.ActualValue = option.ActualValue;
            this.Option = option;

            //set up comboBox events & properties

            this.comboBoxOptions.Items.AddRange(option.ValuesList);
            this.comboBoxOptions.SelectedIndex = comboBoxOptions.FindStringExact(ActualValue);
            this.optionNameLabel.Text = option.Name;
            
        }

        private void SelectedItemChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxOptions.SelectedIndex;
            string selectedValue = comboBoxOptions.Items[selectedIndex].ToString();

            if (selectedValue != ActualValue)
            {
                this.ActualValue = comboBoxOptions.Items[selectedIndex].ToString();
                OptionChangedEvent?.Invoke(Option, selectedValue);
            }
        }

        public void SetWidth(int value)
        {
            this.comboBoxOptions.Width = value - 3;
            this.Width = value;
        }
    }
}
