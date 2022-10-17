using System.Threading.Tasks;

namespace Discord_Bot.Services.RankHandler.Interfaces
{
    public interface IRankHandler
    {
        int CountUsers { get; }
        int[] CountUserInLevels();
    }
}