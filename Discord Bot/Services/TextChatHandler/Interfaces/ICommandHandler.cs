using System.Threading.Tasks;

namespace Discord_Bot.Services.TextChat.Interfaces
{
    public interface ICommandHandler
    {
        Task InstallCommandsAsync();
    }
}