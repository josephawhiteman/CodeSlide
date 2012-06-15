using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CodeGenerator.BL.Generator
{
    [Serializable]
    public class CSharpFieldType : BaseFieldType 
    {
        public override String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public override string Primitive
        {
            get
            {
                return _Primitive;
            }
            set
            {
                _Primitive = value;
            }
        }
    }
}
