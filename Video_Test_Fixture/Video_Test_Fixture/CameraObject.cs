/**
 * File: CameraObject.cs
 * 
 *	Copyright © 2016-2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /CameraObject.cs,v $
 */
/**
*	Provides an interface to configuration data for an individual camera
*
*	Author:			Fred Koschara
*	Creation Date:	December twelfth, 2016
*	Last Modified:	April 13, 2017 @ 9:30 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/04/13	wfredk	class is now derived from BaseVgvVideoSrc
*	2017/04/13	wfredk	fields reordered for future class rearchitecting
*	2017/03/22	wfredk	add documentation
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
//using System.Runtime.InteropServices;
//using System.Windows.Forms;

using Utility.VgvUtility;

using AlaxInfoIpVideoSource;
using DirectShowLib;

namespace Video_Test_Fixture
{
    /// <summary>
    /// provides an interface to configuration data for an individual camera
    /// </summary>
    public class CameraObject : BaseVgvVideoSrc
    {
        /// <summary>
        /// internal name for the camera, e.g., its physical location
        /// </summary>
        public String cameraName;
        /// <summary>
        /// camera manufacturer's name
        /// </summary>
        public String manufacturer;
        /// <summary>
        /// camera model information
        /// </summary>
        public String model;
        /// <summary>
        /// video height, pixels
        /// </summary>
        public int scanLines;
        /// <summary>
        /// video width, pixels
        /// </summary>
        public int scanWidth;
        /// <summary>
        /// last status string returned by the camera
        /// </summary>
        public String status;

        // --------------------------------------------------------------------
        // video filter graph properties, BaseVgvVideoSrc derivation

        /// <summary>
        /// bool, set if the camera is in a paused state
        /// </summary>
        protected bool bPaused = false;

        private GlobalConfig cfg = null;
        private GlobalConfig Cfg
        {   get
            {   if (cfg == null)
                    cfg = GlobalConfig.Instance;
                return cfg;
            }
        }

        private IJpegVideoSourceFilter rawFilter = null;
        protected IBaseFilter srcFilter = null;

        protected IBaseFilter outFilter = null;

        protected WndMonitor wndPreview = null;

        // ---------------------------------------------------------------------
        // class IpCameraObject

        /// <summary>
        /// IP address where the camera is attached to the network
        /// </summary>
        public String ipAddress;
        /// <summary>
        /// port number the camera is attached to at its IP address
        /// </summary>
        public UInt16 port;
        /// <summary>
        /// Ip.ad.dr.ess:port for accessing the camera
        /// </summary>
        public String ipAddrPort;
        /// <summary>
        /// username to be used for accessing the camera
        /// </summary>
        public String userName;
        /// <summary>
        /// password to be used for accessing the camera
        /// </summary>
        public String password;

        // ---------------------------------------------------------------------
        // class Axis[mode]IpCameraObject

        /// <summary>
        /// home position pan command string
        /// </summary>
        public String homePanPosition;
        /// <summary>
        /// home position tilt command string
        /// </summary>
        public String homeTiltPosition;
        /// <summary>
        /// home position zoom command string
        /// </summary>
        public String homeZoomPosition;
        /// <summary>
        /// home position focus command string
        /// </summary>
        public String homeFocusPosition;
        /// <summary>
        /// home iris setting command string
        /// </summary>
        public String homeIrisPosition;
        /// <summary>
        /// home white balance setting command string
        /// </summary>
        public String homeWhitePosition;
        /// <summary>
        /// home gain setting command string
        /// </summary>
        public String homeGainPosition;
        /// <summary>
        /// home backlight compensation setting command string
        /// </summary>
        public String homeBacklightPosition;
        /// <summary>
        /// use auto focus at home position
        /// </summary>
        public bool homeAutoFocusPreset;
        /// <summary>
        /// use auto iris preset at home position
        /// </summary>
        public bool homeAutoIrisPreset;
        /// <summary>
        /// use auto white balance at home position
        /// </summary>
        public bool homeAutoWhitePreset;
        /// <summary>
        /// use auto gain adjustment at home position
        /// </summary>
        public bool homeAutoGainPreset;
        /// <summary>
        /// use auto backlight compensation at home position
        /// </summary>
        public bool homeAutoBacklightPreset;
        /// <summary>
        /// camera pan speed setting
        /// </summary>
        public String panSpeed;
        /// <summary>
        /// camera tilt speed setting
        /// </summary>
        public String tiltSpeed;
        /// <summary>
        /// camera zoom speed setting
        /// </summary>
        public String zoomSpeed;
        /// <summary>
        /// camera focus speed setting
        /// </summary>
        public String focusSpeed;
        /// <summary>
        /// camera hold frames setting
        /// </summary>
        public String holdFrames;

        // --------------------------------------------------------------------

        /// <summary>
        /// default constructor
        /// 
        /// port is set to 80, booleans to false, strings to empty except
        /// status which is set to "&lt; unknown &gt;"
        /// </summary>
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

        /// <summary>
        /// constructor used when reading from a configuration file
        /// </summary>
        /// <param name="row">DataRow, the configuration read from the file</param>
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

        /// <summary>
        /// constructs an empty DataTable object properly structured to use
        /// for writing camera configuration data to disk
        /// </summary>
        /// <param name="tableId">string, the table name property to use</param>
        /// <returns>DataTable, the object to use for writing the file</returns>
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
        /// <summary>
        /// adds the configuration data for a camera to the DataTable that will
        /// be used to write the configuration to a disk file
        /// </summary>
        /// <param name="dt">DataTable, the structure used for writing to disk</param>
        /// <param name="camNumber">int, zero-based camera index in the file</param>
        /// <returns></returns>
        public bool NewRow(DataTable dt,int camNumber)
        {   errorStatus = 0;    // clear any previous error indication

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
        	{   errorStatus = VgvErrorCode.FAILED_ADD_DATATABLE_ROW;
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
        {   ipAddrPort = (ipAddress = newIpAddress) + ":" + port;
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
        {   try
            {   port = UInt16.Parse(newPort);
            }
            catch
            {   return false;
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
        {   ipAddrPort = ipAddress + ":" + (port = newPort);
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
        {   try
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
        {   ipAddrPort = (ipAddress = newIpAddress) + ":" + (port = newPort);
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
        /// <param name="newIpPort">string, new IP address:port</param>
        /// <returns>bool, true=SUCCESS</returns>
        public bool SetIpPort(string newIpPort)
        {   String[] parts = newIpPort.Split(':');  // separate into components
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
            {   // put username and password into credentials
                var client = new WebClient { Credentials = new NetworkCredential(userName, password) };
                // make up webrequest to camera
                response = client.DownloadString("http://" + ipAddrPort + "/axis-cgi/" + command);
            }
            catch (Exception ex)
            {   errorStatus = VgvErrorCode.FAILED_CAM_COMMAND;
                errorString = "Camera command failed: " + ex.ToString();
            }

            return response;
        }

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

        // --------------------------------------------------------------------
        // video filter graph methods, BaseVgvVideoSrc derivation
        // --------------------------------------------------------------------

        /// <summary>
        /// property (accessor) for this camera's source filter object
        /// </summary>
        public IBaseFilter SrcFilter
        {   get
            {   if (srcFilter == null)
                {   if (graph != null)
                    {   int hr = 0;
                        string userPass = (userName != null && password != null)
                                        ? userName + ":" + password + "@"
                                        : "";
                        string location = "http://" + userPass + ipAddrPort + "/axis-cgi/mjpg/video.cgi?resolution="
                                        + scanWidth + "x" + scanLines;

                        //graph builder
                        ICaptureGraphBuilder2 pBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
                        hr = pBuilder.SetFiltergraph(graph);
                        VgvUtil.checkHR(hr,"Can't SetFiltergraph");

                        //add Alax.Info JPEG Video Source
                        Guid CLSID_VideoSource = new Guid("{A8DA2ECB-DEF6-414D-8CE2-E651640DBA4F}");    // IpVideoSource.dll
                        rawFilter = (IJpegVideoSourceFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_VideoSource));
                        srcFilter = rawFilter as IBaseFilter;
                        hr = (graph as IFilterGraph2).AddFilter(srcFilter,"Alax.Info JPEG Video Source");
                        VgvUtil.checkHR(hr,"Can't add Alax.Info JPEG Video Source to graph");

                        rawFilter.Location = location;
                        rawFilter.Width = scanWidth;
                        rawFilter.Height = scanLines;
                    }
                }
                return srcFilter;
            }
        }

        public IBaseFilter InsertBridgeSink(VgvBridge bridge)
        {   if (outFilter != null)
                return outFilter;
            try
            {   outFilter = (IBaseFilter)bridge.Bridge.InsertSinkFilter(Graph);
            }
            catch (Exception ex)
            {   errorStatus = VgvErrorCode.FAILED_CREATE_BRIDGE_FILTER;
                errorString = "Create sink filter for " + cameraName + "failed: " + ex.ToString();
                return null;
            }
            return outFilter;
        }
        public IBaseFilter OutFilter
        {   get
            {   return outFilter;
            }
        }

        // --------------------------------------------------------------------
        // query configuration

        /// <summary>
        /// returns the type of input that is configured
        /// </summary>
        /// <returns>string, "camera"</returns>
        public override string getType()
        {   return "camera";
        }
        /// <summary>
        /// returns the camera name and its IP address/port
        /// 
        /// If no input is configured, it will be "no camera source"
        /// </summary>
        /// <returns>string, input name</returns>
        public override string getId()
        {   return cameraName + " @ " + ipAddrPort;
        }

        // --------------------------------------------------------------------
        // operational controls

        /// <summary>
        /// opens a preview window for this video source
        /// 
        /// If a preview window was already opened for this video source, it is
        /// made visible and brought to the top of the Z stack.
        /// 
        /// This method should be overridden in derived classes.  The derived method
        /// should call this base implementation for the core window functionality.
        /// </summary>
        /// <returns>bool, true=preview opened successfully</returns>
        public override bool preview()
        {
            // TODO: open the camera in a preview window
            wndPreview = new WndMonitor(/*SrcFilter,*/this);
            wndPreview.Show();

            return base.preview();     // bring visible window to top of Z stack
        }

        /// <summary>
        /// shows or hides the output of this video source when it's an overlay
        /// If this video source is NOT an overlay, the returned value will always
        /// be the same as the passed one:  "show" succeeds, "hide" fails
        /// 
        /// This method may be overridden in derived classes.
        /// </summary>
        /// <param name="show">bool, true=show the overlay, false=hide it</param>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool show(bool show)
        {
            if (!bOverlay)
                return show;

            // TODO

            return false;
        }

        /// <summary>
        /// starts the output from this video source
        /// 
        /// If the video source was previously paused, it will resume from where
        /// it left off.  Otherwise, output starts from the beginning of the stream.
        /// 
        /// This method may be overridden in derived classes.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool start()
        {
            if (bPaused)
            {
                // TODO
                bPaused = false;
            }
            else
            {
                // TODO
            }

            return false;
        }

        /// <summary>
        /// temporarily stops the video output from this source
        /// 
        /// If start() is called after the output has been paused by this method,
        /// output will resume from where it was stopped.
        /// 
        /// This method may be overridden in derived classes.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool pause()
        {
            // TODO
            bPaused = true;

            return false;
        }

        /// <summary>
        /// stops output from this video source, closes any associated files or handles
        /// 
        /// This method may be overridden in derived classes.
        /// </summary>
        /// <returns>bool, true=state changed successfully</returns>
        public override bool stop()
        {
            // TODO

            return false;
        }
    }
}
//
// EOF: CameraObject.cs
