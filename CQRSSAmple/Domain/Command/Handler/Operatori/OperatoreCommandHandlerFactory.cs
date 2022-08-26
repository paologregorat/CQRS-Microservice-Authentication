using CQRSSAmple.Business.Operatori;
using CQRSSAmple.Domain.Command.Abstract;
using CQRSSAmple.Domain.Entity;

namespace CQRSSAmple.Domain.Command.Handler.Operatori
{
    public static class OperatoreCommandHandlerFactory
    {
        public static ICommandHandler<CreateTokenCommand, CommandResponse> Build(CreateTokenCommand command, OperatoriBusiness business)
        {
            return new CreteTokenCommandHandler(command, business);
        }
        
        public static ICommandHandler<SaveOperatoreCommand, CommandResponse> Build(SaveOperatoreCommand command, OperatoriBusiness business)
        {
            return new SaveOperatoreCommandHandler(command, business);
        }
    }
}
