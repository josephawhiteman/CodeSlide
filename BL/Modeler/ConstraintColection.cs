using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace CodeGenerator.BL.Modeler
{
    [Serializable]
    public class ConstraintCollection : System.Collections.ObjectModel.Collection<CodeGenerator.BL.Modeler.Constraint>
    {

        public Constraint Find(Predicate<Constraint> match)
        {
            Constraint ret = null;
            foreach (Constraint c in base.Items)
            {
                if (match(c)) ret = c;
            }
            return ret;
        }
        public void AddRange(ConstraintCollection fields)
        {
            foreach (Constraint f in fields)
            {
                base.Add(f);
            }
        }

    }
}
