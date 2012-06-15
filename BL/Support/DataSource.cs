using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator.BL.Support
{
    public class DataSource
    {
        private String _ConnectionString;

        public String ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }
        public override string ToString()
        {
            return _ConnectionString;
        }
    }
}
