using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Hosting;
using System.Web.Http;

namespace CommunitySite.CodeSamples.Controllers
{
    public class SampleController : ApiController
    {
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
