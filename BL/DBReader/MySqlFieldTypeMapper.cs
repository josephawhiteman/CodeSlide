using System;
using System.Collections.Generic;
using System.Text;
using CodeGenerator.BL.Modeler;

namespace CodeGenerator.BL.DBReader
{
    public class MySqlFieldTypeMapper : BaseDBFieldTypeMapper
    {
        public override string TypeName
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public override FieldType MapDBtoLogical()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
