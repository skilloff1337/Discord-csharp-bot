using Discord_Bot.Models;

namespace Discord_Bot.Services.RankHandler.Interfaces
{
    public interface IRankHandler
    {
        int CountUsers { get; }
        int[] CountUserInLevels();
        User GetUser(ulong discordId);
    }
}