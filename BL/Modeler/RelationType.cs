using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CodeGenerator.BL.Modeler
{

    public enum TypeUsage
    {
        Inherited = 1,
        Lookup = 2,
        Parent = 0,
        Association = 3,
        Denormalization = 4
    }
    public enum Cardinality
    {
        Zero,
        ZeroOne,
        One,
        OneMany,
        ZeroMany,
        Many
    }
    [Serializable()]
    public class RelationType
    {
        private TypeUsage _usage;
        /// <summary>
        /// this is a summary
        /// </summary>
        /// <remarks>more remarks</remarks>
        /// <value>RelationType.UsageType.Parent</value>
        /// 
        [LookupFieldEditorAttibute("System.Windows.Forms.ComboBox", "RelationType", "CodeGenerator.BL.Modeler.TypeUsage", "Usage")]
        public TypeUsage Usage
        {
            get
            {
                return _usage;
            }
            set
            {
                _usage = value;
            }
        }
        private Cardinality _Ordinality;
        [LookupFieldEditorAttibute("System.Windows.Forms.ComboBox", "RelationType", "CodeGenerator.BL.Modeler.Cardinality", "Cardinality")]
        public Cardinality Ordinality
        {
            get { return _Ordinality; }
            set { _Ordinality = value; }
        }

        private Cardinality _Cardinality;
        [LookupFieldEditorAttibute("System.Windows.Forms.ComboBox", "RelationType", "CodeGenerator.BL.Modeler.Cardinality", "Cardinality")]
        public Cardinality Cardinality
        {
            get { return _Cardinality; }
            set { _Cardinality = value; }
        }
    }
}
