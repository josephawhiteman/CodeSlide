using System;
using System.Collections.Generic;
using System.Text;
using CodeGenerator.BL.Generator;
using System.Xml.Serialization;

namespace CodeGenerator.BL.Modeler
{
    [Serializable]
    [XmlInclude(typeof(CSharpFieldType))]
    public class Field : INodeElement
    {
        private string _ID;
        [XmlAttribute("ID")]
        [FieldEditorReadonly]
        public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
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

        private string _dbname;
        public string DBName
        {
            get
            {
                return _dbname;
            }
            set
            {
                _dbname = value;
            }
        }

        private FieldType _logicalType;
        [ComplexFieldEditorAttribute("CodeGenerator.Controls.NodeEditor", "Relation Type", "LogicalType")]
        public FieldType LogicalType
        {
            get
            {
                return _logicalType;
            }
            set
            {
                _logicalType = value;
            }
        }

        private string _DBType;
        public string DBType
        {
            get
            {
                return _DBType;
            }
            set
            {
                _DBType = value;
            }
        }
        private bool _nullable;
        public bool Nullable
        {
            get
            {
                return _nullable;
            }
            set
            {
                _nullable = value;
            }
        }


        #region INodeElement Members

        public string getNodeName()
        {
            return LogicalName;
        }

        public ModelTreeNode GetTreeView(TreeConfig treeConfig)
        {

            ModelTreeNode tn = new ModelTreeNode(getNodeName());
            tn.NodeObject = this;
            return tn;
        }

        public void PerformContextAction(NodeElementContextAction actn, ModelTreeNode tn)
        {
            Field fld = (Field)tn.NodeObject;
            tn.Parent.Nodes.Remove(tn);
        }

        #endregion
    }
}
