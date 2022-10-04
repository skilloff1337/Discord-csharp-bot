using System.Threading.Tasks;

namespace Discord_Bot.Services.RankHandler
{
    public interface IRankHandler
    {
        Task InstallEventsAsync();
    }
}