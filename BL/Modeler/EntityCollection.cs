using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CodeGenerator.BL.Modeler
{
    [Serializable]
    [FieldEditorIgnore]
    public class EntityCollection : List<CodeGenerator.BL.Modeler.Entity>, INodeElement
    {
       public void Add(CodeGenerator.BL.Modeler.Entity ent)
       {
           base.Add(ent);
       }

       #region INodeElement Members

       string INodeElement.getNodeName()
       {
           throw new Exception("The method or operation is not implemented.");
       }

       ModelTreeNode INodeElement.GetTreeView(TreeConfig treeConfig)
       {
           throw new Exception("The method or operation is not implemented.");
       }

        public void PerformContextAction(NodeElementContextAction actn, ModelTreeNode tn)
        {
            if (actn == NodeElementContextAction.Delete)
            {
                Entity del = (Entity)tn.NodeObject;
                Remove(del); // remove item from memory
                tn.Parent.Nodes.Remove(tn);
            }
            else if (actn == NodeElementContextAction.Add)
            {
                Entity ne = new Entity();
                ne.LogicalName = "New Entity";
                ModelTreeNode mtn = new ModelTreeNode(ne.LogicalName);
                mtn.NodeObject = ne;
                tn.Parent.Nodes.Add(mtn);

            }
        }

       #endregion
   }
}
