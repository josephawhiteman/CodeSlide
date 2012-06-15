using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Types;

namespace CodeGenerator.BL.Modeler
{
    public enum DataType
    {
        _nil,
        _text,
        _character,
        _integer,
        _real,
        _imaginary,
        _boolean,
        _temporal,
        _geolocation,
        _ogc_shape,
        _guid,
        _bytesequence,
        _variant,
        _xml,
        _object   // representative of a complex object
    }
    
    public enum TemporalTypes
    {
        Date,
        Time,
        DateTime,
        UTC,
        UtcWithOffset
    }

    public enum Encodings
    {
        Unicode,
        UTF8,
        EBCDIC,
        ASCII,
    }

    public class FieldType
    {
        private DataType _LogicalDataType;

        [LookupFieldEditorAttibute("System.Windows.Forms.ComboBox", "Type", "CodeGenerator.BL.Modeler.DataType", "LogicalDataType")]
        public DataType LogicalDataType
        {
            get { return _LogicalDataType; }
            set { _LogicalDataType = value; }
        }

        private DateTime _MaxDate;
        public DateTime MaxDate
        {
            get { return _MaxDate; }
            set { _MaxDate = value; }
        }

        private DateTime _MinDate;
        public DateTime MinDate
        {
            get { return _MinDate; }
            set { _MinDate = value; }
        }

        private int _Max;
        public int Max
        {
            get { return _Max; }
            set { _Max = value; }
        }

        private int _Min;
        public int Min
        {
            get { return _Min; }
            set { _Min = value; }
        }
        private int _Precision;
        public int Precision
        {
            get { return _Precision; }
            set { _Precision = value; }
        }

        private string _Encoding;
        public string Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }

        private TemporalTypes _TemporalPresion;
        [LookupFieldEditorAttibute("System.Windows.Forms.ComboBox", "TemporalTypes", "CodeGenerator.BL.Modeler.TemporalTypes", "TemporalPresion")]
        public TemporalTypes TemporalPresion
        {
            get { return _TemporalPresion; }
            set { _TemporalPresion = value; }
        }
    }
}
