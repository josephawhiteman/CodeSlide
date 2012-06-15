using System;
using System.Collections.Generic;
using System.Text;
using CodeGenerator.BL.Generator;



namespace CodeGenerator.BL.Support
{
    public class Settings
    {
        private static string TIMESTAMP = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Ticks.ToString();
        private string _FileName = "..\\..\\XML\\Outfile" + Settings.TIMESTAMP + ".xml";
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
            }
        }

        private string _xmlNameSpace = "http://schemas.techtemp.ca";
        public string XmlNameSpace
        {
            get { return _xmlNameSpace; }
            set { _xmlNameSpace = value; }
        }

        private DataSourceCollection _Connections;
        public DataSourceCollection Connections
        {
            get
            {
                if (_Connections == null)
                {
                    _Connections = new DataSourceCollection();
                    System.Collections.Specialized.StringCollection cons = CodeGenerator.Properties.Settings.Default.DBConnections;
                    foreach(string con in cons)
                    {
                        DataSource ds = new DataSource();
                        ds.ConnectionString = con;
                        _Connections.Add(ds);
                    }
                }
                return _Connections;
            }
            set
            {
                _Connections = value;
            }
        }

    }
}
