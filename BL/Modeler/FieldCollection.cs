using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CodeGenerator.BL.Modeler
{
    [Serializable]
    public class FieldCollection : System.Collections.Generic.List<CodeGenerator.BL.Modeler.Field>
    {

        //public Field Find(Predicate<Field> match)
        //{
        //    Field ret = null;
        //    foreach (Field f in base.Items)
        //    {
        //        if (match(f)) ret = f;
        //    }
        //    return ret;
        //}
        //public void AddRange(FieldCollection fields)
        //{
        //    foreach(Field f in fields)
        //    {
        //        base.Add(f);
        //    }
        //}
        
    }
}
