using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CodeGenerator.BL.Modeler;

namespace CodeGenerator.Forms
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private Configuration _Settings;
        public Configuration Settings
        {
            get
            {
                return _Settings;
            }
            set
            {
                _Settings = value;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnRootDir_Click(object sender, EventArgs e)
        {
            dlgRootDir.ShowDialog();
        }
        public IWin32Window prnt;
        private void frmSettings_Load(object sender, EventArgs e)
        {

        }
        public IWin32Window PWin
        {
            get
            {
                return prnt;
            }
            set
            {
                prnt = value;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void dlgSettings1_Load(object sender, EventArgs e)
        {

        }
    }
}