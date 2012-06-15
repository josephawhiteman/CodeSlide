using System;
using System.Collections.Generic;
using System.Text;
using CodeGenerator.BL.Modeler;

namespace CodeGenerator.Controls
{
    class TreeNodeContextMenuStrip : System.Windows.Forms.ContextMenuStrip
    {
        public TreeNodeContextMenuStrip(System.Windows.Forms.TreeView tree)
        {
            _ModelTree = tree;
        }
        private System.Windows.Forms.TreeView _ModelTree;

        public System.Windows.Forms.TreeView ModelTree
        {
            get { return _ModelTree; }
            set { _ModelTree = value; }
        }
        public void buildToolStripItems()
        {
            TreeNodeToolStripMenuItem itm = new TreeNodeToolStripMenuItem();
            itm.Name = "tsiDeleteNode";
            itm.Text = "Delete";
            itm.Click += new EventHandler(itm_DeleteClick);
            this.Items.Add(itm);

            TreeNodeToolStripMenuItem add = new TreeNodeToolStripMenuItem();
            add.Name = "tsiAddNode";
            add.Text = "Add";
            add.Click += new EventHandler(add_Click);
            this.Items.Add(add);
        }

        void add_Click(object sender, EventArgs e)
        {
            
        }

        private ModelTreeNode _CurrentTreeNode;

        public ModelTreeNode CurrentTreeNode
        {
            get { return _CurrentTreeNode; }
            set { _CurrentTreeNode = value; }
        }

        void itm_DeleteClick(object sender, EventArgs e)
        {
            ModelTreeNode node = CurrentTreeNode;
            if(node.NodeObject != null)
            {
                node.NodeObject.PerformContextAction(NodeElementContextAction.Delete, node);
            }
        }
    }
}
