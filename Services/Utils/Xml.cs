using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OneConnector.Services.Utils
{
    public class Xml
    {
        public static string Serialize(object sourceObject)
        {
            var serializer = new XmlSerializer(sourceObject.GetType());
            using (StringWriter writer = new())
            {
                serializer.Serialize(writer, sourceObject);
                writer.Flush();
                return writer.ToString();
            }
        }

        public static T Deserialize<T>(string sourceXML) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(sourceXML))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static string RenameNodes(string sourceXML, Dictionary<string, string> renameMap)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(sourceXML);
            foreach (string xPath in renameMap.Keys)
            {
                XmlNodeList nodeList = xmlDocument.SelectNodes(xPath);
                foreach (XmlNode node in nodeList)
                {
                    XmlNode newNode = xmlDocument.CreateElement(renameMap[xPath]);
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        newNode.AppendChild(childNode.CloneNode(true));
                    }
                    XmlNode parent = node.ParentNode;
                    parent.AppendChild(newNode);
                    parent.RemoveChild(node);
                }
            }
            return xmlDocument.OuterXml;
        }
    }
}
