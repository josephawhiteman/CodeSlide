using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;




namespace CodeGenerator.BL.Modeler
{
    [Serializable]
    public class Model : INodeElement
    {
        public delegate void EntityAdding(Entity ent);
        public event EntityAdding OnEntityAdding;
        [XmlAttribute]
        public int EntityCount
        {
            get
            {
                if (EntityCollection != null)
                {
                    return EntityCollection.Count;
                }
                else
                {
                    return 0;
                }

            }
            set
            {
            }
        }
        private EntityCollection _entityCollection;
        public EntityCollection EntityCollection
        {
            get
            {
                return _entityCollection;
            }
            set
            {
                _entityCollection = value;
            }
        }
        private RelationCollection _relationCollection;
        public RelationCollection Relations
        {
            get
            {
                if (_relationCollection == null)
                {
                    _relationCollection = new RelationCollection();
                }
                return _relationCollection; 
            }
            set { _relationCollection = value; }
        }

        public void BuildModel(CodeGenerator.BL.DBReader.BaseSchemaReader dr)
        {
            EntityCollection = dr.RetrieveEntities(this);
            if (dr.WorkerThread != null && dr.WorkerThread.WorkerReportsProgress)
            {
                dr.WorkerThread.ReportProgress(0);
            }
            int count = 1;
            double prog = 0;
            foreach (Entity ent in EntityCollection)
            {
                if (OnEntityAdding != null)
                {
                    OnEntityAdding(ent);
                }
                dr.PopulateFields(ent);
                dr.PopulateTableConstraints(ent);
                if (dr.WorkerThread != null && dr.WorkerThread.WorkerReportsProgress)
                {
                    prog = (double)count / (double)_entityCollection.Count;
                    dr.WorkerThread.ReportProgress((int)(prog * 100));
                }
                count++;
            }
            if (dr.WorkerThread != null && dr.WorkerThread.WorkerReportsProgress)
            {
                dr.WorkerThread.ReportProgress(0);
            } 

            count = 1;
            foreach (Entity ent in EntityCollection)
            {
                dr.PopulateForeignRelations(ent);
                if (dr.WorkerThread != null && dr.WorkerThread.WorkerReportsProgress)
                {
                    prog = (double)count / (double)_entityCollection.Count;
                    dr.WorkerThread.ReportProgress((int)(prog * 100));
                }
                count++;
            }
        }


        #region INodeElement Members

        public ModelTreeNode GetTreeView(TreeConfig treeConfig)
        {
            List<Schemas> schems = new List<Schemas>();
            ModelTreeNode tn = new ModelTreeNode("Model");
            tn.NodeObject = this;

            foreach (Entity ent in _entityCollection)
            {
                Schemas sch = schems.Find(delegate(Schemas s) { return s.SchemaName == ent.Schema; });
                if (sch == null)
                {
                    sch = new Schemas();
                    sch.SchemaName = ent.Schema;
                    schems.Add(sch);
                }
                sch.Entities.Add(ent);
            }
            ModelTreeNode topNode;
            if (treeConfig.ProjectGrouping == TreeConfig.GroupBy.Schema)
            {
                topNode = new ModelTreeNode("Schemas");
                topNode.NodeObject = EntityCollection;
                tn.Nodes.Add(topNode);
            }
            else
            {

                topNode = new ModelTreeNode("Entities");
                topNode.NodeObject = EntityCollection;
                tn.Nodes.Add(topNode);
            }
            foreach (Schemas ss in schems)
            {
                ModelTreeNode tn1;
                if (treeConfig.ProjectGrouping == TreeConfig.GroupBy.Schema)
                {
                    tn1 = new ModelTreeNode(ss.SchemaName);
                    topNode.Nodes.Add(tn1);
                }
                else
                {
                    tn1 = topNode;
                }
                foreach (Entity ent in ss.Entities)
                {
                    if (treeConfig.ViewMode == TreeConfig.TreeViewMode.Flat
                         || (treeConfig.ViewMode == TreeConfig.TreeViewMode.Heirarchical
                            && (ent.ParentRelations.Count == 0
                               || ent.ParentRelations.Find(delegate(RelationReference r) { return r.Relation.ParentEntity.Entity.LogicalName == ent.LogicalName; }) != null
                            )))
                    {
                        ModelTreeNode node = ent.GetTreeView(treeConfig);
                        if (tn1.Nodes.Count == 0 || tn1.Nodes[0].Text.CompareTo(node.Text) > 0)
                        {
                            tn1.Nodes.Insert(0, node);
                        }
                        else if (tn1.Nodes[tn1.Nodes.Count - 1].Text.CompareTo(node.Text) < 0)
                        {
                            tn1.Nodes.Add(node);
                        }
                        else
                        {
                            // simple bubble sort as nodes are inserted
                            for (int i = 0; i < tn1.Nodes.Count; i++)
                            {
                                if (tn1.Nodes[i].Text.CompareTo(node.Text) > 0)
                                {
                                    tn1.Nodes.Insert(i, node);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return tn;
        }

        public string getNodeName()
        {
            return "Model";
        }

        public void PerformContextAction(NodeElementContextAction actn, ModelTreeNode tn)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        #endregion
    }
}
