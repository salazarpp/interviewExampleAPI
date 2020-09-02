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

            String idValue = req.Query["id"];

            var client = new MongoClient(
                "mongodb+srv://salazarpp:Pp1739M@cluster0.tiajx.azure.mongodb.net/party"
            );
            var database = client.GetDatabase("party");
            var collection = database.GetCollection<Participants>("participants");

            // var participants = new Participants { name = "sadf", confirmation = 1 };
            // collection.InsertOne(participants);
            // var id = participants.Id; // Insert will set the Id if necessary (as it was in this example)
            var jsonList = JsonConvert.SerializeObject(null);

            if (string.IsNullOrEmpty(idValue))
            {
                collection = database.GetCollection<Participants>("participants");
                var documents = await collection.Find(_ => true).ToListAsync();
                jsonList = JsonConvert.SerializeObject(documents);
            } else
            {
                ObjectId id = ObjectId.Parse(idValue);
                collection = database.GetCollection<Participants>("participants");
                var documents = collection.Find(x => x.Id == id).ToList();
                jsonList = JsonConvert.SerializeObject(documents);
            }

            return new OkObjectResult(jsonList);
        }
    }
}
