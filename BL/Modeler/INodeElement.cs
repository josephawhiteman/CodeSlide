using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator.BL.Modeler
{
    public enum NodeElementContextAction
    {
        Add,
        Delete,
    }
    public interface INodeElement
    {
        string getNodeName();
        ModelTreeNode GetTreeView(TreeConfig treeConfig);
        void PerformContextAction(NodeElementContextAction actn, ModelTreeNode tn);
    }
}
