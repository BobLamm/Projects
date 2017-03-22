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
*	Last Modified:	March 19, 2017 @ 7:36 pm
*
*	Revision History:
*	   Date		  by		Description
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
    /**
     * global configuration object for the program
     */
    public sealed class GlobalConfig
    {
        // singleton implementation
        // based on http://csharpindepth.com/Articles/General/Singleton.aspx
        private GlobalConfig()
        {
        }
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
        public String CfgRoot
        {
            get { return cfgRoot; }
        }
        private String logRoot;
        public String LogRoot
        {
            get { return logRoot; }
        }
        private String mediaRoot;
        public String MediaRoot
        {
            get { return mediaRoot; }
        }

        // initializes the global configuration object, called during program startup
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

            return true;
        }

        // --------------------------------------------------------------------

        // Application-defined message to notify app of filtergraph events
        public const int WM_GRAPHNOTIFY = 0x8000 + 1;

        IGraphBuilder graph = null;
        public IGraphBuilder Graph
        {   get
            {   if (graph == null)
                {   try
                    {   graph = (IGraphBuilder)new FilterGraph();
                    }
                    catch (COMException ex)
                    {   MessageBox.Show("COM Error: " + ex.ToString());
                    }
                    catch (Exception ex)
                    {   MessageBox.Show("Error: " + ex.ToString());
                    }
                }
                return graph;
            }
        }

        // --------------------------------------------------------------------

        // Reads an XML file in Current configuration folder into the DataGridView that is passed
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

        // Writes an XML file in Current configuration folder from the dataGridView that is passed
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
        public int NumCameras
        {   get
            {
                if (Cameras == null)
                    GetCameras();
                return nCameras;
            }
        }

        public bool AddCamera(CameraObject newCam)
        {
            return DoSaveCameras(-1,newCam);
        }

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

        public bool DeleteCamera(int nDelete)
        {
            return DoSaveCameras(nDelete,null);
        }

        public bool DoSaveCameras(int nDel,CameraObject newCam)
        {
            bool retVal = true;
            if ((Cameras == null || nCameras == 0) && newCam==null)
            {
                MessageBox.Show("No cameras to save!","ERROR");
                return false;
            }
            if (nDel >= nCameras)
            {
                MessageBox.Show("Bad delete index " + nDel + " found in DoCameraSave()");
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
                    if (Cameras[cnt].ErrorStatus != 0)
                        throw new Exception(Cameras[cnt].ErrorString);
                }
                if (newCam != null) // adding a new camera
                {
                    newCam.NewRow(dt,cnt);
                    if (newCam.ErrorStatus != 0)
                        throw new Exception(newCam.ErrorString);
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

        // called after saving camera settings in DlgCameraSettings() using DataGridView
        public void ResetCameraConfig()
        {   try
            {   Cameras = null;
                nCameras = 0;
            }
            catch (Exception ex)
            {   MessageBox.Show("Error resetting camera configuration: " + ex.ToString());
            }
        }

        public bool SaveCameras()
        {
            return DoSaveCameras(-1,null);
        }

        private bool GetCameras()
        {
            bool retVal = true;
            DataSet ds;
            XmlReader xmlFile = null;
            string fspec = cfgRoot + "Current\\Camera_Settings.xml";

            try  // Read Camera Settings from Camera_Settings.xml file into CameraObject array
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
                MessageBox.Show("Directory " + dir + " created\nFile Camera_Settings.xml does not exist");
                return false;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File does not exist:\n" + fspec);
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
