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
    class ModelExporter
    {
        public ModelExporter(Settings settings)
        {
            _Settings = settings;
        }
        private Settings _Settings;
        public Settings Settings
        {
            get
            {
                return _Settings;
            }
        }
        public void ExportDefinition(Project project)
        {
            TextWriter resultsWriter = new StringWriter();
            XmlTextWriter xslResultsWriter = new XmlTextWriter(resultsWriter);
            MemoryStream outStrm = new MemoryStream();

            XmlSerializer xs = new XmlSerializer(typeof(Project));
            TextWriter stringWriter = new StringWriter();
            XmlTextWriter xtr = new XmlTextWriter(stringWriter);



            string xmlnamespace = _Settings.XmlNameSpace;
            XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
            xns.Add("tt", xmlnamespace);
            try
            {
                //xs.Serialize(xtr, model, xns);
                xs.Serialize(outStrm, project, xns);
            }
            catch (Exception ee)
            {
                string m = ee.Message;
            }
            outStrm.Position = 0;


            FileStream fs = new FileStream(Settings.FileName, FileMode.OpenOrCreate, FileAccess.Write);
            outStrm.WriteTo(fs);
            fs.Flush();
            fs.Close();
            //string str = stringWriter.ToString();
            outStrm.Position = 0;
        }
    }
}
