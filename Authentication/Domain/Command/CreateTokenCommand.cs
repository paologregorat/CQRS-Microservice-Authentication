using Authentication.Domain.Command.Abstract;
using Authentication.Domain.Entity;

namespace Authentication.Domain.Command
{
    public class CreateTokenCommand : ICommand<CommandResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public CreateTokenCommand(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
