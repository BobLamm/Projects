﻿/**
 * File: CameraObject.cs
 * 
 *	Copyright © 2016-2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /CameraObject.cs,v $
 */
/**
*	Provides an interface for an individual camera
*
*	Author:			Fred Koschara
*	Creation Date:	December twelfth, 2016
*	Last Modified:	March 18, 2017 @ 12:42 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/03/18  wfredk  add members for scan height, width
*	2017/03/17  wfredk  add default constructor, documentation
*	                    add methods to set ipAddress, port
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
        public int scanLines;   // video height, pixels
        public int scanWidth;   // video width, pixels
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

        // default constructor
        public CameraObject()
        {   ipAddress="";
            port=80;
            ipAddrPort="";
            userName="";
            password="";
            cameraName="";
            manufacturer="";
            model="";
            homePanPosition="";
            homeTiltPosition="";
            homeZoomPosition="";
            homeFocusPosition="";
            homeIrisPosition="";
            homeWhitePosition="";
            homeGainPosition="";
            homeBacklightPosition="";
            homeAutoFocusPreset=false;
            homeAutoIrisPreset=false;
            homeAutoWhitePreset=false;
            homeAutoGainPreset=false;
            homeAutoBacklightPreset=false;
            panSpeed="";
            tiltSpeed="";
            zoomSpeed="";
            focusSpeed="";
            holdFrames="";
            scanLines = 0;
            scanWidth = 0;
            status = "< unknown >";
        }

        // constructor used when reading from a configuration file
        public CameraObject(DataRow row)
        {
            // get camera IP address+port from camera file
            SetIpPort(row["IP_Address"].ToString());
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
            scanLines = int.Parse(row["Scan_Lines"].ToString());
            scanWidth = int.Parse(row["Scan_Width"].ToString());
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
                dt.Columns.Add("Scan_Lines",typeof(string));
                dt.Columns.Add("Scan_Width",typeof(string));
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
                dr["Scan_Lines"] = scanLines;
                dr["Scan_Width"] = scanWidth;
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

        /// <summary>
        /// set the IP address for the camera
        /// 
        /// The ipAddrPort string is set using the new IP address and the
        /// existing port number.
        /// </summary>
        /// <param name="newIpAddress">string, new IP address</param>
        /// <returns>bool, true=SUCCESS</returns>
        public bool SetIpAddress(string newIpAddress)
        {
            ipAddrPort = (ipAddress = newIpAddress) + ":" + port;
            return true;
        }
        /// <summary>
        /// set the port for accessing the camera
        /// 
        /// The ipAddrPort string is set using the existing IP address and the
        /// new port number.
        /// </summary>
        /// <param name="newPort">string, new port number</param>
        /// <returns>bool, true=SUCCESS</returns>
        public bool SetPort(string newPort)
        {
            try
            {
                port = UInt16.Parse(newPort);
            }
            catch
            {
                return false;
            }
            ipAddrPort = ipAddress + ":" + port;
            return true;
        }
        /// <summary>
        /// set the port for accessing the camera
        /// 
        /// The ipAddrPort string is set using the existing IP address and the
        /// new port number.
        /// </summary>
        /// <param name="newPort">UInt16, new port number</param>
        /// <returns>bool, true=SUCCESS</returns>
        public bool SetPort(UInt16 newPort)
        {
            ipAddrPort = ipAddress + ":" + (port = newPort);
            return true;
        }
        /// <summary>
        /// set the IP address and port number for accessing the camera
        /// 
        /// The ipAddrPort string is set using the passed parameters.
        /// </summary>
        /// <param name="newIpAddress">string, new IP address</param>
        /// <param name="newPort">string, new port number</param>
        /// <returns>bool, true=SUCCESS</returns>
        public bool SetIpPort(string newIpAddress,string newPort)
        {
            try
            {   port = UInt16.Parse(newPort);
            }
            catch
            {   return false;
            }
            ipAddrPort = (ipAddress = newIpAddress) + ":" + port;
            return true;
        }
        /// <summary>
        /// set the IP address and port number for accessing the camera
        /// 
        /// The ipAddrPort string is set using the passed parameters.
        /// </summary>
        /// <param name="newIpAddress">string, new IP address</param>
        /// <param name="newPort">UInt16, new port number</param>
        /// <returns>bool, true=SUCCESS</returns>
        public bool SetIpPort(string newIpAddress,UInt16 newPort)
        {
            ipAddrPort = (ipAddress = newIpAddress) + ":" + (port = newPort);
            return true;
        }
        /// <summary>
        /// set the IP address and port number for accessing the camera
        /// 
        /// The ipAddress string and port number member variables are
        /// set by parsing the passed string, separating it on the ':'
        /// character which must occur once in the string.   The IP
        /// address must be the first part of the string, the port number
        /// must be the second part.
        /// </summary>
        /// <param name="newIpAddress">string, new IP address:port</param>
        /// <returns>bool, true=SUCCESS</returns>
        public bool SetIpPort(string newIpPort)
        {
            String[] parts = newIpPort.Split(':');  // separate into components
            if (parts.Length != 2)
                return false;
            try
            {   port = UInt16.Parse(parts[1]);   // set port
            }
            catch
            {   return false;
            }
            ipAddrPort = newIpPort;
            ipAddress = parts[0];   // set ipAddress
            return true;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// send an arbitrary command to the camera
        /// 
        /// The URL used to access the camera is constructed from the IP/port
        /// configured for the camera, "/axis-cgi/", and the passed command.
        /// The access credentials are uaed when accessing the constructed URL.
        /// </summary>
        /// <param name="command">string, command to send to the camera</param>
        /// <returns>string, camera response</returns>
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
        /// <summary>
        /// send an arbitrary command from the /com/ folder to the camera
        /// 
        /// The URL used to access the camera is the configured IP/port for the
        /// camera, "/axis-cgi/com/", plus the passed command.
        /// The access credentials are uaed when accessing the constructed URL.
        /// </summary>
        /// <param name="command">string, command to send to the camera</param>
        /// <returns>string, camera response</returns>
        public string CamCommand(string command)
        {   return CamOperation("com/" + command);
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// send a PTZ command to the camera
        /// 
        /// The URL used to access the camera is the configured IP/port for the
        /// camera, "/axis-cgi/com/ptz.cgi?", plus the passed command.
        /// The access credentials are uaed when accessing the constructed URL.
        /// </summary>
        /// <param name="command">string, PTZ command for the camera</param>
        /// <returns>string, camera response</returns>
        public string PtzCommand(string command)
        {   return CamCommand("ptz.cgi?" + command);
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// query the camera for its current position
        /// </summary>
        /// <returns>string, camera response</returns>
        public string GetPosition()
        {   return PtzCommand("query=position");
        }

        /// <summary>
        /// command the camera to a specific PTZ setting
        /// </summary>
        /// <param name="pan">string, camera azimuth angle</param>
        /// <param name="tilt">string, camera altitude angle</param>
        /// <param name="zoom">string, camera zoom percentage</param>
        /// <returns>string, camera response</returns>
        public string GoTo(string pan, string tilt, string zoom)
        {   return PtzCommand("pan=" + pan + "&tilt=" + tilt + "&zoom=" + zoom);
        }

        /// <summary>
        /// pan the camera
        /// </summary>
        /// <param name="pan">string, camera azimuth angle</param>
        /// <returns>string, camera response</returns>
        public string Pan(string pan)
        {   return PtzCommand("rpan=" + pan);
        }
    }
}
//
// EOF: CameraObject.cs
