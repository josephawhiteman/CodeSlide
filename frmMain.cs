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
using CodeGenerator.Controls;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace CodeGenerator
{
    public partial class frmMain : Form
    {
        Forms.frmConnections loadForm = null;

        delegate void LoadTreeCallBack();
        delegate void ImportDbCallBack();
        public frmMain()
        {
            InitializeComponent();
            _TreeContextMenuStrip = new TreeNodeContextMenuStrip(tvModel);
            _TreeContextMenuStrip.buildToolStripItems();
            _TreeContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(_TreeContextMenuStrip_ItemClicked);
            loadForm = new CodeGenerator.Forms.frmConnections(ProjectDefinition);
            loadForm.loadedEvent += new CodeGenerator.Forms.frmConnections.DBLoadedEventHandler(loadForm_loadedEvent);
        }

        void _TreeContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private TreeNodeContextMenuStrip _TreeContextMenuStrip;

        internal TreeNodeContextMenuStrip TreeContextMenuStrip
        {
            get { return _TreeContextMenuStrip; }
            set { _TreeContextMenuStrip = value; }
        }

        private Settings _settings;
        private Settings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new Settings();
                }
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }
        private TreeConfig _TreeConfiguration;

        public TreeConfig TreeConfiguration
        {
            get
            {
                if (_TreeConfiguration == null)
                {
                    _TreeConfiguration = new TreeConfig();
                    _TreeConfiguration.Tree = tvModel;
                }
                return _TreeConfiguration;
            }
            set { _TreeConfiguration = value; }
        }

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

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "Load the model definition|*.xml";
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Settings.FileName = openFileDialog1.FileName;
            if (Settings.FileName != "")
            {
                workerLoadDB.DoWork += new DoWorkEventHandler(LoadProject_DoWork);
                workerLoadDB.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadProject_RunWorkerCompleted);
                workerLoadDB.RunWorkerAsync();
            }
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeGenerator.Forms.frmSettings SettingsDialog = new CodeGenerator.Forms.frmSettings();

            // Show the dialog and determine the state of the 
            // DialogResult property for the form.
            if (SettingsDialog.ShowDialog() == DialogResult.OK)
            {
                // Do something here to handle data from dialog box.
            }
        }

        private void btnDoGenerate_Click(object sender, EventArgs e)
        {

            workerLoadDB.DoWork += new DoWorkEventHandler(DoGenerate_DoWork);
            workerLoadDB.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DoGenerate_RunWorkerCompleted);
            workerLoadDB.ProgressChanged += new ProgressChangedEventHandler(DoGenerate_ProgressChanged);
            if (!workerLoadDB.IsBusy)
            {
                toolStripProgressBar1.ProgressBar.Maximum = 100;
                toolStripProgressBar1.ProgressBar.Minimum = 0;
                toolStripProgressBar1.Visible = true;
                toolStripStatusLabel1.Text = "Generation Starting";
                workerLoadDB.RunWorkerAsync();
            }

        }

        void DoGenerate_DoWork(object sender, DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;
            worker.WorkerReportsProgress = true;
            ProjectExporter exporter = new ProjectExporter(ProjectDefinition.Settings);
            exporter.WorkerThread = worker;
            exporter.ExportProject(Settings.FileName);
        }

        void DoGenerate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.ProgressBar.Value = e.ProgressPercentage;
            toolStripStatusLabel1.Text = "Generation Completion: " + e.ProgressPercentage + "%";

        }
        void DoGenerate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel1.Text = "Generation Finished";
            workerLoadDB.DoWork -= new DoWorkEventHandler(DoGenerate_DoWork);
            workerLoadDB.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(DoGenerate_RunWorkerCompleted);
            workerLoadDB.ProgressChanged -= new ProgressChangedEventHandler(DoGenerate_ProgressChanged);
        }



        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (loadForm.InvokeRequired)
            {
                loadForm.Invoke(new ImportDbCallBack(doLoadDB));
            }
            else
            {
                loadForm.ShowDialog();
            }
        }
        public void doLoadDB()
        {
            loadForm.ShowDialog();
        }

        void loadForm_loadedEvent(object sender, CodeGenerator.Forms.DBLoadedEventArgs args)
        {
            loadForm.Hide();
            ModelExporter exporter = new ModelExporter(Settings);
            toolStripStatusLabel1.Text = "Model Loaded";
            tvModel.Nodes.Add(ProjectDefinition.GetTreeView(TreeConfiguration));
            exporter.ExportDefinition(ProjectDefinition);
            toolStripProgressBar1.Visible = false;
        }


        void LoadProject_DoWork(object sender, DoWorkEventArgs e)
        {
            // validate file here
            // but for now  read it.
            ProjectDefinition = Project.ReloadProject(Settings.FileName);
            if (tvModel.InvokeRequired)
            {
                LoadTreeCallBack treeLoader = new LoadTreeCallBack(LoadTree);
                this.Invoke(treeLoader);
            }
            else
            {
                LoadTree();
            }
        }

        public void LoadTree()
        {
            tvModel.Nodes.Clear();
            ModelTreeNode tn = ProjectDefinition.GetTreeView(TreeConfiguration);
            tvModel.Nodes.Add(tn);
        }

        void LoadProject_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }


        private void tvModel_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ModelTreeNode tn;
            if (e.Node is ModelTreeNode)
            {
                tn = (ModelTreeNode)e.Node;
                if (tn.NodeObject != null)
                {
                    INodeElement ne = tn.NodeObject;
                    loadNodeEditor(ne);
                }
            }
        }

        private void loadNodeEditor(INodeElement ne)
        {
            pnlNodeEditor.Controls.Clear();
            NodeEditor edit;
            edit = new NodeEditor(ne);
            pnlNodeEditor.Controls.Add(edit);

        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelExporter exporter = new ModelExporter(Settings);
            exporter.ExportDefinition(ProjectDefinition);

        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void treeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TreeConfiguration.ViewMode == TreeConfig.TreeViewMode.Flat)
            {
                TreeConfiguration.ViewMode = TreeConfig.TreeViewMode.Heirarchical;
                treeViewToolStripMenuItem.Text = "Tree View (Flat)";
            }
            else
            {
                TreeConfiguration.ViewMode = TreeConfig.TreeViewMode.Flat;
                treeViewToolStripMenuItem.Text = "Tree View (Heirarchical)";
            }
            tvModel.Nodes.Clear();
            tvModel.Nodes.Add(ProjectDefinition.Model.GetTreeView(TreeConfiguration));


        }

        private void tvModel_Click(object sender, EventArgs e)
        {
            MouseEventArgs args = (MouseEventArgs)(e);
            if (args.Button == MouseButtons.Right)
            {
                ModelTreeNode tn = (ModelTreeNode)tvModel.GetNodeAt(new Point(args.X, args.Y));
                if (tn != null)
                {
                    TreeContextMenuStrip.Text = tn.Text;
                    TreeContextMenuStrip.CurrentTreeNode = tn;
                    TreeContextMenuStrip.Show((TreeView)sender, args.X, args.Y);
                }
            }
            else if (args.Button == MouseButtons.Left)
            {

            }
        }

        private void tvModel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void tvModel_DragLeave(object sender, EventArgs e)
        {
            TreeView tv = (TreeView)sender;

        }

        TreeNode OldNode;
        TreeNode OriginalNode;

        private void tvModel_DragOver(object sender, DragEventArgs e)
        {

            TreeView tv = (TreeView)sender;
            System.Windows.Forms.IDataObject o = e.Data;
            TreeNode aNode = tvModel.GetNodeAt(new Point(e.X - this.Top - tvModel.Top - tvModel.Parent.Top, e.Y - this.Left - tvModel.Left - tvModel.Parent.Left));
            this.toolStripStatusLabel1.Text = aNode.Text;
            if (aNode != null)
            {
                aNode.BackColor = Color.DarkBlue;
                aNode.ForeColor = Color.White;
            }
            if ((OldNode != null) && (OldNode != aNode))
            {
                OldNode.BackColor = OriginalNode.BackColor;
                OldNode.ForeColor = OriginalNode.ForeColor;
            }
            OldNode = aNode;
            e.Effect = DragDropEffects.Move;
        }

        private void tvModel_DragDrop(object sender, DragEventArgs e)
        {

            e.Effect = DragDropEffects.Move;

        }

        private void tvModel_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeView tv = (TreeView)sender;
            ModelTreeNode aNode = (ModelTreeNode)e.Item;
            this.toolStripStatusLabel1.Text = "";
            tv.DoDragDrop(aNode, DragDropEffects.Move);
            OriginalNode = aNode;
        }

        private void tvModel_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            TreeView tv = (TreeView)sender;
            e.UseDefaultCursors = false;
            if ((e.Effect & DragDropEffects.Move) == DragDropEffects.Move)
                Cursor.Current = Cursors.Hand;
            else
                Cursor.Current = Cursors.Cross;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            saveFileDialog1.FileName = Settings.FileName;
            saveFileDialog1.Filter = "Project files (*.xml)|*.xml|All files (*.*)|*.";
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Settings.FileName = saveFileDialog1.FileName;
            ModelExporter exporter = new ModelExporter(Settings);
            exporter.ExportDefinition(ProjectDefinition);
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (pnlNodeEditor.Controls.Count > 0)
            {
                pnlNodeEditor.Controls[0].Width = (this.Width - 10) / 2;
                tvModel.Width = (this.Width - 10) / 2;
            }
        }

        private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetProject();
        }
        private void resetProject()
        {
            tvModel.Nodes.Clear();
            _settings = null;
            _TreeConfiguration = null;  
        }

        private void tsmiGroupBy_Schemas_Click(object sender, EventArgs e)
        {
            TreeConfiguration.ProjectGrouping = TreeConfig.GroupBy.Schema;
            ToolStripMenuItem itm = (ToolStripMenuItem)sender;
            doGroupBy((ToolStripMenuItem)itm.OwnerItem);
            itm.Checked = true;
        }

        private void tsmiGroupBy_Entities_Click(object sender, EventArgs e)
        {
            TreeConfiguration.ProjectGrouping = TreeConfig.GroupBy.Entity;
            ToolStripMenuItem itm = (ToolStripMenuItem)sender;
            doGroupBy((ToolStripMenuItem)itm.OwnerItem);
            itm.Checked = true;
        }
        private void doGroupBy(ToolStripMenuItem top)
        {
            tvModel.Nodes.Clear();
            LoadTree();
            foreach (ToolStripMenuItem tsi in top.DropDownItems)
            {
                tsi.Checked = false;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView vw = listView1;

        }
    }

 //   private class codex : System.Xml.Serialization.CodeExporter
 //   {

 //   }
}