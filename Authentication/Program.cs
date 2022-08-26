using Authentication.Queue;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Authentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //TODO: creare l'exchanges e le queues per utilizzare RabbitMQ events in queue
            //ListenForIntegrationEvents.Listen();
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}