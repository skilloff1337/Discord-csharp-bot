using System.Threading.Tasks;

namespace Discord_Bot.Services.UserHandler.Interfaces
{
    public interface IUserHandler
    {
        Task InstallEventsAsync();
    }
}