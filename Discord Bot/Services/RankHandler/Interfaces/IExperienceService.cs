namespace Discord_Bot.Services.RankHandler
{
    public interface IExperienceService
    {
        bool EnoughExperience(uint currentExp, byte currentLevel);
    }
}