using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace CodeGenerator.BL.Modeler
{
    [Serializable]
    public class FieldReference
    {
        private Field _RelatedField;
        [XmlIgnore]
        public Field RelatedField
        {
            get { return _RelatedField; }
            set { _RelatedField = value; }
        }
        private string _ID;
        [XmlAttribute("IDREF")]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _Name;
        [XmlAttribute]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
    }
}
