using System;
using System.Collections.Generic;
using System.Text;
using CodeGenerator.BL.Modeler;
using CodeGenerator.BL.Generator;

namespace CodeGenerator.BL.DBReader
{
    public abstract class BaseSchemaReader
    {

        protected BaseSchemaReader(LanguageGenerator langMap, string connString)
        {
            _CodeGenerator = langMap;
            _ConnectionString = connString;
        }

        private System.ComponentModel.BackgroundWorker _WorkerThread;
        public System.ComponentModel.BackgroundWorker WorkerThread
        {
            get
            {
                return _WorkerThread;
            }
            set
            {
                _WorkerThread = value;
            }
        }

        private int _EntityCount;
        public int EntityCount
        {
            get
            {
                return _EntityCount;
            }
            set
            {
                _EntityCount = value;
            }
        }

        private string _ConnectionString;
        public string ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }

        private LanguageGenerator _CodeGenerator;
        public LanguageGenerator CodeGenerator
        {
            get
            {
                return _CodeGenerator;
            }
        }



        public int DBEntities
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        private DBReaderFilter _Filter;

        public DBReaderFilter Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }

        protected abstract void RetrieveSchemas(Model model);

        public abstract EntityCollection RetrieveEntities(Model model);

        public abstract void PopulateFields(Entity entity);

        public abstract void PopulateForeignRelations(Entity entity);

        public abstract void PopulateTableConstraints(Entity entity);

        protected abstract void PopulateConstraintFields(Constraint con);

    }
}
