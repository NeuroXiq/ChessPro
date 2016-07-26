using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessPro.GUI
{
    class ProgramMain
    {
        [STAThread]
        static void Main(string[] args)
        {
            MainViewPresenter presenter = new MainViewPresenter();
            MainWindow mainWindow = new MainWindow(presenter);
            presenter.SetView(mainWindow);

            Application.EnableVisualStyles();
            Application.Run(mainWindow);
        }
    }
}
