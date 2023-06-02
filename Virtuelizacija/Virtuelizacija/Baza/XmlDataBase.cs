using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Baza
{
    public class XmlDataBase : IXmlDataBase
    {
        static readonly XmlDocument xmlDocAudit = new XmlDocument();
        XmlElement rootElementAudit = xmlDocAudit.CreateElement("Audits");

        static readonly XmlDocument xmlDocImported = new XmlDocument();
        XmlElement rootElementImported = xmlDocImported.CreateElement("Importeds");

        static readonly XmlDocument xmlDoc = new XmlDocument();
        XmlElement rootElement = xmlDoc.CreateElement("Loads");



        public delegate void EventHandlerXML(object o); //Delegat za In-Memory bazu
        public static event EventHandlerXML XMLEvent;


        public delegate XmlDocument EventHandlerXMLRead(); //Delegat za In-Memory bazu
        public static event EventHandlerXMLRead XMLEventRead;




        public void UpisXml(Load l)
        {
            xmlDoc.AppendChild(rootElement);
            AddLoadObject(xmlDoc, rootElement, l);

            xmlDoc.Save("TBL_LOAD.xml");

            //Console.WriteLine("Load objects added to XML successfully.");
        }

        static void AddLoadObject(XmlDocument xmlDoc, XmlElement parentElement, Load l)
        {
            XmlElement loadElement = xmlDoc.CreateElement("Load");

            AppendChildElement(xmlDoc, loadElement, "Id", l.Id.ToString());
            AppendChildElement(xmlDoc, loadElement, "Timestamp", l.Timestamp);
            AppendChildElement(xmlDoc, loadElement, "ForecastValue", l.ForecastValue.ToString());
            AppendChildElement(xmlDoc, loadElement, "MeasuredValue1", l.MeasuredtValue1.ToString());
            AppendChildElement(xmlDoc, loadElement, "AbsolutePercentageDeviation", l.AbsolutePercentageDeviation.ToString());
            AppendChildElement(xmlDoc, loadElement, "SquaredDeviation", l.SquaredDeviation.ToString());
            AppendChildElement(xmlDoc, loadElement, "ForecastFileId", l.ForecastFileId.ToString());
            AppendChildElement(xmlDoc, loadElement, "MeasuredFileId", l.MeasuredFileId.ToString());

            parentElement.AppendChild(loadElement);
        }

        static void AppendChildElement(XmlDocument xmlDoc, XmlElement parentElement, string elementName, string elementValue)
        {
            XmlElement childElement = xmlDoc.CreateElement(elementName);
            childElement.InnerText = elementValue;
            parentElement.AppendChild(childElement);
        }



        public void UpisXmlAudit(Audit a)
        {
            xmlDocAudit.AppendChild(rootElementAudit);
            AddAuditObject(rootElementAudit, a);

            xmlDocAudit.Save("TBL_AUDIT.xml");

            //Console.WriteLine("Audit objects added to XML successfully.");
        }

        static void AddAuditObject(XmlElement parentElement, Audit a)
        {
            XmlElement auditElement = xmlDocAudit.CreateElement("Audit");

            AppendChildElementAudit(auditElement, "Id", a.Id.ToString());
            AppendChildElementAudit(auditElement, "Timestamp", a.Timestamp);
            AppendChildElementAudit(auditElement, "Message", a.Message);
            AppendChildElementAudit(auditElement, "MessageType", a.MessageType.ToString());

            parentElement.AppendChild(auditElement);
        }

        static void AppendChildElementAudit(XmlElement parentElement, string elementName, string elementValue)
        {
            XmlElement childElement = xmlDocAudit.CreateElement(elementName);
            childElement.InnerText = elementValue;
            parentElement.AppendChild(childElement);
        }



        public void UpisXmlImported(ImportedFile If)
        {
            xmlDocImported.AppendChild(rootElementImported);
            AddImportedObject(rootElementImported, If);

            xmlDocImported.Save("TBL_IMPORTED.xml");

            //Console.WriteLine("Imported objects added to XML successfully.");
        }

        static void AddImportedObject(XmlElement parentElement, ImportedFile If)
        {
            XmlElement importedElement = xmlDocImported.CreateElement("Imported");

            AppendChildElementImported(importedElement, "Id", If.Id.ToString());
            AppendChildElementImported(importedElement, "FileName", If.FileName);

            parentElement.AppendChild(importedElement);
        }

        static void AppendChildElementImported(XmlElement parentElement, string elementName, string elementValue)
        {
            XmlElement childElement = xmlDocImported.CreateElement(elementName);
            childElement.InnerText = elementValue;
            parentElement.AppendChild(childElement);
        }


        public void IzbrisiXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("E:\\Downloads\\ProjekatVirtuelizcija_2\\ProjekatVirtuelizcija (2)\\ProjekatVirtuelizcija\\Virtuelizacija\\Virtuelizacija\\Server\\bin\\Debug\\TBL_AUDIT.xml");

            XmlElement rootElement = xmlDoc.DocumentElement;

            while (rootElement.HasChildNodes)
            {
                rootElement.RemoveChild(rootElement.FirstChild);
            }

            xmlDoc.Save("E:\\Downloads\\ProjekatVirtuelizcija_2\\ProjekatVirtuelizcija (2)\\ProjekatVirtuelizcija\\Virtuelizacija\\Virtuelizacija\\Server\\bin\\Debug\\TBL_AUDIT.xml");

            Console.WriteLine("Objects removed from XML successfully.");
        }


        public void Upisi(object o)
        {
            Load load = new Load();
            if (o.GetType() == load.GetType())
            {
                load = (Load)o;
                UpisXml(load);
            }

            Audit audit = new Audit();
            if (o.GetType() == audit.GetType())
            {
                audit = (Audit)o;
                UpisXmlAudit(audit);
            }

            ImportedFile file = new ImportedFile();
            if (o.GetType() == file.GetType())
            {
                file = (ImportedFile)o;
                UpisXmlImported(file);
            }
        }
        public XmlDocument Citaj()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("TBL_LOAD.xml");
            return xmlDoc;

        }
        public void RegisterEventHandler()
        {
            XMLEvent += Upisi;
            XMLEventRead += Citaj;

        }

        public void TriggerEvent(object o)
        {
            XMLEvent(o);
        }
     


        public XmlDocument TriggerEventRead()
        {
            return XMLEventRead();
        }




        public void RemoveEventHandler()
        {
            XMLEvent -= Upisi;
            XMLEventRead -= Citaj;

        }


        public List<Load> LoadVrati()
        { XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("TBL_LOAD.xml");
            XmlNodeList objectNodes = xmlDoc.GetElementsByTagName("Load");
            List<Load> list = new List<Load>();
            foreach (XmlNode objectNode in objectNodes)
            {
                XmlNode Id = objectNode.SelectSingleNode("Id");
                XmlNode timeStamp = objectNode.SelectSingleNode("Timestamp");
                XmlNode measuredValue = objectNode.SelectSingleNode("MeasuredValue1");
                XmlNode forcastValue = objectNode.SelectSingleNode("ForecastValue");
                XmlNode AbsolutePercentageDeviation = objectNode.SelectSingleNode("AbsolutePercentageDeviation");
                XmlNode SquaredDeviation = objectNode.SelectSingleNode("SquaredDeviation");
                XmlNode fileFor = objectNode.SelectSingleNode("ForecastFileId");
                XmlNode fileMes = objectNode.SelectSingleNode("MeasuredFileId");

                Load l=new Load();

                if(Id!=null)
                {
                    l.Id=Int32.Parse(Id.InnerText);
                }
                if(timeStamp!=null)
                {
                    l.Timestamp = timeStamp.InnerText.ToString();
                }


                if(measuredValue!=null)
                {
                    l.MeasuredtValue1 = double.Parse(measuredValue.InnerText);

                }

                if(forcastValue!=null)
                {
                    l.ForecastValue=double.Parse(forcastValue.InnerText);
                }

                if(AbsolutePercentageDeviation!=null)
                {
                    l.AbsolutePercentageDeviation=double.Parse(AbsolutePercentageDeviation.InnerText);
                }


                if(SquaredDeviation!=null)
                {
                    l.SquaredDeviation=double.Parse(SquaredDeviation.InnerText);
                }

                if(fileFor!=null)
                {
                    l.ForecastFileId=int.Parse(fileFor.InnerText);
                }

                if(fileMes!=null)
                {
                    l.MeasuredFileId=int.Parse(fileMes.InnerText);
                }

                list.Add(l);

            }

            return list;
        }



        public List<Audit> AuditVrati()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("TBL_AUDIT.xml");
            XmlNodeList objectNodes = xmlDoc.GetElementsByTagName("Audit");
            List<Audit> list = new List<Audit>();
            foreach (XmlNode objectNode in objectNodes)
            {
                XmlNode Id = objectNode.SelectSingleNode("Id");
                XmlNode timeStamp = objectNode.SelectSingleNode("Timestamp");
                XmlNode message=objectNode.SelectSingleNode("Message");
                XmlNode messageType = objectNode.SelectSingleNode("MessageType");

                Audit a = new Audit();

                if (Id != null)
                {
                    a.Id = Int32.Parse(Id.InnerText);
                }
                if (timeStamp != null)
                {
                    a.Timestamp = timeStamp.InnerText.ToString();
                }
                if(message != null)
                {
                    a.Message = message.InnerText.ToString();
                }
                if(messageType != null)
                {
                    if (Enum.TryParse(messageType.ToString(), out TypeMessage messageTypeout))
                    {
                        a.MessageType = messageTypeout; // Assign the parsed enum value to the property
                    }
                }


               
                list.Add(a);

            }

            return list;
        }


        public List<ImportedFile> ImportedVrati()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("TBL_IMPORTED.xml");
            XmlNodeList objectNodes = xmlDoc.GetElementsByTagName("Imported");
            List<ImportedFile> list = new List<ImportedFile>();
            foreach (XmlNode objectNode in objectNodes)
            {
                XmlNode Id = objectNode.SelectSingleNode("Id");
                XmlNode fileName = objectNode.SelectSingleNode("FileName");
               

                ImportedFile a = new ImportedFile();

                if (Id != null)
                {
                    a.Id = Int32.Parse(Id.InnerText);
                }
               if(fileName!=null)
                {
                    a.FileName = fileName.InnerText.ToString();
                }



                list.Add(a);

            }

            return list;
        }



    }
}
