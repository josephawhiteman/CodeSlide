using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using CodeGenerator.BL.Modeler;

namespace CodeGenerator.BL.DBReader
{
    [Serializable]
    public abstract class BaseDBFieldTypeMapper
    {
        public abstract string TypeName
        {
            get;
            set;
        }
        public abstract FieldType MapDBtoLogical();
    }
}
