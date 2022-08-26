using System;
using System.Text;
using Authentication.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Linq;
using System.Runtime.CompilerServices;
using Authentication.Domain.Infrasctructure;
using Microsoft.Extensions.Configuration;

namespace Authentication.Queue
{
    public class ListenForIntegrationEvents
    {
        //private readonly EntityContext _dbContext;
        
        //public ListenForIntegrationEvents(EntityContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}
        
        public static void Listen()
        {
            var factory = new ConnectionFactory(){ HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                //var contextOptions = new DbContextOptionsBuilder<EntityContext>().Options;
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfiguration configuration = builder.Build();
                var connString =(string) configuration.GetValue(typeof(string), "ConnectionString");
                var dbContext = new ApplicationDbContext(connString);                
                
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                var data = JObject.Parse(message);
                var type = ea.RoutingKey;
                if (type == "operatore.add")
                {
                    var id = new Guid(data["ID"].Value<string>());
                    var nome = data["Nome"].Value<string>();
                    var cognome = data["Cognome"].Value<string>();
                    var userName = data["UserName"].Value<string>();
                    var password = data["Password"].Value<string>();
                    
                    
                    dbContext.Operatori.Add(new Operatore(id, nome, cognome, userName, password));
                    dbContext.SaveChanges();
                }
                else if (type == "operatore.update")
                {
                    var user = dbContext.Operatori.First(a => a.ID == data["id"].Value<Guid>());
                    user.Password = "kfajlfadslfjadlfsa";
                    dbContext.SaveChanges();
                }
            };
            channel.BasicConsume(queue: "user.postservice",
                autoAck: true,
                consumer: consumer);
        }
    }
}