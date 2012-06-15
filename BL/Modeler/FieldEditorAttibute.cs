using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;


namespace CodeGenerator.BL.Modeler
{
    class FieldEditorIgnore : Attribute
    {
        public FieldEditorIgnore()
        {
        }
    }
    class FieldEditorReadonly : Attribute
    {
        public FieldEditorReadonly()
        {
        }
    }
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    class FieldEditorAttibute : System.Attribute
    {
        public FieldEditorAttibute(string controlName, string labelKey )
        {
            _ControlName = controlName;
            _LabelKey = labelKey;
        }

        private string _ControlName;

        public string ControlName
        {
            get { return _ControlName; }
            set { _ControlName = value; }
        }
        private string _LabelKey;

        public string LabelKey
        {
            get { return _LabelKey; }
            set { _LabelKey = value; }
        }
        public virtual void loadControl(Control ctl, PropertyInfo pi, object obj)
        {
            ctl.Text = pi.GetValue(obj, null).ToString();

        }
        public virtual void StoreValue(Control ctl, PropertyInfo pi, object obj)
        {
        }
    }
    class ComplexFieldEditorAttribute : FieldEditorAttibute
    {
        public ComplexFieldEditorAttribute(string controlName, string labelKey, string valueProperty)
            : base(controlName, labelKey)
        {
            _ValueProperty = valueProperty;
        }
        private string _ValueProperty;

        public string ValueProperty
        {
            get { return _ValueProperty; }
            set { _ValueProperty = value; }
        }
        public override void loadControl(Control ctl, PropertyInfo pi, object obj)
        {
            object o = pi.GetValue(obj, null);
            PropertyInfo info = o.GetType().GetProperty(ValueProperty);
            string sitm = info.GetValue(o, null).ToString();
            ctl.Text = sitm;
        }
    }
    class CollectionFieldEditorAttribute : FieldEditorAttibute
    {
        public CollectionFieldEditorAttribute(string controlName, string labelKey, string listPath, string valueProperty)
            : base(controlName, labelKey)
        {
            _ValueProperty = valueProperty;
            _ListPath = listPath;
        }

        private string _ValueProperty;

        public string ValueProperty
        {
            get { return _ValueProperty; }
            set { _ValueProperty = value; }
        }

        private string _ListPath;
        public string ListPath
        {
            get { return _ListPath; }
            set { _ListPath = value; }
        }

        // example Entity.Fields collection  for the list options
        public override void loadControl(Control ctl, PropertyInfo pi, object obj)
        {

            // obj starts as relation the first path element is Entity
            ListBox lx = (ListBox)ctl;
            object o = obj;
            PropertyInfo pathitem = pi;
            PropertyInfo newItem;
            string[] path = ListPath.Split('.');

            Object sel = pi.GetValue(obj, null);
            System.Collections.ICollection selItms = (System.Collections.ICollection)sel;

            foreach (string pathElement in path)
            {
                newItem = o.GetType().GetProperty(pathElement); // path[0] is a property of obj
                o = newItem.GetValue(o, null);   // this first becomes The Entity object
            }
            System.Collections.ICollection coll = (System.Collections.ICollection) o;
            foreach (object f in coll)
            {
                PropertyInfo fInfo = f.GetType().GetProperty("ID");
                object I1 = fInfo.GetValue(f, null);
                PropertyInfo itmInfo = f.GetType().GetProperty(ValueProperty);
                int ndx = lx.Items.Add(itmInfo.GetValue(f, null));

                foreach (object selitm in selItms)
                {
                    PropertyInfo selInfo = selitm.GetType().GetProperty("ID");
                    object I2 = selInfo.GetValue(selitm, null);
                    if (I1.ToString() == I2.ToString())
                    {
                        lx.SelectedIndex = ndx;
                    }
                }

            }
            //PropertyInfo info = o.GetType().GetProperty(ValueProperty);
            //string sitm = info.GetValue(o, null).ToString();
            //ctl.Text = sitm;
        }
    }
    class PropertyFieldEditorAttribute : FieldEditorAttibute
    {
        public PropertyFieldEditorAttribute(string controlName, string labelKey,string propval)
            : base(controlName, labelKey)
        {
            _ValueProperty = propval;
        }

        private string _ValueProperty;

        public string ValueProperty
        {
            get { return _ValueProperty; }
            set { _ValueProperty = value; }
        }

        // example Entity.Fields collection  for the list options
        public override void loadControl(Control ctl, PropertyInfo pi, object obj)
        {

            Object lst = pi.GetValue(obj, null);
            System.Collections.Generic.List<PropertyPair> lstItms = (System.Collections.Generic.List<PropertyPair>)lst;
            // obj starts as relation the first path element is Entity
            DataGridView lx = (DataGridView)ctl;
            lx.EditMode = DataGridViewEditMode.EditOnEnter;
            lx.BorderStyle = BorderStyle.Fixed3D;
            lx.AllowUserToAddRows = true;
            lx.AllowUserToDeleteRows = true;
            lx.ShowEditingIcon = true;
//            lx.
            lx.DataSource = lstItms;

        }
    }
    class LookupFieldEditorAttibute : FieldEditorAttibute
    {
        public LookupFieldEditorAttibute(string controlName, string labelKey, string enumTypeLookup, string valueProperty ):base(controlName,labelKey)
        {
            _EnumTypeLookup = enumTypeLookup;
            _ValueProperty = valueProperty;
        }
        private string _EnumTypeLookup;
        public string EnumTypeLookup
        {
            get { return _EnumTypeLookup; }
            set { _EnumTypeLookup = value; }
        }
        private string _ValueProperty;
        public string ValueProperty
        {
            get { return _ValueProperty; }
            set { _ValueProperty = value; }
        }


        public override void loadControl(Control ctl, PropertyInfo pi, object obj)
        {
            string sitm = string.Empty;
            ComboBox cmb = (ComboBox) ctl;
            Type lu = Type.GetType(EnumTypeLookup);
            string[] names = Enum.GetNames(lu);
            if (lu.Equals(pi.PropertyType))
            {
                sitm = pi.GetValue(obj, null).ToString();
            }
            else
            {
                object o = pi.GetValue(obj, null);
                PropertyInfo info = o.GetType().GetProperty(ValueProperty);
                sitm = info.GetValue(o, null).ToString();
            }
            int i = 0;
            foreach(string name in names)
            {
                cmb.Items.Add(name);
                if( name == sitm)
                {
                    cmb.SelectedIndex = i;
                }
                i++;
            }
        }
        public override void StoreValue(Control ctl, PropertyInfo pi, object obj)
        {
            base.StoreValue(ctl, pi, obj);
            ComboBox cmb = (ComboBox)ctl;
            string item = (string)cmb.SelectedItem;
            Type lu = Type.GetType(EnumTypeLookup);

            pi.SetValue(obj, Enum.Parse(lu, item, true), null);


        }
    }
}
