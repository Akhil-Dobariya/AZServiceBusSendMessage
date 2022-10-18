using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace AZServiceBusSendMessage
{
    class Program
    {
        static string connectionString = "Your ServiceBusConnectionString Or SAS Token";

        static string topicName = "Your Topic Name";

        static ServiceBusClient client;

        static ServiceBusSender sender;

        static async Task Main(string[] args)
        {
            client = new ServiceBusClient(connectionString);

            sender = client.CreateSender(topicName);

            ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            string keepSending = "";

            do
            {

                for (int i = 0; i < 10; i++)
                {
                    if (messageBatch.TryAddMessage(new ServiceBusMessage($"This is message {i + 1} In Batch")))
                    {
                        Console.WriteLine($"Added in Batch: This is message {i + 1}");
                    }
                }

                try
                {
                    await sender.SendMessagesAsync(messageBatch);
                    Console.WriteLine($"A batch of {messageBatch.Count} messages has been completed");

                    for (int i = 0; i < 5; i++)
                    {
                        await sender.SendMessageAsync(new ServiceBusMessage($"This is message {i + 1}"));
                        Console.WriteLine($"Sent: This is message {i + 1}");
                    }

                    keepSending = Console.ReadLine();
                    Console.WriteLine($"Pressed {keepSending}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.ToString()}");
                }

            } while (keepSending != "n");

        }
    }
}
