/**
 * File: DlgButtonSettings.cs
 * 
 *	Copyright © 2016 by City Council Video.  All rights reserved.
 *
 *	$Id: /DlgButtonSettings.cs,v $
 */
/**
*	Provides a way to check button data
*
*	Author:			Fred Koschara and Bob Lamm
*	Creation Date:	December tenth, 2016
*	Last Modified:	December 27, 2016 @ 11:17 am
*
*	Revision History:
*	   Date		  by		Description
*	2016/12/13	blamm	original development
*		|						|
*	2016/12/10	wfredk	modifications
*	
*   TO DO:  Buttons on main window should be refreshed after updating the file.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;  // For Databasing
using System.Data.OleDb;  // For Databasing


namespace VGV101
{
    public partial class DlgButtonSettings : Form
    {
        public DlgButtonSettings()
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            InitializeComponent();

            // BUTTONS PAGE

            // Read Button Settings from File
            if (!cfg.GetCurrentXml("Buttons", buttonsData)) // we can't proceed from here
            // DON'T DO THIS:  SEE THE NOTE IN GlobalConfig.cs
            //if (!cfg.ReadXMLFile("Buttons.xml", buttonsData)) // we can't proceed from here
                {
                MessageBox.Show("Button data not available", "ERROR");
                this.Close();
                return;
            }

            // Set DataGridView read/write parameters and background colors

            buttonsData.RowTemplate.MinimumHeight = 30;
            // buttonsData.BackgroundColor = Color.Red;  // This is set by DefaultCellStyle in the Properties panel
            buttonsData.Columns[0].Frozen = true;
            buttonsData.Columns[0].ReadOnly = true;
            buttonsData.Columns[0].DefaultCellStyle.BackColor = SystemColors.Control;
            buttonsData.AllowUserToResizeColumns = true;            
        }


        // UPDATE BUTTONS SETTINGS
        private void button18_Click(object sender, EventArgs e)  // Update Buttons
        {
            // Update Startup Settings file from info in buttonsData

            GlobalConfig cfg = GlobalConfig.Instance;
            cfg.WriteCurrentXml("Buttons", buttonsData);
            // DON'T DO THIS:  SEE THE NOTE IN GlobalConfig.cs
            //if (cfg.WriteXMLFile(buttonsData, "Buttons.xml"))
            MessageBox.Show("Updated Buttons Settings");
        }
    }
}
//
// EOF: GlobalConfig.cs
