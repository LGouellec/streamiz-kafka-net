﻿using Confluent.Kafka;
using System;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var producerConfig = new ProducerConfig
            {
                Acks = Acks.All,
                BootstrapServers = "192.168.56.1:9092",
                SaslMechanism = SaslMechanism.ScramSha512,
                SaslPassword = "Michelin/1",
                SaslUsername = "admin",
                SecurityProtocol = SecurityProtocol.SaslPlaintext
            };
            var topic = "test-ktable";
            var builder = new ProducerBuilder<String, String>(producerConfig);
            Console.WriteLine("Enter exit for stopping producer, or enter KEY:VALUE");
            using (var producer = builder.Build())
            {
                string s = Console.ReadLine();
                while (!s.Contains("exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    string[] r = s.Split(":");
                    producer.Produce(topic, new Message<string, string> { Key = r[0], Value = r[1] }, (d) =>
                    {
                        if (d.Status == PersistenceStatus.Persisted)
                        {
                            Console.WriteLine("Message sent !");
                        }
                    });
                    s = Console.ReadLine();
                }
            }
        }
    }
}