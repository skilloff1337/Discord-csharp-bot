using System.Threading.Tasks;

namespace Discord_Bot.Services.TextChatHandler.Interfaces
{
    public interface ICommandHandler
    {
        Task InstallCommandsAsync();
    }
}