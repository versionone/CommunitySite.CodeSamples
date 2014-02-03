namespace CommunityService

open System
open System.Web.Http
open System.IO
open System.Net.Http
open System.Net
open System.Text
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System.Collections.Generic

type HttpRouteDefaults = { Controller : string; Id : obj }

type Global() = 
    inherit System.Web.HttpApplication()
    member this.Application_Start (sender: obj) (e: EventArgs) =
        GlobalConfiguration.Configuration.MapHttpAttributeRoutes() |> ignore
        GlobalConfiguration.Configuration.Routes.MapHttpRoute(
            "DefaultAPI", 
            "api/{controller}/{id}",
            { Controller = "Home"; Id = RouteParameter.Optional }) |> ignore
        GlobalConfiguration.Configuration.EnsureInitialized() |> ignore

module CommunityAPI =
    let CreateErrorResponse(errorMessage : string) =
        let response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        response.Content <- new StringContent(errorMessage)
        response

    let ParseStringToken(value : string) =
        JToken.Parse("\"" + value + "\"")

    type SampleController() =
        inherit ApiController()
        member this.Get(path: string) =
            let samplePath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + path)
            if String.IsNullOrWhiteSpace path then CreateErrorResponse "Please specify a valid path parameter in the form of /sample/<path>."
            elif File.Exists samplePath = false then CreateErrorResponse ("Could not find file at the specified path: " + path)
            else 
                use stream = new StreamReader(samplePath)
                let content = stream.ReadToEnd()
                let response = new HttpResponseMessage(HttpStatusCode.OK)
                response.Content <- new StringContent(content, Encoding.UTF8, "text/plain")
                response

        [<Route("api/samples")>]
        member this.GetByTopic(topic: string) =
            
            let topicPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + topic)
            if String.IsNullOrWhiteSpace topicPath then CreateErrorResponse "Please specify a valid path parameter in the form of /sample?topic=<topic>."
            elif Directory.Exists topicPath = false then CreateErrorResponse ("Could not find a topic named: " + topic)
            else 
                let directory = new DirectoryInfo(topicPath)
                let jArray = new JArray()
                directory.GetDirectories() |> Seq.iter (fun langDir -> 
                    let examples = new JArray()
                    let files = langDir.GetFiles()
                    let exampleFiles = query {
                        for file in files do 
                        where (file.Extension <> ".json")
                        select file
                    }
                    let rootObject = new JObject()
                    rootObject.Add("mode", ParseStringToken langDir.Name)
                    rootObject.Add("content", ParseStringToken String.Empty)
                    rootObject.Add("id", ParseStringToken langDir.Name)
                    let filePathRoot = "https://v1codesamples.azurewebsites.net/api/sample?path=" + topic + "/" + langDir.Name + "/"
                    exampleFiles |> Seq.iter (fun file ->
                        let fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name)
                        let properties = query {
                            for file in files do
                            where (file.Name = fileNameWithoutExtension + ".json")
                            head
                        }
                        use reader = new StreamReader(properties.OpenRead())
                        let json = reader.ReadToEnd()
                        let exampleRoot = rootObject.DeepClone() :?> JObject;
                        let mergeWith = JObject.Parse(json)
                        let example = new JObject(exampleRoot)
                        for token in mergeWith.Properties() do
                            example.Add(token.Name, token.Value)
                        example.Add("url", ParseStringToken (filePathRoot + file.Name))
                        // TODO: multiple examples per language support
                        //examples.Add(example);
                        jArray.Add(example)
                    ) |> ignore                    
                ) |> ignore

                let content = jArray.ToString()

                let response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content <- new StringContent(content, Encoding.UTF8, "text/plain");
                response