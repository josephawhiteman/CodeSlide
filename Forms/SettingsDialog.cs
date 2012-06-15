using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator.Forms
{
    public partial class SettingsDialog : UserControl
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }
        public SettingsDialog(CodeGenerator.BL.Support.Settings info)
        {
            InitializeComponent();
            _Settings = info;
        }
        private CodeGenerator.BL.Support.Settings _Settings;

    }
}
