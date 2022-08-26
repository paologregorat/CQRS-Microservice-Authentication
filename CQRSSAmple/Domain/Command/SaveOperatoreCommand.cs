using CQRSSAmple.Domain.Command.Abstract;
using CQRSSAmple.Domain.Entity;

namespace CQRSSAmple.Domain.Command
{
    public class SaveOperatoreCommand : ICommand<CommandResponse>
    {
    public Operatore Operatore { get; private set; }
    public SaveOperatoreCommand(Operatore item)
    {
        Operatore = item;
    }
    }
}
