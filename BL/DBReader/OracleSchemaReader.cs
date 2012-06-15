using System;
using System.Collections.Generic;
using System.Text;
using CodeGenerator.BL.Modeler;
using CodeGenerator.BL.Generator;


namespace CodeGenerator.BL.DBReader
{
    public class OracleSchemaReader : BaseSchemaReader
    {
        public OracleSchemaReader(LanguageGenerator langGen, string connString)
            : base(langGen, connString)
        {
        }

        public override CodeGenerator.BL.Modeler.EntityCollection RetrieveEntities(Model model)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void PopulateFields(CodeGenerator.BL.Modeler.Entity entity)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //public override void PopulateChildRelations(CodeGenerator.BL.Modeler.Entity entity)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        public override void PopulateForeignRelations(CodeGenerator.BL.Modeler.Entity entity)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void PopulateTableConstraints(CodeGenerator.BL.Modeler.Entity entity)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void PopulateConstraintFields(CodeGenerator.BL.Modeler.Constraint con)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void RetrieveSchemas(Model model)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
