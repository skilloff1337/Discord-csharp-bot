using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.Models;

namespace Discord_Bot.Services.ReactionHandler
{
    public class ReactionLanguageHandler
    {
        private readonly Config _config;
        private readonly DiscordSocketClient _client;

        public ReactionLanguageHandler(Config config, DiscordSocketClient client)
        {
            _config = config;
            _client = client;
            _client.ReactionAdded += ReactionAdded;
            _client.ReactionRemoved += ReactionRemoved;
        }
        private Task ReactionRemoved(Cacheable<IUserMessage, ulong> msgId, Cacheable<IMessageChannel, ulong> channel,
            SocketReaction reaction)
        {
            if (_config.IdMessageLanguageRole != msgId.Id)
                return Task.CompletedTask;

            var user = _client.GetGuild(_config.IdServer).GetUser(reaction.UserId) as IGuildUser;

            switch (reaction.Emote.Name)
            {
                case "🇷🇺":
                    user!.RemoveRoleAsync(_config.IdRoleRu);
                    break;
                case "🇺🇸":
                    user!.RemoveRoleAsync(_config.IdRoleUs);
                    break;
            }
            
            return Task.CompletedTask;
        }

        private Task ReactionAdded(Cacheable<IUserMessage, ulong> msgId, Cacheable<IMessageChannel, ulong> channel,
            SocketReaction reaction)
        {
            if (_config.IdMessageLanguageRole != msgId.Id)
                return Task.CompletedTask;

            var user = _client.GetGuild(_config.IdServer).GetUser(reaction.UserId) as IGuildUser;

            switch (reaction.Emote.Name)
            {
                case "🇷🇺":
                    user!.AddRoleAsync(_config.IdRoleRu);
                    break;
                case "🇺🇸":
                    user!.AddRoleAsync(_config.IdRoleUs);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}