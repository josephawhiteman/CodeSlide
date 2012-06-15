using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;


namespace CodeGenerator.BL.Modeler
{
    [Serializable]
    [XmlInclude(typeof(ModelElementBase))]
    [DebuggerDisplay("Name = {LogicalName}")]
    public class Entity : ModelElementBase,INodeElement   //  
    {
        private Model _model;
        [XmlIgnore]
        [FieldEditorIgnore]
        public Model Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
            }
        }
        private string _schema;
        public string Schema
        {
            get
            {
                return _schema;
            }
            set
            {
                _schema = value;
            }
        }
        private string _EntityID = System.Guid.NewGuid().ToString();
        [XmlAttribute("ID")]
        [FieldEditorReadonly]
        public string EntityID
        {
            get
            {
                return _EntityID;
            }
            set
            {
                _EntityID = value;
            }
        }
        
        private string _logicalPackage = "Data";
        [FieldEditorAttibute("System.Windows.Forms.TextBox","Package Name")]
        public string LogicalPackage
        {
            get
            {
                return _logicalPackage;
            }
            set
            {
                _logicalPackage = value;
            }
        }
        private string _logicalModule = "BL";
        public string LogicalModule
        {
            get
            {
                return _logicalModule;
            }
            set
            {
                _logicalModule = value;
            }
        }

        private string _logicalName;
        public string LogicalName
        {
            get
            {
                return _logicalName;
            }
            set
            {
                _logicalName = value;
            }
        }
        private string _dBName;
        public string DBName
        {
            get
            {
                return _dBName;
            }
            set
            {
                _dBName = value;
            }
        }

        private RelationReferenceCollection _ParentRelations;
        [FieldEditorIgnore]
        public RelationReferenceCollection ParentRelations
        {
            get
            {
                if (_ParentRelations == null)
                {
                    _ParentRelations = new RelationReferenceCollection();
                }
                return _ParentRelations;
            }
            set
            {
                _ParentRelations = value;
            }
        }
        private RelationReferenceCollection _ChildRelations;
        [FieldEditorIgnore]
        public RelationReferenceCollection ChildRelations
        {
            get
            {
                if (_ChildRelations == null)
                {
                    _ChildRelations = new RelationReferenceCollection();
                }
                return _ChildRelations;
            }
            set
            {
                _ChildRelations = value;
            }
        }
        private FieldCollection _fields;
        [FieldEditorIgnore]
        public FieldCollection Fields
        {
            get
            {
                if (_fields == null)
                {
                    _fields = new FieldCollection();
                }
                return _fields;
            }
            set
            {
                _fields = value;
            }
        }
        private ConstraintCollection _Constraints;
        [FieldEditorIgnore]
        public ConstraintCollection Constraints
        {
            get
            {
                if (_Constraints == null)
                {
                    _Constraints = new ConstraintCollection();
                }
                return _Constraints;
            }
            set
            {
                _Constraints = value;
            }
        }

        #region NodeElement Members

        public string getNodeName()
        {
            return LogicalName;
        }

        #endregion

        #region INodeElement Members


        public ModelTreeNode GetTreeView(TreeConfig treeConfig)
        {
            ModelTreeNode tn = new ModelTreeNode(getNodeName());
            tn.NodeObject = this;

            // in Flat mode all entities are added to the root entity node
            // whereas in Heirarchical only the top entities are added to the top 
            // the child relations then recurse down the tree adding the node as necessary
            if (treeConfig.ViewMode == TreeConfig.TreeViewMode.Flat)
            {
                ModelTreeNode tn0 = new ModelTreeNode("Parents");
                tn.Nodes.Add(tn0);
                foreach (RelationReference rel in ParentRelations)
                {
                    tn0.Nodes.Add(rel.Relation.GetTreeView(treeConfig));
                }
            }
            ModelTreeNode tn1 = new ModelTreeNode("Children");
            tn.Nodes.Add(tn1);
            // here in heirarchical mode
            // the Entity object must be searched for
            // and its GetTreeView called
            foreach (RelationReference rel in ChildRelations)
            {
                if (treeConfig.ViewMode == TreeConfig.TreeViewMode.Heirarchical && rel.Relation.Type.Usage == TypeUsage.Denormalization) continue;
                ModelTreeNode rNode = rel.Relation.GetTreeView(treeConfig);
                tn1.Nodes.Add(rNode);
            }

            ModelTreeNode tn2 = new ModelTreeNode("Fields");
            foreach (Field fld in Fields)
            {
                tn2.Nodes.Add(fld.GetTreeView(treeConfig));
            }
            tn.Nodes.Add(tn2);
            return tn;
        }

        public void PerformContextAction(NodeElementContextAction actn, ModelTreeNode tn)
        {
            if (actn == NodeElementContextAction.Delete)
            {
                foreach (RelationReference r in ChildRelations)
                {
                    // remove all relations as this entity is going away
                    ModelTreeNode top = (ModelTreeNode)tn.TreeView.TopNode.Parent;
                    Model modl = (Model)top.NodeObject;
                    modl.Relations.Remove(r.Relation);

                }
                ChildRelations = null;
                foreach (RelationReference p in ParentRelations)
                {
                    // remove all relations as this entity is going away
                    ModelTreeNode top = (ModelTreeNode)tn.TreeView.TopNode.Parent;
                    Model modl = (Model)top.NodeObject;
                    modl.Relations.Remove(p.Relation);
                }
                ParentRelations = null;
                ((ModelTreeNode)tn.Parent).NodeObject.PerformContextAction(actn, tn);
            }
            else if (actn == NodeElementContextAction.Add)
            {

            }
        }


        #endregion
    }
}
