using System.Threading.Tasks;

namespace Discord_Bot.Services.TextChatHandler
{
    public interface IWelcomeHandler
    {
        Task InstallCommandsAsync();
    }
}