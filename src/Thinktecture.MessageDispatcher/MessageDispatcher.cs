using System;
using System.IO;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Thinktecture.MessageDispatcher
{
    public static class MessageDispatcher
    {
        [FunctionName("MessageDispatcher")]
        public static void Run(
            [QueueTrigger("aaa-reversed", Connection = "BastaStorageAccount")] string queueItem,
            [Blob("processed/{rand-guid}.json", FileAccess.Write, Connection = "BastaStorageAccount")] Stream processedBlob,
            ILogger log)
        {
            log.LogInformation("Processing Reversed Task");

            var task = JsonSerializer.Deserialize<Models.ReversedTask>(queueItem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            if (task != null)
            {
                var numberOfKittens = task.ExtractNumberOfKittens();
                using (var writer = new StreamWriter(processedBlob))
                {
                    writer.Write(JsonSerializer.Serialize(new
                    {
                        Id = Guid.NewGuid(),
                        NumberOfKittens = numberOfKittens,
                        AreaRequirements = $"{numberOfKittens * 0.75}qm"
                    }, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }));
                    log.LogInformation("Message destructed and processed");
                }
                return;
            }
            log.LogWarning("Cant process am empty task");
        }
    }
}
