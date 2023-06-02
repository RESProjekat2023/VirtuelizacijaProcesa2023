using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Configuration;
using Baza;
using System.Xml.Linq;
using System.Xml;
using System.ServiceModel;

namespace Server
{
    public class FileHandler : IFileHandler
    {
        Dictionary<string, string> dictionaryForecast = new Dictionary<string, string>();
        Dictionary<string, string> dictionaryMeasured = new Dictionary<string, string>();
        private readonly string databaseType = ConfigurationManager.AppSettings["DatabaseType"];
        public static readonly string formulaType = ConfigurationManager.AppSettings["Formula"];

        DataBase db = new DataBase();
        XmlDataBase xmlDataBase = new XmlDataBase();

        int countLoad = 1;
        int countAudit = 1;
        int countImported = 1;

        [OperationBehavior(AutoDisposeParameters=true)]
        public void UploadFiles(List<FileCSV> fileStreamMeasured, List<FileCSV> fileStreamForecast)
        {
            ChooseMemoryType();

            LoadDictionaryMeasured(fileStreamMeasured);
            LoadDictionaryForcast(fileStreamForecast);

            DisposeStreams(fileStreamMeasured, fileStreamForecast);
            ReadAndCreateObjects();
            CalculateDeviation();
            PrintMemoryBaseOnConsole();
            RemoveEventHandler();
            

        }

        private void RemoveEventHandler()
        {
            if (databaseType == "InMemory")
            {
                db.RemoveEventHandler();
            }
            else if(databaseType == "XML")
            {
                xmlDataBase.RemoveEventHandler();
            }
        }

        private void PrintMemoryBaseOnConsole()
        {
            if (databaseType == "InMemory")
            {
                List<Load> l1 = db.ReadFromInMemoryLoad();
                List<Audit> l2 = db.ReadFromInMemoryAudit();
                List<ImportedFile> l3 = db.ReadFromInMemoryImportedFile();

                Console.WriteLine("Loads:");
                foreach (Load l in l1)
                {
                    Console.WriteLine(l);
                }
                Console.WriteLine("Audits:");
                foreach (Audit l in l2)
                {
                    Console.WriteLine(l);
                }
                Console.WriteLine("ImportedFiles:");
                foreach (ImportedFile l in l3)
                {
                    Console.WriteLine(l);
                }
            }
        }

        private void CalculateDeviation()
        {
            if (databaseType == "InMemory")
            {
                List<Load> list = db.TriggerEventRead();
                foreach (Load l in list)
                {
                    if (formulaType == "Percentage")
                    {
                        l.AbsolutePercentageDeviation = (Math.Abs(l.MeasuredtValue1 - l.ForecastValue) / l.MeasuredtValue1 * 100);
                    }
                    else if(formulaType == "Squared")
                    {
                        l.SquaredDeviation = Math.Pow((l.MeasuredtValue1 - l.ForecastValue) / l.MeasuredtValue1, 2);
                    }


                    db.TriggerEvent(l);
                }
            }
            else
            {
                XmlDocument document = xmlDataBase.TriggerEventRead();
                Deviation.SearchXmlFile("TBL_LOAD.xml", document);
            }
        }

        private void ChooseMemoryType()
        {
            if (databaseType == "InMemory")
            {
                db.RegisterEventHandler();
            }
            else if(databaseType=="XML")
            {

                xmlDataBase.RegisterEventHandler();
            }
        }

        private void LoadDictionaryForcast(List<FileCSV> fileStreamForecast)
        {
            foreach (FileCSV c in fileStreamForecast)
            {
                byte[] data = c.FileStream.ToArray();
                byte[] byteArray = data;

                string result = Encoding.UTF8.GetString(byteArray);
                Console.WriteLine(result);

                string p = (c.FileName.Split(new string[] { "forecast" }, StringSplitOptions.None))[1];
                string key = p.Replace(".csv", "");


                dictionaryForecast.Add(key, result);

            }
        }

        private void LoadDictionaryMeasured(List<FileCSV> fileStreamMeasured)
        {
            foreach (FileCSV c in fileStreamMeasured)
            {
                byte[] data = c.FileStream.ToArray();
                byte[] byteArray = data;

                string result = Encoding.UTF8.GetString(byteArray);
                Console.WriteLine(result);

                string p = (c.FileName.Split(new string[] { "measured" }, StringSplitOptions.None))[1];
                string key = p.Replace(".csv", "");     //_2023_23_02

                dictionaryMeasured.Add(key, result);
            }
        }

        public void ReadAndCreateObjects()
        {
            

            foreach (KeyValuePair<string, string> s in dictionaryMeasured)
            {

                if (dictionaryForecast.ContainsKey(s.Key))
                {
                    Dictionary<string, double> dict1 = new Dictionary<string, double>();      //measured
                    Dictionary<string, double> dict2 = new Dictionary<string, double>();      //forecast
                    bool indikator = false;

                    string[] lines = s.Value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (var line in lines) {
                        //Console.WriteLine(line);
                        if (line.Contains("?TIME_STAMP,MEASURED_VALUE") || line.Contains("TIME_STAMP,MEASURED_VALUE") || string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }
                        string[] values = line.Split(',');

                        if (!char.IsDigit(values[0][0]))
                        {
                            values[0] = values[0].Remove(0, 1);
                        }

                        if (dict1.ContainsKey(values[0]) && Validation(values, s.Key))
                        {
                            dict1[values[0]] = Double.Parse(values[1]);

                            //Izmena(values[0], Double.Parse(values[1]));
                            Console.WriteLine("Zamijenili smo vec postojecu vrijednost " + values[1]);
                            continue;
                        }
                        else if (Validation(values, s.Key))
                            dict1.Add(values[0], Double.Parse(values[1]));
                    }
                    
                    if (dict1.Count != 24)
                        indikator = true;

                    string[] linesForecast = dictionaryForecast[s.Key].Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (var line in linesForecast)
                    {
                        if (line.Contains("?TIME_STAMP,FORECAST_VALUE") || line.Contains("TIME_STAMP,FORECAST_VALUE") ||  string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }
                        string[] values = line.Split(',');

                        if (!char.IsDigit(values[0][0]))
                        {
                            values[0] = values[0].Remove(0, 1);
                        }

                        if (dict2.ContainsKey(values[0]) && Validation(values, s.Key))
                        {
                            dict2[values[0]] = Double.Parse(values[1]);

                            //Izmena(values[0], Double.Parse(values[1]));
                            Console.WriteLine("Zamijenili smo vec postojecu vrijednost " + values[1]);
                            continue;
                        }
                        else if (Validation(values, s.Key))
                            dict2.Add(values[0], Double.Parse(values[1]));
                    }

                    if (dict2.Count != 24)
                        indikator = true;


                    CreateImportedObject(s.Key);

                    if (indikator == true)
                    {
                        CreateAuditErrorObject();
                    }
                    else
                    {
                        CreateAuditInfoObject(s);
                        foreach (KeyValuePair<string, double> kp in dict1)
                        {
                            CreateLoadObject(s, dict2, kp);
                        }
                    }
                }
                else {
                    Console.WriteLine("Nemamo odgovarajuci par forecast measure. "+"("+ s.Key +")");
                }
            }
        }

        private void CreateLoadObject(KeyValuePair<string, string> s, Dictionary<string, double> dict2, KeyValuePair<string, double> kp)
        {
            Load l = new Load();

            l = CreateLoad(kp, s, dict2);

            if (databaseType == "InMemory")
                db.TriggerEvent(l);
            else if(databaseType == "XML")
                xmlDataBase.TriggerEvent(l);
        }

        private void CreateAuditInfoObject(KeyValuePair<string, string> s)
        {
            Audit a = new Audit();
            a = CreateAuditInfo(s);
            if (databaseType == "InMemory")
                db.TriggerEvent(a);
            else if(databaseType == "XML")
                xmlDataBase.TriggerEvent(a);
        }

        private void CreateAuditErrorObject()
        {
            Audit a = new Audit();
            a = CreateAuditError();
            if (databaseType == "InMemory")
                db.TriggerEvent(a);
            else if(databaseType=="XML")
                xmlDataBase.TriggerEvent(a);
        }

        private void CreateImportedObject(string s)
        {
            if (databaseType == "InMemory")
            {
                db.TriggerEvent(new ImportedFile(countImported, "measured" + s));
                countImported++;
                db.TriggerEvent(new ImportedFile(countImported, "forecast" + s));
                countImported++;
            }
            else if(databaseType=="XML")
            {

                xmlDataBase.TriggerEvent(new ImportedFile(countImported, "measured" + s));
                countImported++;
                xmlDataBase.TriggerEvent(new ImportedFile(countImported, "forecast" + s));
                countImported++;
            }
        }

        private bool Validation(string[] values, string DateAndTime)
        {
            string key;
            string value1 = values[0];
            string value2 = values[1];

            if (!char.IsDigit(value1[0]))
            {
                  value1=value1.Remove(0, 1);
            }

            //Console.WriteLine(value1);

            string dateValue = (value1.Split(' '))[0];             //provjera naziv datoteka-datum u csv
            try
            {
                key = "_"+(dateValue.Replace("-", "_"));
                //key = int.Parse(dateValue.Replace("-", ""));           //2023-01-19 00:00

                string timeValue = (value1.Split(' '))[1];
                int time = int.Parse(timeValue.Split(':')[0]);
                if (time < 0 || time > 24)
                {
                    Console.WriteLine("Negativno ili preko 24 ");
                    Audit a = new Audit();
                    a =CreateAuditWarning(values);
                    if (databaseType == "InMemory")
                    {
                        db.TriggerEvent(a);
                    }
                    else
                    {
                        xmlDataBase.TriggerEvent(a);
                    }
                    return false;
                }
            }
            catch (Exception e) {
                return false;
            }
            if(DateAndTime != key)
                return false;


            try
            {
                double value = double.Parse(value2);         //3242.32225
                if (value < 0 || value > 10000) {
                    return false;
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }


        private Load CreateLoad(KeyValuePair<string, double> kp, KeyValuePair<string, string> s, Dictionary<string, double> dict2)
        {

            Load load = new Load()
            {
                Id = countLoad,
                Timestamp = kp.Key,
                ForecastValue = kp.Value,
                MeasuredtValue1 = dict2[kp.Key],
                AbsolutePercentageDeviation = 0,
                SquaredDeviation = 0,
                ForecastFileId = int.Parse(s.Key.Replace("_", "")),
                MeasuredFileId = int.Parse(s.Key.Replace("_", ""))
            };

            countLoad++;
            
            return load;
        }

        private Audit CreateAuditError()
        {
            Audit audit = new Audit()
            {
                Id = countAudit,
                Timestamp = DateTime.Now.ToString(),
                MessageType = TypeMessage.Error,
                Message = "U datoteci nema 24 objekta."
            };

            countAudit++;

            return audit;
        }
       
        private Audit CreateAuditInfo(KeyValuePair<string, string>s)
        {
            Audit audit = new Audit()
            {
                Id = countAudit,
                Timestamp = DateTime.Now.ToString(),
                MessageType = TypeMessage.Info,
                Message = "Uspesno ste procitali datoteke " + "measured" + s.Key + ", " + " forecast" + s.Key
            };
          
            countAudit++;
            
            return audit;
        }

        private Audit CreateAuditWarning(string[] s)
        {
            Audit audit = new Audit()
            {
                Id = countAudit,
                Timestamp = DateTime.Now.ToString(),
                MessageType = TypeMessage.Warning,
                Message = "Nevalidna vrednost."
            };
            
            countAudit++;

            return audit;
        }

       


        private static void DisposeStreams(List<FileCSV> listMeasured, List<FileCSV> listForecasted)
        {
            foreach (FileCSV item in listMeasured)
            {
                item.Dispose();
            }

            foreach (FileCSV item in listForecasted)
            {
                item.Dispose();
            }
        }

        public List<Audit> VratiKlijentuAudit()
        {
            if(databaseType=="InMemory")
            {
                return db.ReadFromInMemoryAudit();
            }
            else
            {
                return xmlDataBase.AuditVrati();
            }
           
        }

        public List<Load> VratiKlijentuLoad()
        {
            if (databaseType == "InMemory")
            {
                return db.ReadFromInMemoryLoad();
            }
            else
            {
                return xmlDataBase.LoadVrati();
            }
        }

        public List<ImportedFile> VratiKlijentuImported()
        {
            if (databaseType == "InMemory")
            {
                return db.ReadFromInMemoryImportedFile();
            }
            else
            {
                return xmlDataBase.ImportedVrati();
            }
        }
    }
}
