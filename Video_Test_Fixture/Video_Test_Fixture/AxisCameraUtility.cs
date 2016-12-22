/**
 * File: AxisCameraUtility.cs
 * 
 *	Copyright © 2016 by City Council Video.  All rights reserved.
 *
 *	$Id: /AxisCameraUtility.cs,v $
 */
/**
*	Provides common Axis camera functions
*
*	Author:			Fred Koschara
*	Creation Date:	December thirteenth, 2016
*	Last Modified:	December 13, 2016 @ 10:32 pm
*
*	Revision History:
*	   Date		  by		Description
*	2016/12/13	wfredk	original development
*/
using System;
using System.Drawing;
using System.Windows.Forms;

using AXISMEDIACONTROLLib;
using Video_Test_Fixture;

namespace Utility.AxisCamera
{
    public class AxisCameraUtility
    {
        public AxisCameraUtility()
        {
        }

        // Initialize camera monitor and feed camera into it
        public static void SetUpCameraMonitor(CameraObject camera, AxAXISMEDIACONTROLLib.AxAxisMediaControl AMC, Label txtCamStatus)
        {   bool bCaught = false;   // used to block second exception MessageBox
            GlobalConfig cfg = GlobalConfig.Instance;

            AMC.Visible = true;     // make the monitor visible

            try
            {   AMC.Stop();         // stop any running streams

                // Set the PTZ properties
                AMC.PTZControlURL = "http://" + camera.ipAddrPort + "/axis-cgi/com/ptz.cgi";
//                AMC.PTZControlURL = "http://" + dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["IP_Address"].Index].Value.ToString() + "/axis-cgi/com/ptz.cgi";
                AMC.UIMode = "ptz-relative";  // alternate is "ptz-absolute"

                // Enable PTZ-position presets from AMC context menu
                AMC.PTZPresetURL = "http://" + camera.ipAddrPort + "/axis-cgi/param.cgi?usergroup=anonymous&action=list&group=PTZ.Preset.P0";
//                AMC.PTZPresetURL = "http://" + dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["IP_Address"].Index].Value.ToString() + "/axis-cgi/param.cgi?usergroup=anonymous&action=list&group=PTZ.Preset.P0";

                //AMC.OneClickZoom =        // Enable one-click-zoom
                AMC.EnableJoystick =        // Enable joystick support
                AMC.EnableAreaZoom = true;  // Enable area zoom

                AMC.EnableOverlays = true;  // Set overlay settings
                AMC.ClientOverlay = (int)AMC_OVERLAY.AMC_OVERLAY_CROSSHAIR | (int)AMC_OVERLAY.AMC_OVERLAY_VECTOR | (int)AMC_OVERLAY.AMC_OVERLAY_ZOOM;

                // Do not show the status bar and the tool bar in the AXIS Media Control
                AMC.ShowStatusBar = AMC.ShowToolbar = false;
                AMC.StretchToFit = AMC.MaintainAspectRatio = AMC.EnableContextMenu = true;
                AMC.ToolbarConfiguration = "+ptz";  // "default,-mute,-volume,+ptz"

                // Set the media URL
                AMC.MediaURL = "http://" + camera.ipAddrPort + "/axis-cgi/mjpg/video.cgi";
                AMC.MediaUsername = camera.userName;
                AMC.MediaPassword = camera.password;
/*
                AMC.MediaURL = "http://" + dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["IP_Address"].Index].Value.ToString() + "/axis-cgi/mjpg/video.cgi";
                AMC.MediaUsername = dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["User_Name"].Index].Value.ToString();
                AMC.MediaPassword = dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["Password"].Index].Value.ToString();
*/
                AMC.Play();                 // Start the download of the mjpeg stream from the Axis camera/video server
                txtCamStatus.BackColor = Color.LawnGreen;
            }
            catch (ArgumentException ArgEx)
            {   MessageBox.Show(ArgEx.Message, "Monitor Error");
                bCaught = true;
                throw new Exception();
/*
                txtCamStatus.BackColor = Color.Red;
                dataGridView1.Rows[camNumber].Cells[dataGridView1.Columns["Status"].Index].Value = "Error!";
*/
            }
            catch (Exception Ex)
            {   if (!bCaught)
                    MessageBox.Show(Ex.Message, "Monitor Error");
                txtCamStatus.BackColor = Color.Red;
                camera.status = "Error!";
            }
        }
    }
}
//
// EOF: AxisCameraUtility.cs
