using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Common
{
    public enum TypeMessage { Info, Warning, Error};
    [Serializable]
    [XmlRoot("Audit")]
    [DataContract]
    public class Audit
    {
        private int id;
        private string timestamp;
        private TypeMessage messageType;
        private string message;
        public Audit() { }

        public Audit(int id, string timestamp, TypeMessage messageType, string message)
        {
            this.id = id;
            this.timestamp = timestamp;
            this.messageType = messageType;
            this.message = message;
        }
        

        [XmlElement("Id")]
        [DataMember]
        public int Id { get => id; set => id = value; }
        [XmlElement("Timestamp")]
        [DataMember]
        public string Timestamp { get => timestamp; set => timestamp = value; }
        [XmlElement("Message")]
        [DataMember]
        public string Message { get => message; set => message = value; }
        [XmlElement("MessageType")]
        [DataMember]
        public TypeMessage MessageType { get => messageType; set => messageType = value; }

        public override string ToString()
        {
            return Id + " " + Timestamp + " " + MessageType + " " + Message;
        }
    }
}
