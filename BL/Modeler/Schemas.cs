using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator.BL.Modeler
{
    class Schemas
    {
        private string _SchemaName;
        public string SchemaName
        {
            get { return _SchemaName; }
            set { _SchemaName = value; }
        }
        private string _SchemaOwner;

        public string SchemaOwner
        {
            get { return _SchemaOwner; }
            set { _SchemaOwner = value; }
        }

        private EntityCollection _Entities;
        public EntityCollection Entities
        {
            get {
                if (_Entities == null)
                {
                    _Entities = new EntityCollection();
                }
                return _Entities; 
            }
            set { _Entities = value; }
        } 
    }
}
