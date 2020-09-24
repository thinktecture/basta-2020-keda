using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Thinktecture.MessageProducer.Models;
using Task = Thinktecture.MessageProducer.Models.Task;

namespace Thinktecture.MessageProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var configFilePath = Path.Join(Environment.CurrentDirectory, "config.json");
            if (!File.Exists(configFilePath))
            {
                Console.WriteLine("Please create a config.json file with a ConnectionString for Azure Blob Storage");
            }
            var serializerSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var configString = File.ReadAllText(configFilePath);
            var config = JsonSerializer.Deserialize<Config>(configString, serializerSettings);

            var queueClient = new QueueClient(config.ConnectionString, "aaa-tasks");

            Parallel.For(1, 100000, (i) =>
             {
                 var t = new Task("Azure Function destructed message no {0}.", i);
                 var message = JsonSerializer.Serialize(t, serializerSettings);
                 queueClient.SendMessage(Base64Encode(message));
                 Console.WriteLine($"Message {i} sent to queue");
             });
        }

        private static string Base64Encode(string message)
        {
            var b = Encoding.UTF8.GetBytes(message);
            return Convert.ToBase64String(b);
        }
    }
}
