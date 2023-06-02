using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Server
{
    public class Deviation
    {

        public static void SearchXmlFile(string filePath,XmlDocument xmlDoc)
        {
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load(filePath);

            XmlNodeList objectNodes = xmlDoc.GetElementsByTagName("Load");
            foreach (XmlNode objectNode in objectNodes)
            {
                XmlNode measuredValue = objectNode.SelectSingleNode("MeasuredValue1");
                XmlNode forcastValue = objectNode.SelectSingleNode("ForecastValue");
                XmlNode AbsolutePercentageDeviation = objectNode.SelectSingleNode("AbsolutePercentageDeviation");
                XmlNode SquaredDeviation = objectNode.SelectSingleNode("SquaredDeviation");
                if (measuredValue != null && forcastValue != null)
                {
                    if (FileHandler.formulaType == "Percentage")
                    {
                        AbsolutePercentageDeviation.InnerText = (Math.Abs(double.Parse(measuredValue.InnerText) - double.Parse(forcastValue.InnerText)) / double.Parse(measuredValue.InnerText) * 100).ToString();
                    }
                    else
                    {
                        SquaredDeviation.InnerText = (Math.Pow(((double.Parse(measuredValue.InnerText) - double.Parse(forcastValue.InnerText)) / double.Parse(measuredValue.InnerText)), 2)).ToString();
                    }
                }

            }

            xmlDoc.Save(filePath);

            Console.WriteLine("Property value updated successfully.");



        }
    }
}
