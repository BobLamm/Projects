using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VGV101
{
    public partial class DlgDeviceData : Form
    {
        public DlgDeviceData(string dataText)
        {
            InitializeComponent();

            textBox1.Text = dataText;
            // textBox1.DeselectAll();
        }
    }
}
