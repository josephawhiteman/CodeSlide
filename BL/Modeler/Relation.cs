using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CodeGenerator.BL.Modeler
{
    [Serializable]
    public class Relation : INodeElement
    {

        private string _id = System.Guid.NewGuid().ToString();
        [XmlAttribute]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private EntityReference _ChildEntity;
        [FieldEditorIgnore]
        public EntityReference ChildEntity
        {
            get
            {
                return _ChildEntity;
            }
            set
            {
                _ChildEntity = value;
            }
        }

        private EntityReference _ParentEntity;
        [FieldEditorIgnore]
        public EntityReference ParentEntity // foreign entity
        {
            get
            {
                return _ParentEntity;
            }
            set
            {
                _ParentEntity = value;
            }
        }      

        private string _relationLogicalName;
        public string LogicalName
        {
            get
            {
                return _relationLogicalName;
            }
            set
            {
                _relationLogicalName = value;
            }
        }

        private string _AssociativeTableName;
        public string AssociativeTableName
        {
            get { return _AssociativeTableName; }
            set { _AssociativeTableName = value; }
        }

        [XmlIgnore]
        public string ParentEntityName
        {
            get
            {
                return ParentEntity.Entity.LogicalName;
            }

        }

        private FieldReferenceCollection _RelatedFields;
        [CollectionFieldEditorAttribute("CodeGenerator.Controls.FieldListBox", "This table", "ParentEntity.Entity.Fields", "LogicalName")]
        public FieldReferenceCollection RelatedFields
        {
            get
            {
                if (_RelatedFields == null)
                {
                    _RelatedFields = new FieldReferenceCollection();
                }
                return _RelatedFields;
            }
            set
            {
                _RelatedFields = value;
            }
        }

        private RelationType _type;
        [ComplexFieldEditorAttribute("CodeGenerator.Controls.NodeEditor", "Relation Type", "Type")]
        public RelationType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }



        [XmlIgnore]
        public string ChildEntityName
        {
            get
            {
                return ChildEntity.Entity.LogicalName;
            }

        }
        private FieldReferenceCollection _ForeignFields;
        [CollectionFieldEditorAttribute("CodeGenerator.Controls.FieldListBox", "Other table", "ChildEntity.Entity.Fields", "LogicalName")]
        public FieldReferenceCollection ForeignFields
        {
            get
            {
                if (_ForeignFields == null)
                {
                    _ForeignFields = new FieldReferenceCollection();
                }
                return _ForeignFields;
            }
            set
            {
                _ForeignFields = value;
            }
        }

        #region INodeElement Members

        public string getNodeName()
        {
            return LogicalName;
        }

        /// <summary>
        /// Danger here as a recursive multi table loop will cause endless loop
        /// </summary>
        /// <param name="treeConfig"></param>
        /// <returns></returns>
 
        public ModelTreeNode GetTreeView(TreeConfig treeConfig)
        {
            ModelTreeNode tn = new ModelTreeNode(getNodeName());

            if (treeConfig.ViewMode == TreeConfig.TreeViewMode.Heirarchical && ParentEntity.Entity.EntityID != ChildEntity.Entity.EntityID)
            {
                Entity chld = ChildEntity.Entity;
                tn.Nodes.Add(chld.GetTreeView(treeConfig));
                // now cascade down the entity tree
            }
            else if (treeConfig.ViewMode == TreeConfig.TreeViewMode.Heirarchical && ParentEntity.Entity.EntityID != ChildEntity.Entity.EntityID)
            {
                ModelTreeNode self = new ModelTreeNode(ParentEntity.Entity.LogicalName);
            }
            tn.NodeObject = this;
            return tn;
        }

        public void PerformContextAction(NodeElementContextAction actn, ModelTreeNode tn)
        {
            //throw new Exception("The method or operation is not implemented.");
            if (actn == NodeElementContextAction.Delete)
            {

            }
        }

        #endregion
    }
}
