﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VGV101
{
    public partial class OldListEditor : Form
    {
        public OldListEditor(int dataReceived)
        {
            InitializeComponent();
//            label1.Text = dataReceived;
            comboBox1.SelectedIndex = dataReceived;
        }
    }
}