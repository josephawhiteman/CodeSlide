using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CodeGenerator.BL.DBReader;
using CodeGenerator.BL.Modeler;
using CodeGenerator.BL.Generator;
using CodeGenerator.BL.Support;
using CodeGenerator.BL.DBReader.DS;


namespace CodeGenerator.Forms
{
    public partial class frmConnections : Form
    {
       public delegate void DBLoadedEventHandler(Object sender, DBLoadedEventArgs args);
       public event DBLoadedEventHandler loadedEvent;
        public frmConnections(Project proj)
        {
            InitializeComponent();
            progressBarLoad.Value = 0;
            workerLoadDB.DoWork += new DoWorkEventHandler(LoadDB_DoWork);
            workerLoadDB.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadDB_RunWorkerCompleted);
            workerLoadDB.ProgressChanged += new ProgressChangedEventHandler(LoadDB_ProgressChanged);
            _project = proj;
            Settings settings = new Settings();
            _filter = new DBReaderFilter();
            checkBox1.Checked = _filter.IncludeViewAsEntities;
            checkBox2.Checked = _filter.IncludeTablesAsEntities;
            foreach( DataSource source in  settings.Connections)
            {
                comboBox1.Items.Add(source);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void loadSchemas()
        {
            try
            {
                SchemaReader.SCHEMATADataTable tbl = new SchemaReader.SCHEMATADataTable();
                CodeGenerator.BL.DBReader.DS.SchemaReaderTableAdapters.SCHEMATATableAdapter adp = new CodeGenerator.BL.DBReader.DS.SchemaReaderTableAdapters.SCHEMATATableAdapter();
                adp.Connection = new System.Data.SqlClient.SqlConnection(comboBox1.Items[comboBox1.SelectedIndex].ToString());
                adp.Fill(tbl);
                listBox1.DisplayMember = tbl.SCHEMA_NAMEColumn.ColumnName;     
                listBox1.ValueMember = tbl.SCHEMA_NAMEColumn.ColumnName;
                listBox1.DataSource = tbl;

            }catch(Exception ex)
            {
            }
        }
        private DBReaderFilter _filter;

        private Project _project;
        private Project ProjectDefinition
        {
            get
            {
                if (_project == null)
                {
                    _project = new Project();
                }
                return _project;
            }
            set
            {
                _project = value;
            }
        }

        void LoadDB_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarLoad.Value = e.ProgressPercentage;

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            dataSource = (DataSource)comboBox1.SelectedItem;
            if (dataSource == null)
            {
                string connstr = comboBox1.Text;
                dataSource = new DataSource();
                dataSource.ConnectionString = connstr;
                Settings settings = new Settings();
                settings.Connections.Add(dataSource);
            }
            if (!workerLoadDB.IsBusy)
            {
                progressBarLoad.Maximum = 100;
                progressBarLoad.Minimum = 0;
                progressBarLoad.Visible = true;
                progressBarLoad.Text = "Load Starting";
                workerLoadDB.RunWorkerAsync();
            }
        }

        void LoadDB_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DBLoadedEventArgs args = new DBLoadedEventArgs();
            if (loadedEvent != null)
            {
                loadedEvent(this, args);
            }
        }
        private List<string> selectedSchemas;
        private DataSource dataSource;
        void LoadDB_DoWork(object sender, DoWorkEventArgs e)
        {
            string conn;
            BaseSchemaReader sr;
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;
            worker.WorkerReportsProgress = true;
            // the language generator here is used to project the primitive variable types
            // this approach has been changed to have the types mapped to a logical model
            // the xslt defines the target type map for the specific language
            LanguageGenerator langGen = LanguageGenerator.CodeGenerator(ProjectDefinition.Settings.CodeLanguage);
            if (ProjectDefinition.Settings.ConnectionString == null)
            {
                // provide a popup to get the connection string
                conn = dataSource.ConnectionString;
                ProjectDefinition.Settings.ConnectionString = conn;
            }
            else
            {
                conn = ProjectDefinition.Settings.ConnectionString;
            }
            sr = new SqlSchemaReader(langGen, conn);
            sr.Filter = _filter;
            sr.WorkerThread = worker;
            ProjectDefinition.Model = new Model();
            ProjectDefinition.Model.OnEntityAdding += new Model.EntityAdding(Model_OnEntityAdding);
            ProjectDefinition.Model.BuildModel(sr);

        }

        void Model_OnEntityAdding(Entity ent)
        {
            if (ProjectDefinition.Settings.PackageIsSchema)
            {
                ent.LogicalPackage = ent.Schema;
            }
            else
            {
                ent.LogicalPackage = ProjectDefinition.Settings.DefaultPackage;
            }
                ent.LogicalModule = ProjectDefinition.Settings.DefaultModule;
        }

        private void frmConnections_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filter.Schemas.Clear();
            foreach (DataRowView itm in listBox1.SelectedItems)
            {
                SchemaReader.SCHEMATARow row = (SchemaReader.SCHEMATARow)itm.Row;
                _filter.Schemas.Add(row.SCHEMA_NAME);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _filter.IncludeViewAsEntities = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            _filter.IncludeTablesAsEntities = checkBox2.Checked;        
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSchemas();

        }

        private void chkFTabs_CheckedChanged(object sender, EventArgs e)
        {
            _filter.IncludeForignTables = checkBox2.Checked; 
        }
    }
    public class DBLoadedEventArgs
    {

    }
}