using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using CodeGenerator.BL.Modeler;

namespace CodeGenerator.BL.DBReader
{
    [Serializable]
    public class SqlFieldTypeMapper : BaseDBFieldTypeMapper
    {

        public enum SQLDBFieldType
        {
            GEOGRAPHY       =   13,
            GEOMETRY        =   14,
            HIERARCHYID     =   15,
            IMAGE           =   34,
            TEXT	        =	35,
            UNIQUEIDENTIFIER=	36,
            DATE            =   40,
            TIME            =   41,
            TINYINT         =   48,
            DATETIME2       =   42,
            DATETIMEOFFSET  =   43,
            SMALLINT        =   52,
            INT	            =	56,
            SMALLDATETIME	=	58,
            REAL	        =	59,
            MONEY	        =	60,
            DATETIME	    =	61,
            FLOAT	        =	62,
            SQL_VARIANT	    =	98,
            NTEXT	        =	99,
            BIT	            =	104,
            DECIMAL	        =	106,
            NUMERIC	        =	108,
            SMALLMONEY	    =	122,
            BIGINT	        =	127,
            VARBINARY	    =	165,
            VARCHAR	        =	167,
            BINARY	        =	173,
            CHAR	        =	175,
            TIMESTAMP	    =	189,
            NVARCHAR	    =	231,
            NCHAR	        =	239,
            XML	            =	241,
            SYSNAME	        =	231
        }

        public override String TypeName
        {
            get
            {
                return _type.ToString();
            }
            set
            {
                _type = (SQLDBFieldType)Enum.Parse(typeof(SQLDBFieldType), value, true);
            }
        }
        private SQLDBFieldType _type;
        public SQLDBFieldType Type
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

        public override FieldType MapDBtoLogical()
        {
            DataType typ = DataType._nil;
            FieldType type = new FieldType();
            switch (_type)
            {
                case SQLDBFieldType.NCHAR:
                case SQLDBFieldType.CHAR:

                    typ = DataType._character;
                    break;

                case SQLDBFieldType.VARCHAR:
                case SQLDBFieldType.NVARCHAR:
                case SQLDBFieldType.TEXT:
                case SQLDBFieldType.NTEXT:

                    typ = DataType._text;
                    break;

                case SQLDBFieldType.TINYINT:
                case SQLDBFieldType.SMALLINT:
                case SQLDBFieldType.INT:

                    typ = DataType._integer;
                    break;

                case SQLDBFieldType.BIGINT:

                    typ = DataType._integer;
                    break;

                case SQLDBFieldType.IMAGE:
                case SQLDBFieldType.BINARY:
                case SQLDBFieldType.VARBINARY:

                    typ = DataType._bytesequence;
                    break;

                case SQLDBFieldType.TIME:
                case SQLDBFieldType.DATETIME:
                case SQLDBFieldType.TIMESTAMP:
                case SQLDBFieldType.SMALLDATETIME:
                case SQLDBFieldType.DATETIMEOFFSET:
                case SQLDBFieldType.DATETIME2:
                case SQLDBFieldType.DATE:
                    typ = DataType._temporal;
                    break;

                case SQLDBFieldType.REAL:
                case SQLDBFieldType.FLOAT:
                case SQLDBFieldType.SMALLMONEY:
                case SQLDBFieldType.MONEY:
                case SQLDBFieldType.NUMERIC:
                case SQLDBFieldType.DECIMAL:

                    typ = DataType._real;
                    break;

                case SQLDBFieldType.BIT:

                    typ = DataType._boolean;
                    break;

                case SQLDBFieldType.UNIQUEIDENTIFIER:

                    typ = DataType._guid;
                    break;
                case SQLDBFieldType.SQL_VARIANT:
                    typ = DataType._variant;
                    break;
                case SQLDBFieldType.XML:
                    typ = DataType._xml;
                    break;

                case SQLDBFieldType.HIERARCHYID:
                    typ = DataType._object;
                    break;

                 case SQLDBFieldType.GEOGRAPHY:
                     typ = DataType._geolocation;
                    break;

                case SQLDBFieldType.GEOMETRY:
                    typ = DataType._ogc_shape;
                    break;
            }

            type.LogicalDataType = typ;
            return type;
        }
    }
}
