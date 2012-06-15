using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator.BL.DBReader
{
    public class DBReaderFilter
    {
        private List<string> _Schemas;

        public List<string> Schemas
        {
            get {
                if (_Schemas == null)
                {
                    _Schemas = new List<string>();
                }
                return _Schemas; 
            }
            set { _Schemas = value; }
        }
        public string SchemasAsCSV()
        {
            string res = String.Empty;
            foreach (string str in Schemas)
            {
                res = res + str + ",";
            }
            res = res.TrimEnd(',');
            return res;
        }


        private bool _IncludeViewAsEntities;
        public bool IncludeViewAsEntities
        {
            get
            {
                return _IncludeViewAsEntities;
            }
            set
            {
                _IncludeViewAsEntities = value;
            }
        }
        private string _LogicalEntityNameRegex;
        public string LogicalEntityNameRegex
        {
            get
            {
                return _LogicalEntityNameRegex;
            }
            set
            {
                _LogicalEntityNameRegex = value;
            }
        }
        private bool _IncludeTablesAsEntities = true;
        public bool IncludeTablesAsEntities
        {
            get
            {
                return _IncludeTablesAsEntities;
            }
            set
            {
                _IncludeTablesAsEntities = value;
            }
        }
        
        private bool _IncludeForignTables = true;
        public bool IncludeForignTables
        {
            get
            {
                return _IncludeForignTables;
            }
            set
            {
                _IncludeForignTables = value;
            }
        }
    }
}
