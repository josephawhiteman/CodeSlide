using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CodeGenerator.BL.Modeler;
using System.Reflection;

namespace CodeGenerator.Controls
{
    public partial class NodeEditor : NodeEditorBase
    {


        public NodeEditor()
        {
            InitializeComponent();

        }
        public NodeEditor(Object nodeObj)
        {
            InitializeComponent();
            NodeObject = nodeObj;
            buildEditor(0);
        }

        public override void buildEditor(int level)
        {
            bool rdonly = false;
            this.Controls.Clear();
            if (NodeObject.GetType().GetCustomAttributes(typeof(FieldEditorIgnore), false).Length == 1) return;

            PropertyInfo[] props = NodeObject.GetType().GetProperties();
            int top = 5;
            int left = 3;
            int fldCol = 105;
            int lblWth = 100;
            int fldWth = 300 - level * (lblWth + 7);
            int fldHtn = 20;
            int id = 1;
            Control ctl = null;
            foreach (PropertyInfo pi in props)
            {
                fldHtn = 20;
                string name = pi.Name;
                FieldEditorAttibute atr;
                string ctlName;
                object ob;
                if (pi.GetCustomAttributes(typeof(FieldEditorIgnore), false).Length > 0) continue;
                object[] attrs = pi.GetCustomAttributes(typeof(FieldEditorAttibute), false);
                if (attrs.Length > 0)
                {

                    atr = (FieldEditorAttibute)attrs[0];
                    ctlName = atr.ControlName;
                    //name = atr.LabelKey;
                    ob = this.GetType().Assembly.CreateInstance(ctlName);
                    if(ob == null)
                    {
                     ob = typeof(TextBox).Assembly.CreateInstance(ctlName);
                    }
                    ctl = (Control)ob;
                    if (ctl is NodeEditorBase)
                    {
                        if (ctl is NodeEditor)
                        {
                         //here the node editor is recursive
                        // and should build another editor
                        //Initialize the Component the properties value with the  
                            object o = pi.GetValue(NodeObject, null);
                            ((NodeEditorBase)ctl).NodeObject = o;
                            ((NodeEditorBase)ctl).buildEditor(level+1);
                        }
                        else
                        {
                            ((NodeEditorBase)ctl).NodeObject = NodeObject;
                        }
                    }
                    else
                    {
                        // load the control where the value is obtained from the Node Object
                        atr.loadControl(ctl, pi, NodeObject);
                        ctl.Name = "txt_" + name + "_" + id;
                    }
                         fldHtn = ctl.Height;
                        ctl.Size = new System.Drawing.Size(fldWth, fldHtn);
                        ctl.Location = new System.Drawing.Point(fldCol, top);
                        ctl.TabIndex = 0;
                   this.Controls.Add(ctl);
                }
                else
                {
                    // ok default editor is a textbox
                    TextBox tb = new TextBox();
                    tb.Location = new System.Drawing.Point(fldCol, top);
                    tb.Name = "txt_" + name + "_" + id;
                    tb.Size = new System.Drawing.Size(fldWth, tb.Height);
                    tb.TabIndex = 0;
                    if (pi.GetCustomAttributes(typeof(FieldEditorReadonly), false).Length > 0)
                    {
                        tb.ReadOnly = true;
                    }
                    if (pi.GetValue(NodeObject, null) == null)
                    {
                        tb.Text = null;
                    }
                    else
                    {
                        tb.Text = pi.GetValue(NodeObject, null).ToString();
                    }
                    this.Controls.Add(tb);
                    ctl = tb;
               }

                Label lbl = new Label();
                lbl.Location = new System.Drawing.Point(3, top);
                lbl.Size = new System.Drawing.Size(lblWth, fldHtn);
                lbl.Name = "lbl_" + name + "_" + id;
                lbl.Text = name;



                this.Controls.Add(lbl);
                top += ctl.Height + 2;//25;
                id += 1;
            }
            if (level == 0)
            {
                Button btn = new Button();
                btn.Name = "btnSave";
                btn.Click += new EventHandler(btnSave_Click);
                btn.Text = "Done";
                btn.Top = top;

                this.Controls.Add(btn);
            }
                top += 25;
            
            
            this.Height = top +10;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is NodeEditor)
                {
                    ((NodeEditor)ctrl).SaveData();
                }
            }
        }
        public void SaveData()
        {
            int id = 1;
            FieldEditorAttibute atr;

            PropertyInfo[] props = NodeObject.GetType().GetProperties();
            foreach (PropertyInfo pi in props)
            {

                if (pi.GetCustomAttributes(typeof(FieldEditorIgnore), false).Length > 0) continue;

                string name = pi.Name;
                string tbName = "txt_" + name + "_" + id;
                Control[] ctls = this.Controls.Find(tbName, false);
                if (ctls.Length == 0) continue;
                Control ctl = ctls[0];

                object[] attrs = pi.GetCustomAttributes(typeof(FieldEditorAttibute), false);
                if (attrs.Length > 0)
                {
                    atr = (FieldEditorAttibute)attrs[0];
                    atr.StoreValue(ctl, pi, NodeObject);
                }
                else
                {

                    if (ctl != null)
                    {
                        if (pi.CanWrite)
                        {
                            if (pi.PropertyType.ToString().Contains("String"))
                            {
                                pi.SetValue(NodeObject, ctl.Text, null);
                            }
                        }
                    }
                }
                id += 1;
            }
        }
    }
}
