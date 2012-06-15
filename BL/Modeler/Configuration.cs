using System;
using System.Collections.Generic;
using System.Text;
using CodeGenerator.BL.Generator;


namespace CodeGenerator.BL.Modeler
{
    public class Configuration : INodeElement
    {
        private string _EntityTransform = "EntityDef.xslt";
        public string EntityTransform
        {
            get
            {
                return _EntityTransform;
            }
            set
            {
                _EntityTransform = value;
            }
        }
        private bool _PackageIsSchema = true;
        public bool PackageIsSchema
        {
            get
            {
                return _PackageIsSchema;
            }
            set
            {
                _PackageIsSchema = value;
            }
        }
        private string _DefaultPackage = "Data";
        public string DefaultPackage
        {
            get
            {
                return _DefaultPackage;
            }
            set
            {
                _DefaultPackage = value;
            }
        }
        private string _DefaultModule = "BL";
        public string DefaultModule
        {
            get
            {
                return _DefaultModule;
            }
            set
            {
                _DefaultModule = value;
            }
        }
        private string _EntityCollectionTransform = "EntityCollectionDef.xslt";
        public string EntityCollectionTransform
        {
            get
            {
                return _EntityCollectionTransform;
            }
            set
            {
                _EntityCollectionTransform = value;
            }
        }

        private String _ConnectionString;
        public String ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }

        private CodeLanguage _CodeLanguage = CodeLanguage.CSharp;
        [LookupFieldEditorAttibute("System.Windows.Forms.ComboBox", "Code Language", "CodeGenerator.BL.Generator.CodeLanguage", "Usage")]
        public CodeLanguage CodeLanguage
        {
            get
            {
                return _CodeLanguage;
            }
            set
            {
                _CodeLanguage = value;
            }
        }
        private string _Pattern = "General";

        public string Pattern
        {
            get { return _Pattern; }
            set { _Pattern = value; }
        }

        private string _ProjectRoot = @"C:\Documents and Settings\user\My Documents\Visual Studio 2005\Projects\menumaster\Data\";
        public string ProjectRoot
        {
            get
            {
                return _ProjectRoot;
            }
            set
            {
                _ProjectRoot = value;
            }
        }

        private bool _UseCollections = true;
        public bool UseCollections
        {
            get
            {
                return _UseCollections;
            }
            set
            {
                _UseCollections = value;
            }
        }

        private bool _ExportSingleFile = true;
        public bool ExportSingleFile
        {
            get
            {
                return _ExportSingleFile;
            }
            set
            {
                _ExportSingleFile = value;
            }
        }
        public string getEntityTransformPath()
        {
            string path;
            path = "..\\..\\Templates\\" + CodeLanguage.ToString() + "\\" + Pattern + "\\" + EntityTransform;
            return path;
        }
        public string getEntityCollectionTransformPath()
        {
            string path;
            path = "..\\..\\Templates\\" + CodeLanguage.ToString() + "\\" + Pattern + "\\" + EntityCollectionTransform;
            return path;
        }

        #region INodeElement Members

        public string getNodeName()
        {
            return "Settings";
        }

        public ModelTreeNode GetTreeView(TreeConfig treeConfig)
        {
            ModelTreeNode n = new ModelTreeNode("Settings");
            n.NodeObject = this;
            return n;
        }

        public void PerformContextAction(NodeElementContextAction actn, ModelTreeNode tn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
