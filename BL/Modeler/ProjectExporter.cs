using System;
using System.Collections.Generic;
using System.Text;
using CodeGenerator.BL.DBReader;
using CodeGenerator.BL.Modeler;
using CodeGenerator.BL.Generator;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using CodeGenerator.BL.Support;

namespace CodeGenerator.BL.Modeler
{
    class ProjectExporter
    {
        public ProjectExporter(Configuration settings)
        {
            _Settings = settings;
        }
        private Configuration _Settings;
        public Configuration Settings
        {
            get
            {
                return _Settings;
            }
        }
        private LanguageGenerator _Generator;
        private LanguageGenerator Generator
        {
            get
            {
                if (_Generator == null)
                {
                    _Generator = LanguageGenerator.CodeGenerator(Settings.CodeLanguage);
                }
                return _Generator;
            }

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
        private int entityCount = 0;
        /// <summary>
        /// since the Nodes are processed by entity to drive the creation of multiple files,
        /// one per entity/class the xslt needs the Relations to be embedded in place of the relation references
        /// also there are field references that need to be also embeded
        /// </summary>
        /// <param name="sourceProject"></param>
        public void ExportProject(string sourceProject)
        {
            if (Settings.ExportSingleFile)
            {
                ExportSingle(sourceProject);
            }
            else
            {
                ExportMulti(sourceProject);
            }
        }
        private void ExportMulti(string sourceProject)
        {
            double count = 0;
            double prog = 0;
            FileStream fs = new FileStream(sourceProject, FileMode.Open, FileAccess.Read);
            XmlDocument all = new XmlDocument();
            all.Load(sourceProject);
            XmlNode newNode;
            XmlNode actRef;
            XmlNodeList ents = all.SelectNodes("/Project/Model/EntityCollection/Entity");
            count = ents.Count;
            XmlNodeList AllRels = all.SelectNodes("/Project/Model/Relations/Relation");
            foreach (XmlNode node in ents)
            {
                XmlAttribute useColl = all.CreateAttribute("UseCollections");
                useColl.Value = Settings.UseCollections.ToString();
                node.Attributes.Append(useColl);
                // inject relations
                XmlNodeList children = node.SelectNodes("ChildRelations/RelationReference");
                //XmlNode rels = node.SelectSingleNode("ChildRelations");
                foreach (XmlNode chld in children)
                {
                    XmlAttribute chldRef = chld.Attributes["IDREF"];
                    actRef = all.SelectSingleNode("/Project/Model/Relations/Relation[@ID = '" + chldRef.Value + "']");
                    // replace the reference with the actual
                    XmlNode newRef = actRef.Clone();
                    chld.AppendChild(newRef);

                    //inject entityreferences
                    XmlNode pent = newRef.SelectSingleNode("ParentEntity");
                    XmlAttribute entid = pent.Attributes["IDREF"];
                    actRef = all.SelectSingleNode("/Project/Model/EntityCollection/Entity[@ID = '" + entid.Value + "']");
                    XmlNode newPent = actRef.Clone();
                    pent.AppendChild(newPent);


                    //inject entityreferences
                    XmlNode cent = newRef.SelectSingleNode("ChildEntity");
                    entid = cent.Attributes["IDREF"];
                    actRef = all.SelectSingleNode("/Project/Model/EntityCollection/Entity[@ID = '" + entid.Value + "']");
                    XmlNode newCent = actRef.Clone();
                    cent.AppendChild(newCent);
                }

                XmlNodeList parents = node.SelectNodes("ParentRelations/RelationReference");
                //rels = node.SelectSingleNode("ParentRelations");
                foreach (XmlNode prnt in parents)
                {
                    XmlAttribute prntRef = prnt.Attributes["IDREF"];
                    actRef = all.SelectSingleNode("/Project/Model/Relations/Relation[@ID = '" + prntRef.Value + "']");
                    // replace the reference with the actual
                    XmlNode newRef = actRef.Clone();
                    prnt.AppendChild(newRef);

                    //inject entityreferences
                    XmlNode pent = newRef.SelectSingleNode("ParentEntity");
                    XmlAttribute entid = pent.Attributes["IDREF"];
                    actRef = all.SelectSingleNode("/Project/Model/EntityCollection/Entity[@ID = '" + entid.Value + "']");
                    XmlNode newPent = actRef.Clone();
                    pent.AppendChild(newPent);


                    //inject entityreferences
                    XmlNode cent = newRef.SelectSingleNode("ChildEntity");
                    entid = cent.Attributes["IDREF"];
                    actRef = all.SelectSingleNode("/Project/Model/EntityCollection/Entity[@ID = '" + entid.Value + "']");
                    XmlNode newCent = actRef.Clone();
                    cent.AppendChild(newCent);
                }

                FileStream fsTEST = new FileStream(Settings.ProjectRoot + "\\TEST\\" + node.SelectSingleNode("LogicalName").InnerText + ".xml", FileMode.Create, FileAccess.Write);
                fsTEST.Write(System.Text.Encoding.UTF8.GetBytes(node.OuterXml), 0, node.OuterXml.Length);
                fsTEST.Flush();
                fsTEST.Close();


                ProcessEntity(node.OuterXml);
                count++;
                if (entityCount > 0 && WorkerThread != null && WorkerThread.WorkerReportsProgress)
                {
                    prog = (count / (double)entityCount);
                    if ((int)(prog * 100) <= 100)
                    {
                        WorkerThread.ReportProgress((int)(prog * 100));
                    }
                }
            }
        }

        private void ExportSingle(string sourceProject)
        {
            FileStream clsFile;
            XmlDocument frag = new XmlDocument();
            System.Xml.Xsl.XslCompiledTransform clsX;
            System.Xml.Xsl.XsltSettings settings = new System.Xml.Xsl.XsltSettings();
            clsX = new System.Xml.Xsl.XslCompiledTransform(false);
            clsX.Load(Settings.getEntityTransformPath(), settings, null);
            XmlDocument all = new XmlDocument();
            all.Load(sourceProject);
            string dirPathPackage = Settings.ProjectRoot + Settings.DefaultPackage;
            string dirPathModule = string.Empty;
            XmlNode entities = all.SelectSingleNode("Project/Model");

            FileStream fsTEST = new FileStream(Settings.ProjectRoot + "\\TEST\\AllModule" + ".xml", FileMode.Create, FileAccess.Write);
            fsTEST.Write(System.Text.Encoding.UTF8.GetBytes(entities.OuterXml), 0, entities.OuterXml.Length);
            fsTEST.Flush();
            fsTEST.Close();

            if (!Directory.Exists(dirPathPackage))
            {
                Directory.CreateDirectory(dirPathPackage);
            }
            dirPathModule = dirPathPackage + "\\" + Settings.DefaultModule;
            if (!Directory.Exists(dirPathModule))
            {
                Directory.CreateDirectory(dirPathModule);
            }
            string fileName = dirPathModule + "\\Model." + Generator.CodeFileExtension;
            FileStream clsX1File;
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            clsX1File = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            clsX.Transform(entities, null, clsX1File);
            clsX1File.Flush();
            clsX1File.Close();
        }


        private void ProcessModel(XmlTextReader rdr)
        {
            string entityXML;
            entityXML = rdr.GetAttribute(0);
            if (!int.TryParse(entityXML, out entityCount))
            {
                entityCount = 1;
            }

        }


        private void ProcessSettings(XmlTextReader rdr)
        {
            string entityXML;
            System.Xml.Xsl.XslCompiledTransform clsX;
            System.Xml.Xsl.XsltSettings settings = new System.Xml.Xsl.XsltSettings();
            FileStream clsFile;
            entityXML = rdr.ReadOuterXml();
            XmlDocument frag = new XmlDocument();
            frag.LoadXml(entityXML);
        }
        private void ProcessEntity(XmlTextReader rdr)
        {

            string entityXML = rdr.ReadOuterXml();
            ProcessEntity(entityXML);
        }
        private void ProcessEntity(string entityXML)
        {
            FileStream clsFile;
            XmlDocument frag = new XmlDocument();
            System.Xml.Xsl.XslCompiledTransform clsX;
            System.Xml.Xsl.XsltSettings settings = new System.Xml.Xsl.XsltSettings();
            clsX = new System.Xml.Xsl.XslCompiledTransform(false);
            clsX.Load(Settings.getEntityTransformPath(), settings, null);
            frag.LoadXml(entityXML);
            XmlNode package = frag.SelectSingleNode("/Entity/LogicalPackage");
            XmlNode module = frag.SelectSingleNode("/Entity/LogicalModule");
            XmlNode name = frag.SelectSingleNode("/Entity/LogicalName");
            string dirPathPackage = Settings.ProjectRoot + package.InnerText;
            if (!Directory.Exists(dirPathPackage))
            {
                Directory.CreateDirectory(dirPathPackage);
            }
            string dirPathModule = dirPathPackage + "\\" + module.InnerText;
            if (!Directory.Exists(dirPathModule))
            {
                Directory.CreateDirectory(dirPathModule);
            }
            clsFile = new FileStream(dirPathModule + "\\" + name.InnerText + "." + Generator.CodeFileExtension, FileMode.OpenOrCreate, FileAccess.Write);
            clsX.Transform(frag, null, clsFile);
            clsFile.Flush();
            clsFile.Close();

            XmlParserContext pc = new XmlParserContext(frag.NameTable, new XmlNamespaceManager(frag.NameTable), "", XmlSpace.Default);
            XmlTextReader rdr2 = new XmlTextReader(entityXML, XmlNodeType.Element, pc);
            while (rdr2.Read())
            {
                switch (rdr2.NodeType)
                {
                    case XmlNodeType.Element:
                        if (rdr2.Name == "Relation")
                        {
                            System.Xml.Xsl.XslCompiledTransform clsX1 = new System.Xml.Xsl.XslCompiledTransform(false);
                            if (Settings.UseCollections)
                            {

                                clsX1.Load(Settings.getEntityCollectionTransformPath(), settings, null);
                                string childEntityXML = rdr2.ReadOuterXml();
                                XmlDocument chld = new XmlDocument();
                                chld.LoadXml(childEntityXML);

                                XmlNode chldPackage = chld.SelectSingleNode("/Relation/ChildEntity/Entity/LogicalPackage");
                                XmlNode chldModule = chld.SelectSingleNode("/Relation/ChildEntity/Entity/LogicalModule");
                                XmlNode chldName = chld.SelectSingleNode("/Relation/ChildEntity/Entity/LogicalName");

                                dirPathPackage = Settings.ProjectRoot + chldPackage.InnerText;
                                if (!Directory.Exists(dirPathPackage))
                                {
                                    Directory.CreateDirectory(dirPathPackage);
                                }
                                dirPathModule = dirPathPackage + "\\" + chldModule.InnerText;
                                if (!Directory.Exists(dirPathModule))
                                {
                                    Directory.CreateDirectory(dirPathModule);
                                }
                                dirPathModule = dirPathModule + "\\Collections\\";
                                if (!Directory.Exists(dirPathModule))
                                {
                                    Directory.CreateDirectory(dirPathModule);
                                }
                                string fileName = dirPathModule + chldName.InnerText + "Collection." + Generator.CodeFileExtension;
                                FileStream clsX1File;
                                if (File.Exists(fileName))
                                {
                                    File.Delete(fileName);
                                }
                                clsX1File = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                                clsX1.Transform(chld, null, clsX1File);
                                clsX1File.Flush();
                                clsX1File.Close();
                            }
                        }
                        else
                        {

                        }

                        break;
                }
            }
            rdr2.Close();
        }
    }
}
