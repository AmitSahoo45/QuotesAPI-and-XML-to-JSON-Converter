using Newtonsoft.Json;
using System;
using System.Xml;
using System.Xml.Linq;

namespace QuotesAPI.Utility
{
    public class CustomUtility
    {
        public static string XMLtoJSON(XmlDocument xml)
        {
            try
            {
                XNode node = XDocument.Parse(xml.OuterXml);
                string json = JsonConvert.SerializeXNode(node, Newtonsoft.Json.Formatting.Indented);

                return json;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}