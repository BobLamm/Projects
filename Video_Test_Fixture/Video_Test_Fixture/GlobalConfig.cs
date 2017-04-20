/**
 * File: GlobalConfig.cs
 * 
 *	Copyright © 2016-2017 by City Council Video.  All rights reserved.
 *
 *	$Id: /GlobalConfig.cs,v $
 */
/**
*	Provides an interface to program configuration files
*
*	Author:			Fred Koschara
*	Creation Date:	December tenth, 2016
*	Last Modified:	April 13, 2017 @ 9:53 pm
*
*	Revision History:
*	   Date		  by		Description
*	2017/04/13	wfredk	tweaked error reporting
*	2017/04/10	wfredk	MuxGraph member replaces DirectShow graph member
*	2017/03/22	wfredk	add [more] documentation
*	2017/03/19	wfredk	add global filter graph object
*	2017/03/18	wfredk	GetCameras() internal documentation corrected
*	                    support adding and deleting cameras
*	2017/01/09	wfredk	read Registry inside Init() method (restored)
*	2016/12/20	wfredk	import into Video_Test_Fixture
*	2016/12/13	wfredk	original development
*		|						|
*	2016/12/10	wfredk	original development
*/
using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;  // for XML

using DirectShowLib;

using Utility.ModifyRegistry;
using Utility.VgvUtility;

namespace Video_Test_Fixture
{
    /// <summary>
    /// global configuration object for the program
    /// </summary>
    public sealed class GlobalConfig
    {
        // singleton implementation
        // based on http://csharpindepth.com/Articles/General/Singleton.aspx
        private GlobalConfig()
        {
        }
        /// <summary>
        /// returns a reference to the single instance of this object that exists
        /// </summary>
        public static GlobalConfig Instance
        {
            get { return Nested.instance; }
        }
        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly GlobalConfig instance = new GlobalConfig();
        }
        // --------------------------------------------------------------------

        private bool bInitialized = false;
        private XmlReaderSettings xmlReaderSettings = null;

        private String cfgRoot;
        /// <summary>
        /// returns the root path of the configuration directory tree on disk
        /// </summary>
        public String CfgRoot
        {
            get { return cfgRoot; }
        }
        private String logRoot;
        /// <summary>
        /// returns the root path of the log directory tree on disk
        /// </summary>
        public String LogRoot
        {
            get { return logRoot; }
        }
        private String mediaRoot;
        /// <summary>
        /// returns the root path of the media directory tree on disk
        /// </summary>
        public String MediaRoot
        {
            get { return mediaRoot; }
        }

        /// <summary>
        /// initializes the global configuration object
        /// 
        /// This method is called once during program startup.  Any subsequent
        /// calls will be silently ignored.
        /// 
        /// If CfgRoot, LogRoot and/or MediaRoot keys are found in the Registry
        /// HKEY_LOCAL_MACHINE\SOFTWARE hive for this program, they are used.
        /// Otherwise, the paths default to these values:
        ///     CfgRoot     C:\VGV Software\Configuration\
        ///     LogRoot     C:\VGV Software\Logs\
        ///     MediaRoot   C:\VGV Customer Media\
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            if (bInitialized)
                return false;

            ModifyRegistry regData = new ModifyRegistry();
            // MessageBox.Show(regData.SubKey);    // "SOFTWARE\Video_Test_Fixture"

            // ensure paths end with a trailing slash
            cfgRoot = VgvUtil.TerminatePath(regData.ReadString("CfgRoot", @"C:\VGV Software\Configuration\"));
            logRoot = VgvUtil.TerminatePath(regData.ReadString("LogRoot", @"C:\VGV Software\Logs\"));
            mediaRoot = VgvUtil.TerminatePath(regData.ReadString("MediaRoot", @"C:\VGV Customer Media\"));

            xmlReaderSettings = new XmlReaderSettings();

            bInitialized = true;

            return true;
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// application-defined message to notify app of filtergraph events
        /// </summary>
        public const int WM_GRAPHNOTIFY = 0x8000 + 1;

        MuxGraph graph = null;
        /// <summary>
        /// returns a handle to the global filter graph for the program
        /// 
        /// The filter graph is instantiated the first time this property
        /// is accessed.
        /// </summary>
        public MuxGraph Graph
        {   get
            {   if (graph == null)
                {   try
                    {   graph = new MuxGraph();
                    }
                    catch (COMException ex)
                    {   MessageBox.Show("COM Error: " + ex.ToString(),"ERROR");
                    }
                    catch (Exception ex)
                    {   MessageBox.Show(ex.ToString(),"ERROR");
                    }
                }
                return graph;
            }
        }

        // --------------------------------------------------------------------

        /// <summary>
        /// reads an XML file in Current configuration folder into the DataGridView that is passed
        /// </summary>
        /// <param name="fileName">string, filename (without ".xml" extension) to read</param>
        /// <param name="dataGridView">DataGridView, where to return the data read from the file</param>
        /// <returns>bool, true=successful operation</returns>
        public bool GetCurrentXml(string fileName, DataGridView dataGridView)
        {
            bool retVal = true;
            DataSet ds;
            XmlReader xmlFile = null;

            try  // Read Button Settings from Buttons.xml file into dataGridView1
            {
                xmlFile = XmlReader.Create(cfgRoot+"Current\\"+fileName+".xml", xmlReaderSettings);
                ds = new DataSet();
                ds.ReadXml(xmlFile);
                dataGridView.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not read Current "+fileName.Replace("_", " ")+" file into memory:\n"+ex.ToString(),
                                "GetCurrentXml Exception");
                retVal = false;
            }

            if (xmlFile != null)
                xmlFile.Close();

            return retVal;
        }

        /// <summary>
        /// writes an XML file in Current configuration folder from the dataGridView that is passed
        /// </summary>
        /// <param name="fileName">string, filename (without ".xml" extension) to read</param>
        /// <param name="dataGridView">DataGridView, data to be written to the file</param>
        /// <returns>bool, true=successful operation</returns>
        public bool WriteCurrentXml(string fileName,DataGridView dataGridView)
        {
            bool retVal = true;

            try
            {
                DataTable dt = new DataTable("Record");
                dt = (DataTable)dataGridView.DataSource;
                dt.WriteXml(cfgRoot + "Current\\" + fileName + ".xml", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not write Current " + fileName.Replace("_", " ") + " from memory:\n" + ex.ToString(),
                                "WriteCurrentXml Exception");
                retVal = false;
            }

            return retVal;
        }

        // --------------------------------------------------------------------

        private CameraObject[] Cameras=null;
        private int nCameras = 0;
        /// <summary>
        /// returns the number of cameras known to the application
        /// </summary>
        public int NumCameras
        {   get
            {
                if (Cameras == null)
                    GetCameras();
                return nCameras;
            }
        }

        /// <summary>
        /// adds a camera to the application's list
        /// 
        /// The camera configuration file is updated
        /// </summary>
        /// <param name="newCam">CameraObject, the camera's configuration data</param>
        /// <returns>bool, true=successful operation</returns>
        public bool AddCamera(CameraObject newCam)
        {
            return DoSaveCameras(-1,newCam);
        }

        /// <summary>
        /// returns the configuration data for a particular camera
        /// 
        /// The first time this method is called, the camera data is read
        /// from the configuration file.
        /// 
        /// N.B.:   On error, this method returns a null object.
        /// 
        /// An error is returned if the requested camera number is less than
        /// zero, or greater than the number of cameras configured.
        /// </summary>
        /// <param name="camNum">int, zero-based camera index number</param>
        /// <returns>CameraObject, the camera configuration</returns>
        public CameraObject Camera(int camNum)
        {
            if (Cameras == null)
                GetCameras();

            if (camNum < 0 || camNum > nCameras)
            {
                MessageBox.Show("No camera # " + camNum + " available","ERROR");
                return null;
            }

            return Cameras[camNum];
        }

        /// <summary>
        /// removes a camera configuration from the application's list
        /// 
        /// The camera configuration file is updated
        /// </summary>
        /// <param name="nDelete">int, zero-based index of the camera to delete</param>
        /// <returns>bool, true=successful operation</returns>
        public bool DeleteCamera(int nDelete)
        {
            return DoSaveCameras(nDelete,null);
        }

        /// <summary>
        /// common method used to add or delete cameras, or update the camera
        /// configuration file
        /// 
        /// The nDel parameter must be a positive integer between zero and the
        /// number of cameras defined to delete a camera configuration.
        /// 
        /// If a non-null newCam object is passed, it is added to the camera
        /// configuration after the other existing cameras have been written.
        /// 
        /// N.B.:   It is an error to try adding and deleting cameras at the
        ///         same time.
        /// </summary>
        /// <param name="nDel">int, zero based index of camera to delete</param>
        /// <param name="newCam">CameraObject, new camera to add to the file</param>
        /// <returns>bool, true=successful operation</returns>
        private bool DoSaveCameras(int nDel,CameraObject newCam)
        {
            bool retVal = true;
            if ((Cameras == null || nCameras == 0) && newCam==null)
            {
                MessageBox.Show("No cameras to save!","ERROR");
                return false;
            }
            if (nDel >= nCameras)
            {
                MessageBox.Show("Bad delete index " + nDel + " found in DoCameraSave()","ERROR");
                return false;
            }
            if (nDel>=0 && newCam!=null)
            {
                MessageBox.Show("Simultaneous add and delete not supported","ERROR");
                return false;
            }
            try
            {
                int cnt;
                DataTable dt = CameraObject.NewTable("Record");
                for (cnt = 0; cnt < nCameras; cnt++)
                {
                    if (nDel >= 0 && nDel == cnt)
                        continue;   // don't save the one being deleted
                    Cameras[cnt].NewRow(dt,cnt);
                    if (Cameras[cnt].ErrorStatus() != VgvErrorCode.NO_ERROR)
                        throw new Exception(Cameras[cnt].ErrorString());
                }
                if (newCam != null) // adding a new camera
                {
                    newCam.NewRow(dt,cnt);
                    if (newCam.ErrorStatus() != VgvErrorCode.NO_ERROR)
                        throw new Exception(newCam.ErrorString());
                }
                dt.WriteXml(cfgRoot + "Current\\Camera_Settings.xml",true);
                if (nDel >= 0 || newCam != null)    // if camera deleted or added
                    ResetCameraConfig();    // in-memory array is now invalid
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"ERROR Saving Cameras");
                retVal = false;
            }
            return retVal;
        }

        /// <summary>
        /// discards the in-memory copy of the camera configuration data after
        /// the disk file has been modified to ensure the correct information
        /// will be used for accessing cameras:  The next time the code calls
        /// the Camera() method, the configuration is freshly read from disk
        /// into memory as a clean copy.
        /// 
        /// For example, this method is called after saving camera settings in
        /// DlgCameraSettings() using DataGridView.
        /// </summary>
        public void ResetCameraConfig()
        {   try
            {   Cameras = null;
                nCameras = 0;
            }
            catch (Exception ex)
            {   MessageBox.Show("Error resetting camera configuration: " + ex.ToString(),"ERROR");
            }
        }

        /// <summary>
        /// updates the camera configuration file on disk with the current
        /// contents of the CameraObject array in memory
        /// </summary>
        /// <returns>bool, true=successful operation</returns>
        public bool SaveCameras()
        {
            return DoSaveCameras(-1,null);
        }

        /// <summary>
        /// reads the camera settings from the Camera_Settings.xml file in the
        /// configuration directory into the internal CameraObject array
        /// 
        /// If the configuration directory does not exist, it is created.  A
        /// MessageBox is displayed to inform the user the directory has been
        /// created, and that the configuration file does not exist.
        /// 
        /// If the configuration file does not exist, a MessageBox is displayed
        /// to inform the user.
        /// </summary>
        /// <returns>bool, true=successful operation</returns>
        private bool GetCameras()
        {
            bool retVal = true;
            DataSet ds;
            XmlReader xmlFile = null;
            string fspec = cfgRoot + "Current\\Camera_Settings.xml";

            try
            {
                xmlFile = XmlReader.Create(fspec, xmlReaderSettings);
                ds = new DataSet();
                ds.ReadXml(xmlFile);
                nCameras = ds.Tables[0].Rows.Count;
                Cameras = new CameraObject[nCameras];
                DataTable dt = ds.Tables[0];
                int cnt = 0;
                foreach (DataRow row in dt.Rows)
                    Cameras[cnt++] = new CameraObject(row);
            }
            catch (DirectoryNotFoundException)
            {
                string dir = cfgRoot + "Current";
                Directory.CreateDirectory(dir);
                MessageBox.Show("Directory " + dir + " created\nFile Camera_Settings.xml does not exist","Notice!");
                return false;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File does not exist:\n" + fspec,"ERROR");
                retVal = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not read Current Camera Settings file into memory:\n" + ex.ToString(),
                                "GetCurrentXml Exception");
                retVal=false;
            }

            if (xmlFile != null)
                xmlFile.Close();

            return retVal;
        }
    }
}
//
// EOF: GlobalConfig.cs
