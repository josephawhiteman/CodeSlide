using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator.BL.Generator
{
    public class PHPGenerator : LanguageGenerator
    {
        public override BaseFieldType GetCodeType(CodeGenerator.BL.DBReader.BaseDBFieldTypeMapper dbType)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override string CodeFileExtension
        {
            get
            {
                return "ph";
            }
        }
    }
}
