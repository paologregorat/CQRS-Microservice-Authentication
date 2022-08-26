using System.Linq;
using System.Text;
using CQRSSAmple.Business.Abstract;
using CQRSSAmple.Domain.Command;
using CQRSSAmple.Domain.Entity;
using CQRSSAmple.Domain.Infrasctructure;
using CQRSSAmple.Domain.Infrasctructure.Authorization;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CQRSSAmple.Business.Operatori
{
    public class OperatoriBusiness: IOperatoriBusiness
    {
        private EntityContext _context;

        public OperatoriBusiness(EntityContext context)
        {
            _context = context;
        }
        
        public Operatore GetUtente(string username, string password)
        {
            var entity = _context.Operatori.FirstOrDefault(c => c.UserName == username && c.Password == password);
            return entity;
        }

        public CommandResponse CreteToken(CreateTokenCommand command)
        {
            var response = new CommandResponse()
            {
                Success = false
            };
           
            var utente = GetUtente(command.UserName, command.Password);
            if (utente == default)
            {
                response.Success = false;
                response.Message = "Operatore non trovato.";
                return response;
            }
            
            var token = new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create("grgpla74a26g284d"))
                .AddSubject(utente.Cognome)
                .AddIssuer("Fiver.Security.Bearer")
                .AddAudience("Fiver.Security.Bearer")
                .AddClaim("ID", utente.ID.ToString())
                .AddExpiry(1440)
                .Build();
            
            response.ID = utente.ID;
            response.Success = true;
            response.Message = token.Value;
            
            return response;
        }
        
        public CommandResponse Save(SaveOperatoreCommand command)
        {
            var response = new CommandResponse()
            {
                Success = false
            };

            var integrationEventData = "";
            var integrationEvent = "";
            var toUpdate = _context.Operatori.FirstOrDefault(e => e.ID == command.Operatore.ID);
            if (toUpdate == default)
            {
                _context.Operatori.Add(command.Operatore);
                integrationEventData = JsonConvert.SerializeObject(command.Operatore);
                integrationEvent = "operatore.add";
            } else {
                
                _context.Entry(toUpdate).CurrentValues.SetValues(command.Operatore);
                integrationEventData = JsonConvert.SerializeObject(toUpdate);
                integrationEvent = "operatore.update";
            }
            
            _context.SaveChanges();
            
            //send event to RabbitMQ 
            PublishOperatoreEventToMessageQueue(integrationEvent, integrationEventData);
            
            
            response.ID = command.Operatore.ID;
            response.Success = true;
            response.Message = "Entity salvata.";

            
            
            return response;
        }

        private void PublishOperatoreEventToMessageQueue(string integrationEvent, string eventData)
        {
            // TOOO: Reuse and close connections and channel, etc, 
            var factory = new ConnectionFactory(){ HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(eventData);
            channel.BasicPublish(exchange: "user",
                routingKey: integrationEvent,
                basicProperties: null,
                body: body);
        }
    }
}
