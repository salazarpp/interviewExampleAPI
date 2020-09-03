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
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

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
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(idValue))
            {
                idValue = req.Headers["id"];
            }


            var client = new MongoClient(
                "mongodb+srv://salazarpp:Pp1739M@cluster0.tiajx.azure.mongodb.net/party"
            );
            var database = client.GetDatabase("party");
            var collection = database.GetCollection<Participants>("participants");

            var jsonList = JsonConvert.SerializeObject(null);
            
            if (req.Method == "POST" && !string.IsNullOrEmpty(requestBody))
            {
                dynamic response = JsonConvert.DeserializeObject(requestBody);
                List<Participants> sendData = response.ToObject<List<Participants>>();
                await collection.InsertManyAsync(sendData);
                collection = database.GetCollection<Participants>("participants");
                var documents = await collection.Find(_ => true).ToListAsync();
                jsonList = JsonConvert.SerializeObject(documents);
            }

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
