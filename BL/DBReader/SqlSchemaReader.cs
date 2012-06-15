using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using CodeGenerator.BL.Modeler;
using CodeGenerator.BL.Generator;
using CodeGenerator.BL;

namespace CodeGenerator.BL.DBReader
{
    public class SqlSchemaReader : BaseSchemaReader
    {

        public SqlSchemaReader(LanguageGenerator langGen, string connString)
            : base(langGen, connString)
        {
        }


        public override EntityCollection RetrieveEntities(Model model)
        {
            Entity ent;
            EntityCollection entities = new EntityCollection();
            DS.SchemaReader.EntitiesDataTable dt = new CodeGenerator.BL.DBReader.DS.SchemaReader.EntitiesDataTable();

            DS.SchemaReaderTableAdapters.EntitiesTableAdapter adp = new CodeGenerator.BL.DBReader.DS.SchemaReaderTableAdapters.EntitiesTableAdapter();
            adp.Connection = new System.Data.SqlClient.SqlConnection(base.ConnectionString);

            adp.Fill(dt , Filter.IncludeTablesAsEntities, Filter.IncludeViewAsEntities, Filter.IncludeForignTables,Filter.SchemasAsCSV() );
            base.EntityCount = dt.Rows.Count;
            if (WorkerThread != null && WorkerThread.WorkerReportsProgress)
            {
                WorkerThread.ReportProgress(0);
            }
            int count = 1;
            foreach (DS.SchemaReader.EntitiesRow row in dt.Rows)
            {
                ent = new Entity();
                ent.Model = model;
                ent.DBName = row.TABLE_NAME;
                ent.Schema = row.TABLE_SCHEMA;
                ent.EntityID = row.OBJECT_ID.ToString();
                ent.LogicalName =   row.TABLE_NAME;

                FillProperties(ent, adp.Connection, null, "schema", ent.Schema, "table", ent.DBName);
                LogicalNameGenerator gen = new LogicalNameGenerator();
                gen.GenerateLogicalName(ent);
                entities.Add(ent);
                if (WorkerThread != null && WorkerThread.WorkerReportsProgress)
                {
                    WorkerThread.ReportProgress((count / base.EntityCount)*100);
                }
                count++;
            }
            return entities;
        }

        private void FillProperties(ModelElementBase ele, System.Data.SqlClient.SqlConnection conn, params string[] prms)
        {
            string[] parameters = new string[7];
            for( int i=0;i<prms.Length ;i++)
            {
                parameters[i] = prms[i];
            }
            DS.SchemaReaderTableAdapters.DBObjPropertiesTableAdapter dbProps = new CodeGenerator.BL.DBReader.DS.SchemaReaderTableAdapters.DBObjPropertiesTableAdapter();
            dbProps.Connection = conn;
            DS.SchemaReader.DBObjPropertiesDataTable dtProps = new CodeGenerator.BL.DBReader.DS.SchemaReader.DBObjPropertiesDataTable();
            dbProps.Fill(dtProps, parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5], parameters[6]);

            foreach (DS.SchemaReader.DBObjPropertiesRow rprop in dtProps.Rows)
            {
                ele.Properties.Add(new PropertyPair(rprop.name.ToString(), rprop.value.ToString()));
            }
        }

        public override void PopulateFields(Entity entity)
        {
            DS.SchemaReader.FieldDefinitionsDataTable dt = new CodeGenerator.BL.DBReader.DS.SchemaReader.FieldDefinitionsDataTable();
            DS.SchemaReaderTableAdapters.FieldDefinitionsTableAdapter adp = new CodeGenerator.BL.DBReader.DS.SchemaReaderTableAdapters.FieldDefinitionsTableAdapter();
            adp.Connection = new System.Data.SqlClient.SqlConnection(base.ConnectionString);
            adp.FillByTableName(dt, entity.DBName);

            foreach (DS.SchemaReader.FieldDefinitionsRow row in dt)
            {
                Field f;
                f = entity.Fields.Find(delegate(Field ff) { return ff.DBName == row.column_name; });
                if (f == null)
                {
                    f = new Field();
                    entity.Fields.Add(f);
                }
                f.DBName = row.column_name;
                f.LogicalName = row.column_name;
                f.DBType = row.Data_type;
                f.ID = row.object_id.ToString();
                SqlFieldTypeMapper fieldType = new SqlFieldTypeMapper();
                fieldType.TypeName = row.Data_type;

//                f.LogicalType = CodeGenerator.GetCodeType(fieldType);
                f.LogicalType = fieldType.MapDBtoLogical();
                f.Nullable = Convert.ToBoolean(row.is_nullable.ToLower() == "no"? false:true);

            }
        }

        public override void PopulateTableConstraints(Entity entity)
        {
            DS.SchemaReader.TABLE_CONSTRAINTSDataTable dt = new CodeGenerator.BL.DBReader.DS.SchemaReader.TABLE_CONSTRAINTSDataTable();
            DS.SchemaReaderTableAdapters.TABLE_CONSTRAINTSTableAdapter adp = new CodeGenerator.BL.DBReader.DS.SchemaReaderTableAdapters.TABLE_CONSTRAINTSTableAdapter();
            adp.Connection = new System.Data.SqlClient.SqlConnection(base.ConnectionString);
            adp.FillByTableName(dt, entity.DBName);
            foreach (DS.SchemaReader.TABLE_CONSTRAINTSRow row in dt)
            {
                Constraint con = new Constraint(entity);
                con.Name = row.CONSTRAINT_NAME;
                con.LogicalName = row.CONSTRAINT_NAME;
                con.Type = row.CONSTRAINT_TYPE;
                con.RelatedEntityName = row.TABLE_NAME;

                entity.Constraints.Add(con);
                PopulateConstraintFields(con);
            }
        }
        protected override void PopulateConstraintFields(Constraint con)
        {
            DS.SchemaReader.CONSTRAINT_COLUMN_USAGEDataTable dt = new CodeGenerator.BL.DBReader.DS.SchemaReader.CONSTRAINT_COLUMN_USAGEDataTable();
            DS.SchemaReaderTableAdapters.CONSTRAINT_COLUMN_USAGETableAdapter adp = new CodeGenerator.BL.DBReader.DS.SchemaReaderTableAdapters.CONSTRAINT_COLUMN_USAGETableAdapter();
            adp.Connection = new System.Data.SqlClient.SqlConnection(base.ConnectionString);
            adp.FillByConstraintName(dt, con.Name);
            foreach (DS.SchemaReader.CONSTRAINT_COLUMN_USAGERow row in dt)
            {
                Field f;
                f = con.Entity.Fields.Find(delegate(Field ff) { return ff.DBName == row.COLUMN_NAME; });
                if (f == null)
                {
                    f = new Field();
                }
                f.DBName = row.COLUMN_NAME;
                f.LogicalName = row.COLUMN_NAME;
                FieldReference fr = new FieldReference();
                fr.RelatedField = f;
                fr.ID = f.ID;
                fr.Name = f.LogicalName;
                con.Fields.Add(fr);
            }
        }


   
        public override void PopulateForeignRelations(Entity entity)
        {
            Entity prntEnt;
            Entity frnEntity = null;

            DS.SchemaReader.ForeignRelationsDataTable tr = new DS.SchemaReader.ForeignRelationsDataTable();
            DS.SchemaReaderTableAdapters.ForeignRelationsTableAdapter tra = new DS.SchemaReaderTableAdapters.ForeignRelationsTableAdapter();
            tra.Connection = new System.Data.SqlClient.SqlConnection(base.ConnectionString);
            tra.FillBy(tr, entity.DBName);


            DS.SchemaReader.TABLE_CONSTRAINTSDataTable tc = new DS.SchemaReader.TABLE_CONSTRAINTSDataTable();
            DS.SchemaReaderTableAdapters.TABLE_CONSTRAINTSTableAdapter tca = new DS.SchemaReaderTableAdapters.TABLE_CONSTRAINTSTableAdapter();
            tca.Connection = new System.Data.SqlClient.SqlConnection(base.ConnectionString);
            tca.FillByTableName(tc,entity.DBName);
            System.Data.DataRow[] tcrows = tc.Select("CONSTRAINT_TYPE = 'FOREIGN KEY'");
            foreach (DS.SchemaReader.TABLE_CONSTRAINTSRow tcr in tcrows)
            {
                System.Data.DataRow[] trrows = tr.Select("ConstraintName = '" + tcr.CONSTRAINT_NAME + "'");
                DS.SchemaReader.ForeignRelationsRow trr = (DS.SchemaReader.ForeignRelationsRow)trrows[0];
                string ftab = trr.RefTab; ;
                prntEnt = entity.Model.EntityCollection.Find(delegate(Entity ent) { return ent.DBName == ftab; });

                // this code assumes that the foreign keys' source is the primary key 
                // allthough the source could be a unique key
                Constraint pk = prntEnt.Constraints.Find(delegate(Constraint c) { return c.Type == "PRIMARY KEY"; });
                Constraint fk = entity.Constraints.Find(delegate(Constraint c) { return c.Name == tcr.CONSTRAINT_NAME; });
                Relation rel = new Relation();

                EntityReference child = new EntityReference();
                child.Entity = entity;
                child.EntityID = entity.EntityID;
               // child.EntityName = entity.LogicalName;
                rel.ChildEntity  = child;


                EntityReference parent = new EntityReference();
                parent.Entity = prntEnt;
                parent.EntityID = prntEnt.EntityID;
                // parent.EntityName = prntEnt.LogicalName;
                rel.ParentEntity = parent;


                RelationReference relref = new RelationReference();
                relref.Relation = rel;
                relref.ID = System.Guid.NewGuid().ToString();
                entity.ParentRelations.Add(relref);
                prntEnt.ChildRelations.Add(relref);

                rel.ID = relref.ID;

                foreach (FieldReference frf in pk.Fields)
                {
                    rel.RelatedFields.Add(frf);
                }
                foreach (FieldReference pkf in fk.Fields)
                {
                    rel.ForeignFields.Add(pkf);
                }
                //rel.RelatedEntityID = prntEnt.EntityID;
                rel.Type = new RelationType();

                rel.Type.Usage = TypeUsage.Parent;

                string logical = child.Entity.LogicalName + "." + fk.Fields[0].RelatedField.DBName;
                rel.LogicalName = logical;
                entity.Model.Relations.Add(rel);
            }
        }

        protected override void RetrieveSchemas(Model model)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
