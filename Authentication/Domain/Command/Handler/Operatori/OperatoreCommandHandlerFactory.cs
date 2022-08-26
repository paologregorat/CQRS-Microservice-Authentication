using Authentication.Business.Operatori;
using Authentication.Domain.Command;
using Authentication.Domain.Command.Abstract;
using Authentication.Domain.Entity;

namespace CQRSSAmple.Domain.Command.Handler.Operatori
{
    public static class OperatoreCommandHandlerFactory
    {
        public static ICommandHandler<CreateTokenCommand, CommandResponse> Build(CreateTokenCommand command, OperatoriBusiness business)
        {
            return new CreteTokenCommandHandler(command, business);
        }
    }
}
