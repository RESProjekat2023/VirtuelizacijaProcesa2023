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
    [XmlRoot("Load")]
    [DataContract]
    public class Load
    {
        private int id;
        private string timestamp;
        private double forecastValue;
        private double MeasuredtValue;
        private double absolutePercentageDeviation=0;
        private double squaredDeviation=0;
        private int forecastFileId;
        private int measuredFileId;
        public Load() { }

        public Load(int id, string timestamp, double forecastValue, double measuredtValue, double absolutePercentageDeviation, double squaredDeviation, int forecastFileId, int measuredFileId)
        {
            this.id = id;
            this.timestamp = timestamp;
            this.forecastValue = forecastValue;
            MeasuredtValue = measuredtValue;
            this.absolutePercentageDeviation = absolutePercentageDeviation;
            this.squaredDeviation = squaredDeviation;
            this.forecastFileId = forecastFileId;
            this.measuredFileId = measuredFileId;
        }

        [XmlElement("Id")]
        [DataMember]
        public int Id { get => id; set => id = value; }
        [XmlElement("Timestamp")]
        [DataMember]
        public string Timestamp { get => timestamp; set => timestamp = value; }
        [XmlElement("ForecastValue")]
        [DataMember]
        public double ForecastValue { get => forecastValue; set => forecastValue = value; }
        [XmlElement("MeasuredtValue1")]
        [DataMember]
        public double MeasuredtValue1 { get => MeasuredtValue; set => MeasuredtValue = value; }
        [XmlElement("AbsolutePercentageDeviation")]
        [DataMember]
        public double AbsolutePercentageDeviation { get => absolutePercentageDeviation; set => absolutePercentageDeviation = value; }
        [XmlElement("SquaredDeviation")]
        [DataMember]
        public double SquaredDeviation { get => squaredDeviation; set => squaredDeviation = value; }
        [XmlElement("ForecastFileId")]
        [DataMember]
        public int ForecastFileId { get => forecastFileId; set => forecastFileId = value; }
        [XmlElement("MeasuredFileId")]
        [DataMember]
        public int MeasuredFileId { get => measuredFileId; set => measuredFileId = value; }

        public override string ToString()
        {
            return Id+" "+ Timestamp+" "+ForecastValue+" "+MeasuredtValue+" "+AbsolutePercentageDeviation + " "+ SquaredDeviation + " "+ ForecastFileId + " " + MeasuredFileId;
        }
    }
}
