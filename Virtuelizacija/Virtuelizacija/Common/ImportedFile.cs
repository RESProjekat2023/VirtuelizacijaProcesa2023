using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Common
{
    [Serializable]
    [DataContract]
    [XmlRoot("ImportedFile")]
    public class ImportedFile
    {
        [XmlElement("Id")]
        [DataMember]
        public int Id { get; set; }
        [XmlElement("FileName")]
        [DataMember]
        public string FileName { get; set; }

        public ImportedFile() { }
        public ImportedFile(int id, string fileName)
        {
            Id = id;
            FileName = fileName;
        }
        public override string ToString()
        {
            return $"{Id} {FileName}";
        }
    }
}
