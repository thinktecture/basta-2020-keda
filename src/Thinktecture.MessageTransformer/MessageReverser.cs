using System;
using System.IO;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Thinktecture.MessageTransformer
{
    public static class MessageReverser
    {
        [FunctionName("MessageReverser")]
        [return: Queue("aaa-reversed", Connection = "BastaStorageAccount")]
        public static string Run(
            [QueueTrigger("aaa-tasks", Connection = "BastaStorageAccount")] string queueItem,
            ILogger log)
        {
            log.LogInformation("Processing Task");

            var task = JsonSerializer.Deserialize<Models.Task>(queueItem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            if (task != null)
            {
                log.LogInformation("Message composed and reversed");
                return JsonSerializer.Serialize(new { Message = task.Reverse() }, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            }
            log.LogWarning("Cant reverse an empty message");
            return null;
        }
    }
}
