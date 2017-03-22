using System;
using System.Windows.Forms;

namespace Video_Test_Fixture
{
    public partial class DlgCamConfig : Form
    {
        public DlgCamConfig()
        {
            InitializeComponent();
            LoadCameras();
        }

        private void btnDelCamera_Click(object sender,EventArgs e)
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            ListView.SelectedIndexCollection indexes = this.listCameras.SelectedIndices;
            int cnt = (int) listCameras.Items[indexes[0]].Tag;
            CameraObject cam=cfg.Camera(cnt);

            // get confirmation
            if (MessageBox.Show("\t"+cam.cameraName+"\n"
                                +"\tat "+cam.ipAddrPort+"\n"
                                +"\t("+cam.scanWidth+" x "+cam.scanLines+")\n\n"
                                +"Are you sure?","Delete this camera?",
                                MessageBoxButtons.OKCancel,MessageBoxIcon.Warning)
                ==DialogResult.OK)
            {
                // remove camera from global config & update file
                cfg.DeleteCamera(cnt);
                // remove camera from this dialog
                listCameras.Items.Clear();
                LoadCameras();
            }

            btnDelCamera.Enabled = btnEditCamera.Enabled = false;
        }

        private void btnEditCamera_Click(object sender,EventArgs e)
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            ListView.SelectedIndexCollection indexes = this.listCameras.SelectedIndices;
            int cnt = indexes[0];

            DlgEditCamConfig dlg = new DlgEditCamConfig(cfg.Camera(cnt));
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                listCameras.Items.Clear();
                LoadCameras();
            }

            btnDelCamera.Enabled = btnEditCamera.Enabled = false;
        }

        private void btnNewCamera_Click(object sender,EventArgs e)
        {
            DlgEditCamConfig dlg = new DlgEditCamConfig(null);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                listCameras.Items.Clear();
                LoadCameras();
            }
        }

        private void listCameras_SelectedIndexChanged(object sender,EventArgs e)
        {
            GlobalConfig cfg = GlobalConfig.Instance;
            ListView.SelectedIndexCollection indexes = this.listCameras.SelectedIndices;
            bool bEnabled=indexes.Count==1 && indexes[0]>=0 && indexes[0]<cfg.NumCameras;

            btnDelCamera.Enabled = bEnabled;
            btnEditCamera.Enabled = bEnabled;
        }

        private void LoadCameras()
        {
            GlobalConfig cfg = GlobalConfig.Instance;

            for (int cnt = 0; cnt < cfg.NumCameras; cnt++)
            {
                CameraObject cam = cfg.Camera(cnt);
                var item = new ListViewItem(new[] { (cnt + 1).ToString(),cam.ipAddrPort,cam.cameraName,cam.scanWidth + " x " + cam.scanLines });
                item.Tag = cnt;
                listCameras.Items.Add(item);
            }
        }
    }
}
