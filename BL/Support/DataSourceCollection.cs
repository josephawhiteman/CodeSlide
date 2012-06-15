using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator.BL.Support
{
    public class DataSourceCollection : List<DataSource>
    {
        public DataSourceCollection()
        {

        }
        private int _ActiveDataSourceIndex;
        public DataSource ActiveDataSource
        {
            get { return this[_ActiveDataSourceIndex]; }
            set
            {
                int newIndex;
                newIndex = this.FindIndex(delegate(DataSource ds) {return  ds.ConnectionString == value.ConnectionString; });
                if(newIndex < 0)
                {
                    throw new Exception("DataSource Not found");
                }
                else
                {
                    _ActiveDataSourceIndex = newIndex;
                }

            }
        }
        public void AddDataSource(DataSource ds)
        {
            base.Add(ds);
            _ActiveDataSourceIndex = base.Count -1;
        }
    }
}
