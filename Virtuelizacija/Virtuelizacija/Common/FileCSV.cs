using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.IO;

namespace Common
{
    [DataContract]
    public class FileCSV:IDisposable
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public MemoryStream FileStream { get; set; }

        public void Dispose()
        {
            if(FileStream != null)
            {
                return;
            }
            else
            {
                try
                {
                    FileStream.Dispose();
                    FileStream.Close();
                    FileStream = null;
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
