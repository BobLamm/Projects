/**
 * File: CameraObject.cs
 * 
 *	Copyright © 2016 by City Council Video.  All rights reserved.
 *
 *	$Id: /CameraObject.cs,v $
 */
/**
*	Provides an interface for an individual camera
*
*	Author:			Fred Koschara
*	Creation Date:	December twelfth, 2016
*	Last Modified:	December 20, 2016 @ 1:24 am
*
*	Revision History:
*	   Date		  by		Description
*	2016/12/20	wfredk	import into Video_Test_Fixture
*	2016/12/13	wfredk	original development
*		|						|
*	2016/12/12	wfredk	original development
*/
using System;
using System.Data;
using System.Net;

namespace Video_Test_Fixture
{
    public class CameraObject
    {
        public String ipAddress;
        public UInt16 port;
        public String ipAddrPort;
        public String userName;
        public String password;
        public String cameraName;
        public String manufacturer;
        public String model;
        public String homePanPosition;
        public String homeTiltPosition;
        public String homeZoomPosition;
        public String homeFocusPosition;
        public String homeIrisPosition;
        public String homeWhitePosition;
        public String homeGainPosition;
        public String homeBacklightPosition;
        public bool homeAutoFocusPreset;
        public bool homeAutoIrisPreset;
        public bool homeAutoWhitePreset;
        public bool homeAutoGainPreset;
        public bool homeAutoBacklightPreset;
        public String panSpeed;
        public String tiltSpeed;
        public String zoomSpeed;
        public String focusSpeed;
        public String holdFrames;
        public String status;

        private int errorStatus = 0;
        public int ErrorStatus
        {   get { return errorStatus; }
        }
        private string errorString = "";
        public string ErrorString
        {   get { return errorString; }
        }

        // --------------------------------------------------------------------

        // constructor
        public CameraObject(DataRow row)
        {
            ipAddrPort = row["IP_Address"].ToString();  // get camera IP address from camera file
            userName = row["User_Name"].ToString();  // get camera user name from camera file
            password = row["Password"].ToString();  // get camera password from camera file
            cameraName = row["Camera_Name"].ToString();
            manufacturer = row["Manufacturer"].ToString();
            model = row["Model"].ToString();
            homePanPosition = row["Home_Pan_Position"].ToString();
            homeTiltPosition = row["Home_Tilt_Position"].ToString();
            homeZoomPosition = row["Home_Zoom_Position"].ToString();
            homeFocusPosition = row["Home_Focus_Position"].ToString();
            homeIrisPosition = row["Home_Iris_Position"].ToString();
            homeWhitePosition = row["Home_White_Position"].ToString();
            homeGainPosition = row["Home_Gain_Position"].ToString();
            homeBacklightPosition = row["Home_Backlight_Position"].ToString();
            homeAutoFocusPreset = row["Home_Auto_Focus_Preset"].ToString() == "On";
            homeAutoIrisPreset = row["Home_Auto_Iris_Preset"].ToString() == "On";
            homeAutoWhitePreset = row["Home_Auto_White_Preset"].ToString() == "On";
            homeAutoGainPreset = row["Home_Auto_Gain_Preset"].ToString() == "On";
            homeAutoBacklightPreset = row["Home_Auto_Backlight_Preset"].ToString() == "On";
            panSpeed = row["Pan_Speed"].ToString();
            tiltSpeed = row["Tilt_Speed"].ToString();
            zoomSpeed = row["Zoom_Speed"].ToString();
            focusSpeed = row["Focus_Speed"].ToString();
            holdFrames = row["Hold_Frames"].ToString();
            status = row["Status"].ToString();
        }

        // --------------------------------------------------------------------

        // methods for saving camera data, used by GlobalConfig.SaveCameras()
        public static DataTable NewTable(string tableId)
        {   DataTable dt = null;

            try
        	{   dt=new DataTable(tableId);

                dt.Columns.Add("Camera_Number",typeof(string));
                dt.Columns.Add("IP_Address",typeof(string));
                dt.Columns.Add("User_Name",typeof(string));
                dt.Columns.Add("Password",typeof(string));
                dt.Columns.Add("Camera_Name",typeof(string));
                dt.Columns.Add("Manufacturer",typeof(string));
                dt.Columns.Add("Model",typeof(string));
                dt.Columns.Add("Home_Pan_Position",typeof(string));
                dt.Columns.Add("Home_Tilt_Position",typeof(string));
                dt.Columns.Add("Home_Zoom_Position",typeof(string));
                dt.Columns.Add("Home_Focus_Position",typeof(string));
                dt.Columns.Add("Home_Iris_Position",typeof(string));
                dt.Columns.Add("Home_White_Position",typeof(string));
                dt.Columns.Add("Home_Gain_Position",typeof(string));
                dt.Columns.Add("Home_Backlight_Position",typeof(string));
                dt.Columns.Add("Home_Auto_Focus_Preset",typeof(string));
                dt.Columns.Add("Home_Auto_Iris_Preset",typeof(string));
                dt.Columns.Add("Home_Auto_White_Preset",typeof(string));
                dt.Columns.Add("Home_Auto_Gain_Preset",typeof(string));
                dt.Columns.Add("Home_Auto_Backlight_Preset",typeof(string));
                dt.Columns.Add("Pan_Speed",typeof(string));
                dt.Columns.Add("Tilt_Speed",typeof(string));
                dt.Columns.Add("Zoom_Speed",typeof(string));
                dt.Columns.Add("Focus_Speed",typeof(string));
                dt.Columns.Add("Hold_Frames",typeof(string));
                dt.Columns.Add("Status",typeof(string));
            }
            catch (Exception ex)
            {  throw new Exception("Create DataTable failed: " + ex.ToString());
            }

            return dt;
        }
        public bool NewRow(DataTable dt,int camNumber)
        {
            errorStatus = 0;    // clear any previous error indication

            try
        	{	DataRow dr=dt.NewRow();

                dr["Camera_Number"] = camNumber.ToString();
                dr["IP_Address"] = ipAddrPort;
                dr["User_Name"] = userName;
                dr["Password"] = password;
                dr["Camera_Name"] = cameraName;
                dr["Manufacturer"] = manufacturer;
                dr["Model"] = model;
                dr["Home_Pan_Position"] = homePanPosition;
                dr["Home_Tilt_Position"] = homeTiltPosition;
                dr["Home_Zoom_Position"] = homeZoomPosition;
                dr["Home_Focus_Position"] = homeFocusPosition;
                dr["Home_Iris_Position"] = homeIrisPosition;
                dr["Home_White_Position"] = homeWhitePosition;
                dr["Home_Gain_Position"] = homeGainPosition;
                dr["Home_Backlight_Position"] = homeBacklightPosition;
                dr["Home_Auto_Focus_Preset"] = (homeAutoFocusPreset ? "On" : "Off");
                dr["Home_Auto_Iris_Preset"] = (homeAutoIrisPreset ? "On" : "Off");
                dr["Home_Auto_White_Preset"] = (homeAutoWhitePreset ? "On" : "Off");
                dr["Home_Auto_Gain_Preset"] = (homeAutoGainPreset ? "On" : "Off");
                dr["Home_Auto_Backlight_Preset"] = (homeAutoBacklightPreset ? "On" : "Off");
                dr["Pan_Speed"] = panSpeed;
                dr["Tilt_Speed"] = tiltSpeed;
                dr["Zoom_Speed"] = zoomSpeed;
                dr["Focus_Speed"] = focusSpeed;
                dr["Hold_Frames"] = holdFrames;
                dr["Status"] = status;

                dt.Rows.Add(dr);
        	}
        	catch (Exception ex)
        	{   errorStatus = -2;
                errorString = "Add row to DataTable failed: " + ex.ToString();
                return false;
        	}

        	return true;
        }

        // --------------------------------------------------------------------

        // send an arbitrary command to the camera
        public string CamOperation(string command)
        {   string response = "no response";
            errorStatus = 0;    // clear any previous error indication

            try
            {
                // put username and password into credentials
                var client = new WebClient { Credentials = new NetworkCredential(userName, password) };
                // make up webrequest to camera
                response = client.DownloadString("http://" + ipAddrPort + "/axis-cgi/" + command);
            }
            catch (Exception ex)
            {   errorStatus = -3;
                errorString = "Camera command failed: " + ex.ToString();
            }

            return response;
        }

        // send an arbitrary command from the /com/ folder to the camera
        public string CamCommand(string command)
        {   return CamOperation("com/" + command);
        }

        // --------------------------------------------------------------------

        // send a PTZ command to the camera
        public string PtzCommand(string command)
        {   return CamCommand("ptz.cgi?" + command);
        }

        // --------------------------------------------------------------------

        // query the camera for its current position
        public string GetPosition()
        {   return PtzCommand("query=position");
        }

        // command the camera to a specific PTZ setting
        public string GoTo(string pan, string tilt, string zoom)
        {   return PtzCommand("pan=" + pan + "&tilt=" + tilt + "&zoom=" + zoom);
        }

        // pan the camera
        public string Pan(string pan)
        {   return PtzCommand("rpan=" + pan);
        }
    }
}
//
// EOF: CameraObject.cs
