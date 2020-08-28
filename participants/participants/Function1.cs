using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Driver;

namespace participants
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // string name = req.Query["name"];
            var client = new MongoClient(
                "mongodb+srv://salazarpp:Pp1739M@cluster0.tiajx.azure.mongodb.net/party"
            );
            var database = client.GetDatabase("party");
            var collection = database.GetCollection<Participants>("participants");

            // var participants = new Participants { name = "sadf", confirmation = 1 };
            // collection.InsertOne(participants);
            // var id = participants.Id; // Insert will set the Id if necessary (as it was in this example)

            collection = database.GetCollection<Participants>("participants");
            var documents = await collection.Find(_ => true).ToListAsync();
            var jsonList = JsonConvert.SerializeObject(documents);
            /*
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                name = name ?? data?.name;
                string responseMessage = string.IsNullOrEmpty(name)
                    ? jsonList
                    : $"Hello, {name}. This HTTP triggered function executed successfully.";
            */

            return new OkObjectResult(jsonList);
        }
    }
}
