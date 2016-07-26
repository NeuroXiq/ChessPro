using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessPro.GUI
{
    public partial class EngineCommunicationWindow : Form, IEngineCommunicationView
    {
        private IEngineCommunicationViewPresenter presenter;

        public EngineCommunicationWindow(IEngineCommunicationViewPresenter presenter)
        {
            InitializeComponent();
            this.presenter = presenter;
        }

        public void AddLine(string line)
        {
            this.Invoke((MethodInvoker)
                delegate () 
                {
                    this.engineCommunicationRichBox.Text += line + "\n";
                    engineCommunicationRichBox.SelectionStart = engineCommunicationRichBox.Text.Length;
                    // scroll it automatically
                    engineCommunicationRichBox.ScrollToCaret();
                    if (engineCommunicationRichBox.Text.Length > 5000)
                        Clear();
                });
        }

        public void Clear()
        {
            this.Invoke((MethodInvoker)
                delegate () { this.engineCommunicationRichBox.Clear(); });
        }
    }
}
