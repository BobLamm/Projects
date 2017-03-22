using System;
using System.Windows.Forms;

namespace Video_Test_Fixture
{
    public partial class DlgEditCamConfig : Form
    {
        private bool bEdit;
        private CameraObject theCam;

        public DlgEditCamConfig(CameraObject cam)
        {
            InitializeComponent();
            Text = (cam == null ? "New" : "Edit") + " Camera";
            if ((bEdit=((theCam=cam) != null)))
            {
                txtName.Text = cam.cameraName;
                txtIpAddr.Text = cam.ipAddress;
                txtPort.Text = cam.port.ToString();
                txtUserName.Text = cam.userName;
                txtPassword.Text = cam.password;
                txtScanWidth.Text = cam.scanWidth.ToString();
                txtScanLines.Text = cam.scanLines.ToString();
            }
            else
            {
                txtPort.Text = "80";
                txtScanWidth.Text = "0";
                txtScanLines.Text = "0";
            }
        }

        private void btnCancel_Click(object sender,EventArgs e)
        {   // ?? do we need to do anything here? I don't think so...
        }

        private void btnOK_Click(object sender,EventArgs e)
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            // TOOD: entry validation !!

            if (theCam == null)
                theCam = new CameraObject();

            theCam.cameraName = txtName.Text;
            theCam.SetIpPort(txtIpAddr.Text,txtPort.Text);
            theCam.userName = txtUserName.Text;
            theCam.password = txtPassword.Text;
            theCam.scanLines = int.Parse(txtScanLines.Text);
            theCam.scanWidth = int.Parse(txtScanWidth.Text);

            if (bEdit) cfg.SaveCameras();
            else cfg.AddCamera(theCam);
        }

        private void chkShowPassword_CheckedChanged(object sender,EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }
    }
}
