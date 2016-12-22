using System;
// using System.Collections.Generic;
// using System.ComponentModel;
using System.Data;
// using System.Drawing;
// using System.Linq;
// using System.Text;
using System.Windows.Forms;

namespace VGV101
{
    public partial class DlgListEditor : Form
    {
        int buttonNumber;

        public DlgListEditor(int listWanted, int isConnectedToButton)  
            
        // 'listWanted' tells it which list is being called, 
        // 'isConnectedToButton' tells it which button it is connected to. (-1 means it was called from the main menu)
        // buttonNumber is a variable all the functions can see
        // displayedListData is for the displayed list information.  
        // activeRowData is for keeping track of the active rows in each list
        // buttonsData is for loading button info if it needs updating

        {
            InitializeComponent();
            comboBox1.SelectedIndex = listWanted;
            buttonNumber = isConnectedToButton;

            // Read wanted list info from file into displayedListData
            // Read current active list entries file into activeRowData to see which row in list is active
            if (!GetListData(listWanted))
                return;

            int currentListRow = 0;  // Default active row is the first one...
            // This reads the active row for this list from activeRowData - Int32.TryParse(TextBoxD1.Text, out x)
            Int32.TryParse(activeRowData.Rows[listWanted].Cells[1].Value.ToString(), out currentListRow);
            // This makes the row active by selecting the first cell in it.
            displayedListData.CurrentCell = displayedListData.Rows[currentListRow].Cells[0];
        }

        private string GetCategoryName(int listWanted)
        {   switch (listWanted)
            {   case 1: return "Agenda_Items";
                case 2: return "Notices";
                case 3: return "Other_Items";
            }

            return "People";    // default, case 0
        }

        private bool GetListData(int listWanted)
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            string category = GetCategoryName(listWanted);

            // Read appropriate list file into displayedListData
            if (!cfg.GetCurrentXml(category, displayedListData)) // we can't proceed from here
            {
                MessageBox.Show(category.Replace("_"," ") + " data not available","ERROR");
                return false;
            }

            // Read current active list entries file into activeRowData to see which row in list is active
            if (!cfg.GetCurrentXml("Current_Active_List_Entries", activeRowData)) // we can't proceed from here
            {
                MessageBox.Show("Current Active List Entries not available","ERROR");
                return false;
            }

            return true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)  // If the selected category changes...
        {
            int listWanted = comboBox1.SelectedIndex;  // Which list are we looking at?

            // Read wanted list info from file into displayedListData
            // Read current active list entries file into activeRowData to see which row in list is active
            if (!GetListData(listWanted))
                return;

            int currentListRow = 0;   // Default active row is the first one...
            // This reads the active row for this list from activeRowData - Int32.TryParse(TextBoxD1.Text, out x)
            Int32.TryParse(activeRowData.Rows[listWanted].Cells[1].Value.ToString(), out currentListRow);
            // This makes the row active by selecting the first cell in it
            displayedListData.CurrentCell = displayedListData.Rows[currentListRow].Cells[0];
        }

        private void button4_Click(object sender, EventArgs e)  // Click on 'Add New Row to List' button pressed...
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            string category = GetCategoryName(comboBox1.SelectedIndex);

            displayedListData.ClearSelection();  // Add new row
            displayedListData.Rows[displayedListData.Rows.Count - 1].Selected = true;

            // Update List file from info in displayedListData
            cfg.WriteCurrentXml(category, displayedListData);
        }

        private void button3_Click(object sender, EventArgs e)  // Remove Selected Rows button pressed...
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            string category = GetCategoryName(comboBox1.SelectedIndex);

            foreach (DataGridViewRow item in this.displayedListData.SelectedRows)  // Delete all selected rows
            {
                displayedListData.Rows.RemoveAt(item.Index);  // Delete Row(s)
            }

            // Update List file from info in displayedListData
            cfg.WriteCurrentXml(category, displayedListData);
        }

        private void button2_Click(object sender, EventArgs e)  // 'Select Text/Update Text' button pressed...
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            string category = GetCategoryName(comboBox1.SelectedIndex);

            // Update text in list file and also make it the active entry

            // Update List file from info in displayedListData
            cfg.WriteCurrentXml(category, displayedListData);

            // Update Current Active Entry activeRowData and File
            activeRowData.Rows[comboBox1.SelectedIndex].Cells[1].Value = displayedListData.CurrentRow.Index; // displayedListData.CurrentRow.Index;

            // Update Current-Active-List-Entry file from info in activeRowData
            cfg.WriteCurrentXml("Current_Active_List_Entries", activeRowData);

            // Update button ('buttonNumber') in buttonsData. If the button number is -1 means this menu has been called from the main menu.
            if (buttonNumber == -1)
            {   MessageBox.Show("Button was called for menu");
                return;
            }

            if (!cfg.GetCurrentXml("Buttons", buttonsData)) // we can't proceed from here
            {   MessageBox.Show("Button data not available","ERROR");
                return;
            }

            buttonsData.Rows[buttonNumber].Cells[buttonsData.Columns["Text_Line_1_Source"].Index].Value = category.Replace("_", " ");

            // Update Buttons.xml file from info in buttonsData
            cfg.WriteCurrentXml("Buttons", buttonsData);
        }


        // Stuff for later fleshing out...

        private void button7_Click(object sender, EventArgs e)  // 'Current Date' button pressed...
        {
            MessageBox.Show("Inserts current date into text.  This keeps up with computer date.");
        }

        private void button8_Click(object sender, EventArgs e)  // 'Current Time' button pressed...
        {
            MessageBox.Show("Inserts current time into text.  This keeps up with computer time.");
        }

        private void button9_Click(object sender, EventArgs e)  // 'Excel Value' button pressed...
        {
            MessageBox.Show("Gets a File/Worksheet/Cell value.  Additional UI to choose file and cel and whether to watch the file for changes.");
        }

        private void button10_Click(object sender, EventArgs e)  // 'XML Value' button pressed...
        {
            MessageBox.Show("Gets a simple XML field from a file.  Additional UI to choose file and field and whether to watch the file for changes..");
        }

        private void button11_Click(object sender, EventArgs e)  // 'Twitter' button pressed...
        {
            MessageBox.Show("Gets a field from a Twitter feed.  Additional UI to configure and manage.");
        }

        private void button12_Click(object sender, EventArgs e)  // 'RSS Feed' button pressed...
        {
            MessageBox.Show("Gets a field from an RSS feed.  Additional UI to configure and manage.");
        }
    }
}
