using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using Server;

namespace Baza
{
    public class DataBase : IDataBase
    {
        private Dictionary<int, Audit> auditBaza;
        private Dictionary<int, Load> loadBaza;
        private Dictionary<int, ImportedFile> importedBaza;
        private int loadId;
        private int auditId;
        private int importedId;


        public delegate void InMemoryEventHandler(object o); //Delegat za In-Memory bazu
        public static event InMemoryEventHandler InMemoryEvent; // Event za iIn-Memory bazu

        public delegate List<Load> InMemoryEventHandlerRead(); 
        public static event InMemoryEventHandlerRead InMemoryEventRead; 



        public DataBase()
        {
            auditBaza = new Dictionary<int, Audit>();
            loadBaza = new Dictionary<int, Load>();
            importedBaza = new Dictionary<int, ImportedFile>();
            loadId = 0;
            auditId = 0;
            importedId = 0;

        }

        public List<Audit> ReadFromInMemoryAudit()
        {
            try
            {
                return new List<Audit>(auditBaza.Values);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return null;
        }

        public List<ImportedFile> ReadFromInMemoryImportedFile()
        {
            try
            {
                return new List<ImportedFile>(importedBaza.Values);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return null;

        }

        public List<Load> ReadFromInMemoryLoad()
        {
            try
            {
                return new List<Load>(loadBaza.Values);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return null;

        }


        public void WriteToInMemoryLoad(Load load)
        {

            int id;
            try
            {

                if ((id = validacija("load", load)) == -1)
                {
                    loadBaza.Add(loadId++, load);
                }
                else
                {

                    loadBaza[id] = load;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        public void WriteToInMemoryAudit(Audit audit)
        {
            int id;
            try
            {

                if ((id = validacija("audit", audit)) == -1)
                {
                    auditBaza.Add(auditId++, audit);
                }
                else
                {
                    auditBaza[id] = audit;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        public void WriteToInMemoryImportedFile(ImportedFile importedFiles)
        {

            int id;
            try
            {

                if ((id = validacija("imported", importedFiles)) == -1)
                {
                    importedBaza.Add(importedId++, importedFiles);
                }
                else
                {

                    importedBaza[id] = importedFiles;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }


        public int validacija(string tip, object o)
        {

            int i = 0;
            if (tip.Equals("load"))
            {
                Load l = (Load)o;
                foreach (Load l1 in loadBaza.Values)
                {
                    if (l.Id == l1.Id)
                    {
                        return i;
                    }
                    i++;
                }
                return -1;
            }
            else if (tip.Equals("audit"))
            {
                Audit a = (Audit)o;

                foreach (Audit a1 in auditBaza.Values)
                {
                    if (a.Id == a1.Id)
                    {
                        return i;
                    }
                    i++;
                }
                return -1;
            }
            else if (tip.Equals("imported"))
            {
                ImportedFile IF = (ImportedFile)o;
                foreach (ImportedFile IF1 in importedBaza.Values)
                {
                    if (IF.Id == IF1.Id)
                    {
                        return i;
                    }
                    i++;
                }
                return -1;
            }
            else
            {
                return -1;
            }
        }

        public void Upisi(object o)
        {
            Load load = new Load();
            if (o.GetType() == load.GetType())
            {
                load = (Load)o;
                WriteToInMemoryLoad(load);
            }

            Audit audit = new Audit();
            if (o.GetType() == audit.GetType())
            {
                audit = (Audit)o;
                WriteToInMemoryAudit(audit);
            }

            ImportedFile file = new ImportedFile();
            if (o.GetType() == file.GetType())
            {
                file = (ImportedFile)o;
                WriteToInMemoryImportedFile(file);
            }
        }
        public List<Load> Citaj()
        {
            return ReadFromInMemoryLoad();

        }
        public void RegisterEventHandler()
        {
            InMemoryEvent += Upisi;
            InMemoryEventRead += Citaj;

        }

        public void TriggerEvent(object o)
        {
            InMemoryEvent(o);
        }


        
        public List<Load> TriggerEventRead()
        {
            return InMemoryEventRead();
        }




        public void RemoveEventHandler()
        {
            InMemoryEvent -= Upisi;
            InMemoryEventRead -= Citaj;

        }


    }
}
