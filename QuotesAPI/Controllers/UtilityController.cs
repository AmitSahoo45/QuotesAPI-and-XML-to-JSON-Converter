using System;
using System.IO;
using System.Net;
using System.Web.Http;
using System.Xml;
using Newtonsoft.Json;
using QuotesAPI.Utility;

namespace QuotesAPI.Controllers
{
    public class UtilityController : ApiController
    {
        [HttpGet]
        [Route("api/Utility/XMLToJSON/{fileName}")]
        public IHttpActionResult ConvertXMLToJSON(string fileName)
        {
            try
            {
                fileName = $"{fileName.ToLower().Trim()}.xml";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "test", fileName);

                if (!File.Exists(filePath))
                    return Content(HttpStatusCode.NotFound, $"The file {fileName} does not exist.");

                XmlDocument xml = new XmlDocument();
                xml.Load(filePath);

                // var json = CustomUtility.XMLtoJSON(xml);
                dynamic jsonObject = JsonConvert.DeserializeObject(JsonConvert.SerializeXmlNode(xml, Newtonsoft.Json.Formatting.Indented, true));

                return Ok(jsonObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return BadRequest("An error occurred while processing the request.");
            }
        }
    }
}
