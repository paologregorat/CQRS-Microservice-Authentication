using CQRSSAmple.Business.Operatori;
using CQRSSAmple.Domain.Command.Abstract;
using CQRSSAmple.Domain.Entity;

namespace CQRSSAmple.Domain.Command.Handler.Operatori
{
    public class SaveOperatoreCommandHandler: ICommandHandler<SaveOperatoreCommand, CommandResponse>
    {
        private readonly SaveOperatoreCommand _command;
        private readonly OperatoriBusiness _business;
        public SaveOperatoreCommandHandler(SaveOperatoreCommand command, OperatoriBusiness business)
        {
            _command = command;
            _business = business;
        }
        public CommandResponse Execute()
        {
            return _business.Save(_command);
        }
    }
}
