using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace CodeGenerator.BL.Modeler
{
    [Serializable]
    public class Constraint
    {

        private Entity _Entity;
        private String _Name;
        private String _Type;
        private string _LogicalName;
        public Constraint()
        {
        }
        public Constraint(Entity entity)
        {
            _Entity = entity;
        }
        [XmlIgnore]
        public Entity Entity
        {
            get
            {
                return _Entity;
            }
            set
            {
                _Entity = value;
            }
        }
        
        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public String Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }

        public string LogicalName
        {
            get
            {
                return _LogicalName;
            }
            set
            {
                _LogicalName = value;
            }
        }
        private string _RelatedEntityName;
        public string RelatedEntityName
        {
            get
            {
                return _RelatedEntityName;
            }
            set
            {
                _RelatedEntityName = value;
            }
        }
        private FieldReferenceCollection _fields;
        public FieldReferenceCollection Fields
        {
            get
            {
                if (_fields == null)
                {
                    _fields = new FieldReferenceCollection();
                }
                return _fields;
            }
            set
            {
                _fields = value;
            }
        }

    }
}
