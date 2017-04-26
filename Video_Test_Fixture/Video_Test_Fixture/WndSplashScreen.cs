using System;
using System.Threading;
using System.Windows.Forms;

namespace Video_Test_Fixture
{
    public partial class WndSplashScreen : Form
    {
        public WndSplashScreen()
        {
            InitializeComponent();

            Show();

            for (int cnt=0; cnt<333; cnt++)
            {
                Application.DoEvents();
                Thread.Sleep(1);
            }
        }
    }
}
