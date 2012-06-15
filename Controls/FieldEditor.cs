using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CodeGenerator.BL.Modeler;

namespace CodeGenerator.Controls
{
    public partial class FieldEditor : NodeEditorBase
    {
        public FieldEditor()
        {
            InitializeComponent();
        }
        public FieldEditor(CodeGenerator.BL.Modeler.Field field)
        {
            InitializeComponent();
            this.Field = field;
            LoadForm();

        }
        private Field _Field;
        public Field Field
        {
            get { return _Field; }
            set { _Field = value; }
        }
        private void LoadForm()
        {
            textBox1.Text = Field.LogicalName;
        }
    }
}
