using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CodeGenerator.BL.Modeler
{
    [Serializable]
    public class Project : INodeElement
    {

        public Project()
        {
        }
        private Model _model;
        public Model Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
            }
        }
        private Configuration _Settings;
        public Configuration Settings
        {
            get
            {
                if (_Settings == null)
                {
                    _Settings = new Configuration();
                }
                return _Settings;
            }
            set
            {
                _Settings = value;
            }
        }
        public static Project ReloadProject(string fileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Project));
            System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
            System.Xml.XmlTextReader xr = new System.Xml.XmlTextReader(sr);
            Project obj = (Project)xs.Deserialize(xr);
            foreach (Entity ent in obj.Model.EntityCollection)
            {
                ent.Model = obj.Model;
                foreach(RelationReference cRel in ent.ChildRelations)
                {
                    cRel.Relation = obj.Model.Relations.Find(delegate(Relation r) { return r.ID == cRel.ID; });
                    if (cRel.Relation == null)
                    {
                    }
                    else
                    {
                        cRel.Relation.ParentEntity.Entity = ent;
                        cRel.Relation.ChildEntity.Entity = obj.Model.EntityCollection.Find(delegate(Entity e) { return e.EntityID == cRel.Relation.ParentEntity.EntityID; });
                        foreach (FieldReference fRef in cRel.Relation.ForeignFields)
                        {
                            fRef.RelatedField = cRel.Relation.ParentEntity.Entity.Fields.Find(delegate(Field f) { return f.ID == fRef.ID; });
                        }
                        foreach (FieldReference pRef in cRel.Relation.RelatedFields)
                        {
                            pRef.RelatedField = cRel.Relation.ChildEntity.Entity.Fields.Find(delegate(Field f) { return f.ID == pRef.ID; });
                        }
                    }
                }
                foreach (RelationReference pRel in ent.ParentRelations)
                {
                    pRel.Relation = obj.Model.Relations.Find(delegate(Relation r) { return r.ID == pRel.ID; });
                    if (pRel.Relation == null)
                    {
                    }
                    else
                    {
                        pRel.Relation.ParentEntity.Entity = obj.Model.EntityCollection.Find(delegate(Entity e) { return e.EntityID == pRel.Relation.ParentEntity.EntityID; });
                        pRel.Relation.ChildEntity.Entity = ent;
                        foreach (FieldReference pRef in pRel.Relation.ForeignFields)
                        {
                            pRef.RelatedField = pRel.Relation.ChildEntity.Entity.Fields.Find(delegate(Field f) { return f.ID == pRef.ID; });
                        }
                        foreach (FieldReference pRef in pRel.Relation.RelatedFields)
                        {
                            pRef.RelatedField = pRel.Relation.ParentEntity.Entity.Fields.Find(delegate(Field f) { return f.ID == pRef.ID; });
                        }
                    }
                }
            }
            return obj;
        }

        #region INodeElement Members

        public string getNodeName()
        {
            return "Project";
        }

        public ModelTreeNode GetTreeView(TreeConfig treeConfig)
        {
            ModelTreeNode n = new ModelTreeNode("Project");
            //n.NodeObject = this;
            n.Nodes.Add(Model.GetTreeView(treeConfig));
            ModelTreeNode setns = Settings.GetTreeView(treeConfig);
            n.Nodes.Add(setns);

            return n;
        }

        public void PerformContextAction(NodeElementContextAction actn, ModelTreeNode tn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
