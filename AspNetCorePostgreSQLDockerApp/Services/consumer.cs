using Confluent.Kafka;
using System;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace AspNetCorePostgreSQLDockerApp.Consumer
{
    class Consumer {

        public static void Start()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddIniFile("./getting-started.properties")
                .Build();

            configuration["group.id"] = "kafka-dotnet-getting-started";
            configuration["auto.offset.reset"] = "earliest";

            const string topic = "invoices";

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            using (var consumer = new ConsumerBuilder<string, string>(
                configuration.AsEnumerable()).Build())
            {
                consumer.Subscribe(topic);

                Console.WriteLine(topic);
                
                try {
                    while (true) {
                        var cr = consumer.Consume(cts.Token);
                        Console.WriteLine($"Consumed event from topic {topic} with key {cr.Message.Key,-10} and value {cr.Message.Value}");
                    }
                }
                catch (OperationCanceledException) {
                    // Ctrl-C was pressed.
                }
                finally{
                    consumer.Close();
                }
            }
        }
    }
}