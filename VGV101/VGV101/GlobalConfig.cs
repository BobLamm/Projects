/**
 * File: GlobalConfig.cs
 * 
 *	Copyright © 2016 by City Council Video.  All rights reserved.
 *
 *	$Id: /GlobalConfig.cs,v $
 */
/**
*	Provides an interface to program configuration files
*
*	Author:			Fred Koschara
*	Creation Date:	December tenth, 2016
*	Last Modified:	December 13, 2016 @ 11:17 am
*
*	Revision History:
*	   Date		  by		Description
*	2016/12/13	wfredk	original development
*		|						|
*	2016/12/10	wfredk	original development
*/
using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;  // for XML

namespace VGV101
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
        public bool Init(string cfgRoot,string mediaRoot,string logRoot)
        {
            if (bInitialized)
                return false;

            // ensure paths end with a trailing slash
            this.cfgRoot = TerminatePath(cfgRoot);
            this.logRoot = TerminatePath(logRoot);
            this.mediaRoot = TerminatePath(mediaRoot);

            xmlReaderSettings = new XmlReaderSettings();

            return true;
        }

        // --------------------------------------------------------------------

        public string TerminatePath(string path)
        {   if (!path.EndsWith(@"\"))
                return path + @"\";

            return path;
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
            bool retVal = true;
            if (Cameras==null || nCameras==0)
            {
                MessageBox.Show("No cameras to save!", "ERROR");
                return false;
            }
            try
            {
                DataTable dt = CameraObject.NewTable("Record");
                for (int cnt = 0; cnt < nCameras; cnt++)
                {
                    Cameras[cnt].NewRow(dt, cnt);
                    if (Cameras[cnt].ErrorStatus != 0)
                        throw new Exception(Cameras[cnt].ErrorString);
                    dt.WriteXml(cfgRoot + "Current\\Camera_Settings.xml", true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR Saving Cameras");
                retVal = false;
            }
            return retVal;
        }

        private bool GetCameras()
        {
            bool retVal = true;
            DataSet ds;
            XmlReader xmlFile = null;

            try  // Read Button Settings from Buttons.xml file into CameraObject array
            {
                xmlFile = XmlReader.Create(cfgRoot + "Current\\Camera_Settings.xml", xmlReaderSettings);
                ds = new DataSet();
                ds.ReadXml(xmlFile);
                nCameras = ds.Tables[0].Rows.Count;
                Cameras = new CameraObject[nCameras];
                DataTable dt = ds.Tables[0];
                int cnt = 0;
                foreach (DataRow row in dt.Rows)
                    Cameras[cnt++] = new CameraObject(row);
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
