using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml.Serialization;

namespace CodeGenerator.BL.Modeler
{
    [Serializable]
    [XmlInclude(typeof(List<DictionaryEntry>))]
    [XmlInclude(typeof(DictionaryEntry))]
    public class ModelElementBase
    {
        private List<PropertyPair> _Properties;
        [PropertyFieldEditorAttribute("System.Windows.Forms.DataGridView", "Properties", "Properties")]
        public List<PropertyPair> Properties
        {
            get
            {
                if (_Properties == null)
                    _Properties = new List<PropertyPair>();
                return _Properties;
            }
            set
            {
                _Properties = value;
            }
        }
    }
    [Serializable]
    public class PropertyPair
    {
        public PropertyPair(string name, string value)
        {
            _name = name;
            _value = value;
        }
        public PropertyPair() { }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
