using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Threading;
using System.Windows.Forms;

namespace VGV101
{
    public partial class DlgSetupWizard : Form
    {
        int nextFormNumber = 1;

        public DlgSetupWizard()  //Display Text on initilizing
        {
            InitializeComponent();
            label1.Text = "Fill in camera IP address, Camera Name and User Name/Password and save.  The 'Ping' button on the right will test the IP address from within Windows.  " +
                "The 'Test' button will communicate with the camera, try to turn it on, and list critical properties, such as if the Pan/Tilt/Zoom/Focus/Iris functions are enabled.  " +
                " Check these parameters if the camera doesn't seem to respond to click-and-dragging in the viewing monitor on the right of the Camera Settings menu.  " +
                "The 'List' button will print out an exhaustive list of all camera parameters in case the camera's behavior is still unexplained.  " +
                "If you click and drag inside the monitor menu on the right, the cameras should move.  " +
                "\n \nThen close the Camera Settings form and press 'Continue Setup' or 'Abandon Setup'.";
            DlgCameraSettings frm = new DlgCameraSettings();
            frm.Show();
            nextFormNumber = 2;
        }

        private void button1_Click(object sender, EventArgs e)  // Continue Button
        {
            if (nextFormNumber == 2)
            {
                label1.Text = "Form = 2 Now right-click in the main menu and set the background to a picture or floor plan of the room. (This step is optional.  " +
                    "\n \nThen press 'Continue Setup' or 'Abandon Setup'.";
            }
            if (nextFormNumber == 3) {
                DlgButtonSettings frm = new DlgButtonSettings();
                frm.Show();
                label1.Text = "Form = 3 Put up Form 21.  Right-click in the main menu and make a couple of shot buttons.  " + 
                    "Position them so they correspond to where people set.  Then right-click on the buttons and choose two camera shots for each person - " + 
                    "a primary shot and a secondary shot from a different camera." +
                    " \n \nThen close the form and press 'Continue Setup' or 'Abandon Setup'.";
            }
            if (nextFormNumber == 4)
            { 
              label1.Text = "Form - 4 Right-click in the main menu and make a couple of shot buttons.  Position them so they correspond to where people sit.  " + 
                    "Then right-click on the buttons and choose two camera shots for each person - a primary shot and a secondary shot from a different camera." + 
                    "\n \nThen press 'Continue Setup' or 'Abandon Setup'.";  }
            if (nextFormNumber == 5) { label1.Text = "Press 'Finish' to exit"; button1.Visible = false; button2.Text = "Finish";
            }
            nextFormNumber += 1;
        }

        private void button2_Click(object sender, EventArgs e)  // Abandon Setup/Finish Button
        {
            this.Close();
        }
    }
}


/*
         public Form19(string wizardText)  //Display Text on initilizing
        {
            InitializeComponent();
            // showNext = hShowNext;
            label1.Text = wizardText;
        }
 * */




//        private int button1_Click(object sender, EventArgs e)  // Continue Button
//        {
// int n = 1;
// return n;
// MessageBox.Show("Continue Setup");
// resultCode = resultType.CONTINUE;
// MessageBox.Show("Continue ResultType sent");
// this.Close();
//        }
