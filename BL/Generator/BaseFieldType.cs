using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CodeGenerator.BL.Generator
{


    [Serializable]
    public abstract class BaseFieldType
    {
        protected String _Name;
        public abstract String Name
        {
            get;
            set;
        }
        protected String _Primitive;
        public abstract string Primitive
        {
            get;
            set;
        }
    }
}
