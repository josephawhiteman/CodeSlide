using System;
using System.Collections.Generic;
using System.Text;
using CodeGenerator.BL.DBReader;

namespace CodeGenerator.BL.Generator
{
    public class CSharpGenerator : LanguageGenerator
    {
        [Obsolete("Replaced with DbTypemapper.MaptoLogical",true)]
       public override BaseFieldType GetCodeType(BaseDBFieldTypeMapper dbType)
        {
            string name = String.Empty;
            CSharpFieldType type = new CSharpFieldType();
            switch (dbType.TypeName)
            {
                case "NCHAR":
                case "CHAR":
                    name = "char";
                    break;
                case "VARCHAR":
                case "NVARCHAR":
                case "TEXT":
                case "NTEXT":
                    name = "string";
                    break;
                case "TINYINT":
                case "SMALLINT":
                case "INT":
                    name = "int";
                    break;
                case "BIGINT":
                    name = "Int64";
                    break;
                case "IMAGE":
                case "BINARY":
                case "VARBINARY":
                    name = "byte[]";
                    break;
                case "DATETIME":
                case "TIMESTAMP":
                case "SMALLDATETIME":
                    name = "System.DateTime";
                    break;
                case "REAL":
                case "FLOAT":
                case "SMALLMONEY":
                case "NUMERIC":
                case "DECIMAL":
                    name = "double";
                    break;
                case "BIT":
                    name = "bool";
                    break;
                case "UNIQUEIDENTIFIER":
                    name = "System.Guid";
                    break;
                default:
                    name = dbType.TypeName ;
                    break;
            }
            
            type.Name = name;
            return type;
        }
        public override string CodeFileExtension
        {
            get
            {
                return "cs";
            }
        }
    }
}
