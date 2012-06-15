using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CodeGenerator.BL.Modeler;
using System.Reflection;


namespace CodeGenerator.Controls
{
    public partial class RelationEditor : UserControl  
    {
        public RelationEditor()
        {
            InitializeComponent();
        }
        public RelationEditor(CodeGenerator.BL.Modeler.Relation relation)
        {
            InitializeComponent();
            this.Relation = relation;
            LoadForm();
        }
        private Relation _Relation;
        public Relation Relation
        {
            get { return _Relation; }
            set { _Relation = value; }
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: Add custom paint code here

            // Calling the base class OnPaint
            base.OnPaint(pe);
        }

        private void LoadForm()
        {
            PropertyInfo[] props = Relation.GetType().GetProperties();
            int top = 45;
            int left = 3;
            int id = 1;
            foreach (PropertyInfo pi in props)
            {
                System.Reflection.PropertyAttributes attrs = pi.Attributes;
                string name = pi.Name;
                Label lbl = new Label();
                lbl.Location = new System.Drawing.Point(3, top);
                lbl.Size = new System.Drawing.Size(70, 20);
                lbl.Name = "lbl_" + name + "_" + id;
                lbl.Text = name; 

                TextBox tb = new TextBox();
                tb.Location = new System.Drawing.Point(80, top);
                tb.Name = "txt_" + name + "_" + id;
                tb.Size = new System.Drawing.Size(200, 20);
                tb.TabIndex = 0;
                tb.Text = pi.GetValue(Relation, null).ToString();

                this.Controls.Add(lbl);
                this.Controls.Add(tb);
                top += 25;
                id += 1;
            }



            //tb.Text = Relation.LogicalName;
            //foreach (string enu in Enum.GetNames(typeof(CodeGenerator.BL.Modeler.RelationType.TypeUsage)))
            //{
            //    cboRelationType.Items.Add(enu);
            //}
            //cboRelationType.SelectedIndex = cboRelationType.FindString(Relation.Type.Usage.ToString());
        }



        private void cboRelationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            Relation.Type.Usage = (TypeUsage)Enum.Parse(typeof(TypeUsage), cb.SelectedItem.ToString(), false);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
