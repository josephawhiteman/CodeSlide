using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace CodeGenerator.BL.Modeler
{
    public class EntityReference
    {
        private Entity _Entity;
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
        private string _EntityID;
        [XmlAttribute("IDREF")]
        public string EntityID
        {
            get
            {
                return _EntityID;
            }
            set
            {
                _EntityID = value;
            }
        }
        //private string _EntityName;
        //public string EntityName
        //{
        //    get
        //    {
        //        return _EntityName;
        //    }
        //    set
        //    {
        //        _EntityName = value;
        //    }
        //}
    }
}
