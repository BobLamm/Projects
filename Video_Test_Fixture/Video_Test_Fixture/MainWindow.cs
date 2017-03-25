/**
 * File: MainWindow.cs
 * 
 *	Copyright © 2016-2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /Video_Test_Fixture/MainWindow.cs,v $
 */
/**
*	Main application window for the video test fixture
*
*	Author:			Bob Lamm
*	Creation Date:	October twentieth, 2016
*	Last Modified:	March 19, 2017 @ 7:00 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/03/19	wfredk	call StopAllRunningCameras() in connectToCameras_Click()
*	                    add camera preview buttons
*	2017/03/18	wfredk	call DlgCamConfig in connectToCameras_Click()
*	2016/12/20	wfredk	break text into lines to avoid massive scrolling
*	2016/12/20	wfredk	change PictureBox to AMC video control
*	2016/12/20	wfredk	rename program to Video_Test_Fixture, add documentation
*	2016/10/20	blamm	original development ("Prototype for Fred" C# solution)
*/
using System;
using System.Windows.Forms;

namespace Video_Test_Fixture
{
    /// <summary>
    /// main window for the test fixture application
    /// </summary>
    public partial class MainWindow : Form
    {
        CameraVgvVideoSrc srcCam1=null;
        CameraVgvVideoSrc srcCam2=null;

        /// <summary>
        /// constructor, creates the form elements
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void pictureBoxForVideo_Click(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnClickEvent e)
        {
            MessageBox.Show("Video should show up here.  "
                            + "The 'Camera 1' and 'Camera 2' Buttons should switch between the two cameras.  "
                            + "As I remember, Craig did it by putting them on separate layers and changing the opacities.  "
                            + "The 'Superimpose Graphic' should put a bitmap (such as TGA) over the picture, "
                                + "such as the background artwork for a subtitle.  "
                            + "I can give you a bit map with an alpha channel to play with.  "
                            + "The 'Superimpose Text' button should put some text up on top of the graphic.  "
                            + "This way I can have the final graphics be editable.");
        }

        private void camera1Button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Changes video in monitor to stream from Camera 1");
        }

        private void camera2Button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Changes video in monitor to stream from Camera 2");
        }

        private void addGraphicButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Superimposes a bitmap (such as a .TGA file) over the live video.  "
                            + "I can give you a sample.");
        }

        private void addTextButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Superimposes editable text over the live video and graphic.  "
                            + "This way the text in a subtitle remains editable.");
        }

        private void connectToCameras_Click(object sender, EventArgs e)
        {
            //
            // **** REALLY IMPORTANT ****
            StopAllRunningCameras();

            DlgCamConfig dlg = new DlgCamConfig();
            dlg.ShowDialog();

/*            MessageBox.Show("I can give you the C# code that I use to connect to the camera in California.  "
                            + "Also the code I use to send it web commands.  "
                            + "The documentation for the camera comes in two sets:  \n\n"
                            + "1) A .CHM file for the Active Object that displays the "
                            + "unified camera control user interface I was using \n\n"
                            + "2) A bunch of 'VAPIX' API documents that explain the "
                            + "web request commands and isolated streaming technology.");
*/
        }

        /// <summary>
        /// This method MUST be called before running the Camera Configuration dialog to prevent
        /// invalid CameraObject references which *WOULD* cause "incorrect program behavior"
        /// </summary>
        private void StopAllRunningCameras()
        {
            // TODO
            // stop all running cameras
            // invalidate all CameraObject references
        }

        private void btnPreview1_Click(object sender,EventArgs e)
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            if (srcCam1 == null)
                srcCam1 = new CameraVgvVideoSrc(cfg.Camera(0));

            srcCam1.preview();
        }

        private void btnPreview2_Click(object sender,EventArgs e)
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            if (srcCam2 == null)
                srcCam2 = new CameraVgvVideoSrc(cfg.Camera(1));

            srcCam2.preview();
        }
    }
}
//
// EOF: MainWindow.cs
