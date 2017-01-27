/* *************************************************************************************************
 * ModifyRegistry.cs
 * -------------------------------------------------------------------------------------------------
 * adapted from https://www.codeproject.com/articles/3389/read-write-and-delete-from-registry-with-c
 * originally written by Francesco Natali (fn.varie@libero.it)
 * updated by Andrew Morpeth to suppport RegistryView
 * -------------------------------------------------------------------------------------------------
 * Revision History:
 *    Date        by        Description
 * 2017-01-27   wfredk  clean up from w7/64 Registry debugging
 * 2017-01-27   wfredk  default to RegistryView.Registry64 so programs work as expected on w7/64
 *                      also don't use (and comment out support for) baseRegistryKey
 * 2016-12-09   wfredk  major rewrite to incorporate fixes from comments, add error checking, etc.
 * *************************************************************************************************/

using System;
using Microsoft.Win32;      // required for reading/writing into the Registry
using System.Windows.Forms; // for the MessageBox function

/*
* N.B.:
*
* Registry.CurrentUser stores under HKEY_USERS instead of HKEY_CURRENT_USER for Server application
* 	masteryoda21	30-Nov-11 13:49
*
*    ModifyRegistry reg = new ModifyRegistry(); // Attach to the Registry.
*    reg.BaseRegistryKey = Registry.CurrentUser; // HKey: Current User.
*
* followed by:
*
*    reg.Write(KeyName, value);
*
* writes under HKEY_USERS (specifically HKEY_USERS\.DEFAULT) instead of HKEY_CURRENT_USER for a [C#] Service.
*
* The user name for a service is "System", in other words, there is no current user for a service.
*
* Run any of these statements in a service:
*    string hostname = System.Net.Dns.GetHostName();
*    string nexttext = WindowsIdentity.GetCurrent().Name;
*    string username = System.Windows.Forms.SystemInformation.UserName;
*    string eusrname = Environment.UserName;
*    System.Security.Principal.WindowsIdentity user =
*    System.Security.Principal.WindowsIdentity.GetCurrent();
*    string usernme = user.Name;
*
* They will all return variants of "System".
*
* So, for a service writing to the Registry, there is no way to write to HKEY_CURRENT_USER, because there is no current user.
*/

namespace Utility.ModifyRegistry
{
    /**
    * A class to read/write/delete/count Registry keys
    */
    public class ModifyRegistry
	{
		private bool showError = false;
		/// <summary>
		/// A property to show or hide error messages 
		/// (default = false)
		/// </summary>
		public bool ShowError
		{
			get { return showError; }
			set	{ showError = value; }
		}

		private string subKey = @"SOFTWARE\" + Application.ProductName.ToString();
        /// <summary>
        /// A property to set the SubKey value
        /// (default = "SOFTWARE\\" + Application.ProductName.ToString())
        /// </summary>
        public string SubKey
		{
			get { return subKey; }
			set	{ subKey = value; }
		}

        //Andrew Morpeth
        //Added ability to specifiy x86 or x64 Registry targets using RegistryKey.OpenBaseKey Method rather than baseRegistryKey. 
        //Orignally written for the write method to stop re-direction to the Wow6432Node under software

        //Set RegistryHive
        private RegistryHive registryHive = RegistryHive.LocalMachine;
        /// <summary>
        /// A property to set the RegistryHive value
        /// (default = RegistryHive.LocalMachine)
        /// </summary>
        public RegistryHive RegistryHive
        {
            get { return registryHive; }
            set { registryHive = value; }
        }

        //Set RegistryView
        private RegistryView registryView = RegistryView.Registry64;
        /// <summary>
        /// A property to set the registryView value
        /// (default = RegistryView.Default)
        /// 
        /// USE:
        ///     ModifyRegistry addKey = new ModifyRegistry();
        ///     addKey.RegistryHive = RegistryHive.LocalMachine;    // redundant for this value
        /// specifies to use the x64 Registry node for x32 application.
        /// If you request a 64-bit view on a 32-bit operating system, the returned keys will be in the 32-bit view.
        ///     addKey.RegistryView = RegistryView.Registry64;
        /// </summary>
        public RegistryView RegistryView
        {
            get { return registryView; }
            set { registryView = value; }
        }

        /* **************************************************************************
		 * **************************************************************************/

        public int? ReadInt(string KeyName)     // for DWORD values
        {
            return (int)this.Read(KeyName);
        }

        public string ReadString(string KeyName)
        {
            return (string)this.Read(KeyName);
        }

        /// <summary>
        /// To read a Registry key.
        /// input: KeyName (string)
        /// output: value (object) 
        /// </summary>
        public object Read(string KeyName)
		{
			// Opening the Registry key
			RegistryKey rk = RegistryKey.OpenBaseKey(registryHive,registryView);
			// Open subKey as read-only
			RegistryKey sk1 = rk.OpenSubKey(subKey);

            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                rk.Close();
                return null;
            }

            try 
			{
                // If the RegistryKey exists I get its value
                // or null is returned.
                object val = sk1.GetValue(KeyName);
                sk1.Close();
                rk.Close();
                return val;
			}
			catch (Exception e)
			{
				ShowErrorMessage(e, "Reading Registry " + KeyName);
                sk1.Close();
                rk.Close();
                return null;
			}
		}

        public int? ReadInt(string KeyName, object defaultValue)     // for DWORD values
        {
            return (int)this.Read(KeyName,defaultValue);
        }

        public string ReadString(string KeyName, object defaultValue)
        {
            return (string)this.Read(KeyName,defaultValue);
        }

        public object Read(string KeyName, object defaultValue)
        {
            // Opening the Registry key
            RegistryKey rk = RegistryKey.OpenBaseKey(registryHive,registryView);
            // Open subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(subKey);

            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                rk.Close();
                return defaultValue;
            }

            try
            {
                // If the RegistryKey exists I get its value
                // or null is returned.
                object w1 = sk1.GetValue(KeyName);
                sk1.Close();
                rk.Close();
                return (w1==null ? defaultValue : w1);
            }
            catch (Exception e)
            {
                ShowErrorMessage(e, "Reading Registry " + KeyName);
                sk1.Close();
                rk.Close();
                return defaultValue;
            }
        }

        /* **************************************************************************
		 * **************************************************************************/

        /// <summary>
        /// To write into a Registry key.
        /// input: KeyName (string) , Value (object)
        /// output: true or false 
        /// </summary>
        public bool Write(string KeyName, object Value)
		{
            RegistryKey rk=null;     // Registry key
            RegistryKey sk1=null;    // subkey

            try
			{
                //Updated by Andrew M to suppport RegistryView
                //rk = baseRegistryKey;
                rk = RegistryKey.OpenBaseKey(registryHive,registryView);

                // use CreateSubKey: create or open it if already exits 
                sk1 = rk.CreateSubKey(subKey);

				// Save the value
				sk1.SetValue(KeyName, Value);
                sk1.Close();
                rk.Close();
                return true;
			}
			catch (Exception e)
			{
				ShowErrorMessage(e, "Writing Registry " + KeyName);
                if (sk1 != null) sk1.Close();
                if (rk != null) rk.Close();
                return false;
			}
		}

        /// <summary>
        /// To write into a Registry key.
        /// input: KeyName (string) , Value (bool)
        /// output: true or false 
        /// </summary>
        public bool WriteBool(string KeyName, bool Value)
        {
            return WriteInt(KeyName, (Value ? 1 : 0));
        }

        public bool WriteInt(string KeyName, int? Value)
        {
            RegistryKey rk = null;     // Registry key
            RegistryKey sk1 = null;    // subkey

            try
            {
                //Updated by Andrew M to suppport RegistryView
                //rk = baseRegistryKey;
                rk = RegistryKey.OpenBaseKey(registryHive,registryView);

                // use CreateSubKey: create or open it if already exits 
                sk1 = rk.CreateSubKey(subKey);

                // Save the value
                sk1.SetValue(KeyName, Value, RegistryValueKind.DWord);
                sk1.Close();
                rk.Close();
                return true;
            }
            catch (Exception e)
            {
                ShowErrorMessage(e, "Writing Registry " + KeyName);
                if (sk1 != null) sk1.Close();
                if (rk != null) rk.Close();
                return false;
            }
        }

        /* **************************************************************************
		 * **************************************************************************/

        /// <summary>
        /// To delete a Registry key.
        /// input: KeyName (string)
        /// output: true or false 
        /// </summary>
        public bool DeleteValue(string KeyName)
		{
            RegistryKey rk = RegistryKey.OpenBaseKey(registryHive,registryView);
            RegistryKey sk1 = rk.OpenSubKey(subKey, true);

            // If the RegistrySubKey doesn't exists -> (true)
            if (sk1 == null)
            {
                rk.Close();
                return true;
            }

            bool retVal = true;
            try
            {
				sk1.DeleteValue(KeyName);
			}
			catch (Exception e)
			{
				ShowErrorMessage(e, "Deleting value " + KeyName);
                retVal = false;
			}

            sk1.Close();
            rk.Close();
            return retVal;
        }

        /* **************************************************************************
		 * **************************************************************************/

        /// <summary>
        /// To delete a sub key and any child.
        /// input: void
        /// output: true or false 
        /// </summary>
        public bool DeleteSubKeyTree()
		{
            bool bDontDoThis = subKey.Equals(@"SOFTWARE\" + Application.ProductName.ToString());
            if (bDontDoThis)
            {
                // you almost certainly don't want to delete all of the settings for the program.
                MessageBox.Show( "If you really want to delete\n"
                                +"the program's Registry key,\n"
                                +"you need to modify the code.",
                                "Seriously??",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            RegistryKey rk = RegistryKey.OpenBaseKey(registryHive,registryView);
            RegistryKey sk1 = rk.OpenSubKey(subKey);

            if (sk1==null)
            {
                rk.Close();
                return true;
            }

            bool retVal = true;
            try
            {
				rk.DeleteSubKeyTree(subKey);
			}
			catch (Exception e)
			{
				ShowErrorMessage(e, "Deleting SubKey " + subKey);
                retVal = false;
			}

            sk1.Close();
            rk.Close();
            return retVal;
        }

        /* **************************************************************************
		 * **************************************************************************/

        /// <summary>
        /// Retrive the count of subkeys at the current key.
        /// input: void
        /// output: number of subkeys
        /// </summary>
        public int SubKeyCount()
		{
            RegistryKey rk = RegistryKey.OpenBaseKey(registryHive,registryView);
            RegistryKey sk1 = rk.OpenSubKey(subKey);

            try
            {
                // count if the RegistryKey exists, else zero
                int cnt = (sk1 != null ? sk1.SubKeyCount : 0);
                rk.Close();
                return cnt;
			}
			catch (Exception e)
			{
				ShowErrorMessage(e, "Retriving subkeys of " + subKey);
                if (sk1 != null) sk1.Close();
                rk.Close();
                return 0;
			}
		}

		/* **************************************************************************
		 * **************************************************************************/

		/// <summary>
		/// Retrive the count of values in the key.
		/// input: void
		/// output: number of keys
		/// </summary>
		public int ValueCount()
		{
            RegistryKey rk = RegistryKey.OpenBaseKey(registryHive,registryView);
            RegistryKey sk1 = rk.OpenSubKey(subKey);

            try
            {
                // count if the RegistryKey exists, else zero
                int cnt = (sk1 != null ? sk1.ValueCount : 0);
                rk.Close();
                return cnt;
            }
            catch (Exception e)
			{
				ShowErrorMessage(e, "Retriving keys of " + subKey);
                if (sk1 != null) sk1.Close();
                rk.Close();
                return 0;
			}
		}

		/* **************************************************************************
		 * **************************************************************************/
		
		private void ShowErrorMessage(Exception e, string Title)
		{
			if (showError == true)
				MessageBox.Show(e.Message,
								Title,
								MessageBoxButtons.OK,
								MessageBoxIcon.Error);
		}
	}
}
//
// EOF: ModifyRegistry.cs
