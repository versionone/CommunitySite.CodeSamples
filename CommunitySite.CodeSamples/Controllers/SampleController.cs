using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Hosting;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommunitySite.CodeSamples.Controllers
{
    public class SampleController : ApiController
    {
        [Route("api/samples")]
        public HttpResponseMessage GetByTopic(string topic)
        {
            var topicPath = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(topic))
                {
                    throw new ArgumentNullException("Please specify a valid topic parameter in the form of /sample/<topic>.");
                }
                topicPath = HostingEnvironment.MapPath("~/" + topic);
                if (!Directory.Exists(topicPath))
                {
                    throw new FileNotFoundException("Could not find a topic named: " + topic);
                }
            }
            catch (ArgumentNullException ex)
            {
                return CreateErrorResponse(ex);
            }
            catch (FileNotFoundException ex)
            {
                return CreateErrorResponse(ex);
            }

            var directories = new DirectoryInfo(topicPath);
            var jArray = new JArray();

            foreach (var langDir in directories.GetDirectories())
            {
                var examples = new JArray();
                var files = langDir.GetFiles();
                var exampleFiles = files.Where(f => f.Extension != ".json");
                var rootObject = new JObject();
                rootObject["mode"] = langDir.Name;
                rootObject["content"] = string.Empty;
                rootObject["id"] = langDir.Name;
                var filePathRoot = "https://v1codesamples.azurewebsites.net/api/sample?path=" + topic + "/" + langDir.Name + "/";

                foreach (var file in exampleFiles)
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
                    var properties = files.First(f => f.Name == fileNameWithoutExtension + ".json");
                    using (var reader = new StreamReader(properties.OpenRead()))
                    {
                        var json = reader.ReadToEnd();
                        var exampleRoot = rootObject.DeepClone() as JObject;
                        var mergeWith = JObject.Parse(json);
                        var example = new JObject(exampleRoot.Concat(mergeWith.AsJEnumerable()));
                        example["url"] = filePathRoot + file.Name;
                        // TODO: multiple examples per language support
                        //examples.Add(example);
                        jArray.Add(example);
                    }
                }
                // TODO: support multiple examples
                //jArray.Add(examples);
            }

            var content = jArray.ToString();

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(content, Encoding.UTF8, "text/plain");
            return resp;

        }

        public HttpResponseMessage Get(string path)
        {
            var samplePath = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new ArgumentNullException("Please specify a valid path parameter in the form of /sample/<path>.");
                }
                samplePath = HostingEnvironment.MapPath("~/" + path);
                if (!File.Exists(samplePath))
                {
                    throw new FileNotFoundException("Could not find file at the specified path: " + path);
                }
            }
            catch (ArgumentNullException ex) 
            {
                return CreateErrorResponse(ex);
            }
            catch (FileNotFoundException ex)
            {
                return CreateErrorResponse(ex);
            }

            var content = new StreamReader(samplePath).ReadToEnd();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(content, Encoding.UTF8, "text/plain");
            return resp;
        }

        private static HttpResponseMessage CreateErrorResponse(Exception ex)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            resp.Content = new StringContent(ex.Message);
            return resp;
        }
    }
}
