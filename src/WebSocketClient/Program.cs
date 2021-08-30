using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace WebSocketClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:8888/hub/notification")
                .Build();

            await connection.StartAsync();
            Console.WriteLine("Connection started...");
            Console.WriteLine("Waiting for messages...");
            connection.On<string, string>("ProductionPlanCalculated", (command, productionPlan) =>
            {
                Console.WriteLine(productionPlan);
            });

            Console.ReadKey();
        }
    }
}
