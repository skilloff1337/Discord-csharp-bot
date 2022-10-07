using Discord;
using Discord.WebSocket;
using Discord_Bot.Models;

namespace Discord_Bot.Services.RankHandler
{
    public class ExperienceService : IExperienceService
    {
        private readonly Config _config;
        private readonly DiscordSocketClient _client;

        public ExperienceService(Config config, DiscordSocketClient client)
        {
            _config = config;
            _client = client;
        }

        public bool EnoughExperience(uint currentExp, byte currentLevel)
        {
            var messageChannel = _client.GetChannel(_config.ChannelIdForBotLog) as IMessageChannel;

            if (currentLevel < _config.Ranks.Count) 
                return currentExp >= _config.Ranks[currentLevel + 1].NeedExp;
            
            messageChannel!.SendMessageAsync($"Not found rank, level {currentLevel+1}");
            return false;

        }
    }
}