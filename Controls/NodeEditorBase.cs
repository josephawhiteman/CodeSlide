using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator.Controls
{
    public partial class NodeEditorBase : UserControl
    {
        public NodeEditorBase()
        {
            InitializeComponent();
        }


        private Object _NodeObject;
        public virtual Object NodeObject
        {
            get { return _NodeObject; }
            set {
                _NodeObject = value; 
            }
        }

        public virtual void buildEditor(int level)
        {
        }
    }
}
