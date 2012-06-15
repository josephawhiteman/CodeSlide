using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator.BL.Modeler
{
    public class ModelTreeNode : TreeNode
    {
        public ModelTreeNode(string nodeName) : base(nodeName)
        {

        }
        private INodeElement _nodeObject;
        public INodeElement NodeObject
        {
            get
            {
                return _nodeObject;
            }
            set
            {
                _nodeObject = value;
            }

        }
    }
}
