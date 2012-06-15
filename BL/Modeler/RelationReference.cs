using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CodeGenerator.BL.Modeler
{
    [Serializable]
    public class RelationReference
    {
        private Relation _relation;
        [XmlIgnore]
        public Relation Relation
        {
            get { return _relation; }
            set { _relation = value; }
        }
        private string _ID;
        [XmlAttribute("IDREF")]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
    }
}
