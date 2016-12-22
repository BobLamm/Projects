using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VGV101
{
    public partial class DlgSourceMonitor : Form
    {
//        public bool Form1.takeToProgram; //  Defined in Form1...


        // Variables for drag/drop...

//        private int offsetX;
//        private int offsetY;
//        private bool moveButton = false;  // Read this from current settings too...
//        private bool isDragging = false;

        // camera tells the monitor which camera it is (1-4) or other video sources (5 and 6).  0 = Preview
        // monitorLabel tells the monitor what text to put in its label
        // tally tells the monitor which camera is on the air so it knows if it should be red or not.

        public DlgSourceMonitor(int camera, string monitorLabel, int tally)
        {
            InitializeComponent();
            label1.Text = monitorLabel;
            if (camera == tally)
            {
                label1.BackColor = Color.FromArgb(255, 0, 0);
            }
            else
            {
                label1.BackColor = Color.FromArgb(255, 255, 255);
            }
        }

        private void Form5_Click(object sender, EventArgs e) // Take camera to Program or Preview
        {
            if (MainWindow.takeToProgram == true)
            {
                MessageBox.Show("Takes this camera to Program.");
            }
            else
            {
                MessageBox.Show("Takes this camera to Preview.");
            }
        }


        //CONTEXT MENU1 

        private void takeToProgramToolStripMenuItem_Click(object sender, EventArgs e)  // ContextMenu1 Item:  Take to Program
        {
            MainWindow.takeToProgram = true;
            button1.Text = "Take to Program";
        }

        private void takeToPreviewToolStripMenuItem_Click(object sender, EventArgs e)  // ContextMenu1 Item:  Take to Preview
        {
            MainWindow.takeToProgram = false;
            button1.Text = "Take to Preview";
        }


        // MONITOR DRAG AND DROP - moveButton must be TRUE - Add this!

//        private void Form5_MouseDown(object sender, MouseEventArgs e)  // Monitor Mouse event down to start dragging
//        {
//            if (moveButton)
//            {
//                isDragging = true;
//                offsetX = e.X;
//                offsetY = e.Y;
//                button2.Visible = true;
//            }
//        }

//        private void Form5_MouseUp(object sender, MouseEventArgs e)  // Monitor Dragging is completed
//        {
//            isDragging = false;
//        }

//        private void Form5_MouseMove(object sender, MouseEventArgs e)  // Monitor is following mouse
//        {
//            if ((moveButton) && (isDragging))
//            {
//                button1.Left = e.X + button1.Left - offsetX;
//                button1.Top = e.Y + button1.Top - offsetY;
//            }
//        }

//        private void button2_Click(object sender, EventArgs e)  // Button2 stops drag/drop
//        {
//            moveButton = false;
//            button2.Visible = false;
//        }

   












    }
}
