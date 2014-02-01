using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web.Hosting;

namespace CodeSamples
{
    public class Sample : ApiController
    {
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        public string Get(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("Please specify a valid path parameter in the form of /sample/<path>.");
            }
            var samplePath = HostingEnvironment.MapPath("~/" + path);
            if (!File.Exists(samplePath))
            {
                throw new FileNotFoundException("Could not find file at the specified path: " + path);
            }

            var content = new StreamReader(samplePath).ReadToEnd();

            return content;
        }

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}