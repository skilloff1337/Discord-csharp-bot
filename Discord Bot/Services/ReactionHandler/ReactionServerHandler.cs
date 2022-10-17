using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.Models;

namespace Discord_Bot.Services.ReactionHandler
{
    public class ReactionServerHandler
    {
        private readonly Config _config;
        private readonly DiscordSocketClient _client;

        public ReactionServerHandler(Config config, DiscordSocketClient client)
        {
            _config = config;
            _client = client;
            _client.ReactionAdded += ReactionAdded;
            _client.ReactionRemoved += ReactionRemoved;
        }
        private Task ReactionRemoved(Cacheable<IUserMessage, ulong> msgId, Cacheable<IMessageChannel, ulong> channel,
            SocketReaction reaction)
        {
            Console.WriteLine();
            if (_config.IdMessageServerRole != msgId.Id)
                return Task.CompletedTask;

            var user = _client.GetGuild(_config.IdServer).GetUser(reaction.UserId) as IGuildUser;

            switch (reaction.Emote.Name)
            {
                case "ServerAlpha":
                    user!.RemoveRoleAsync(1031502998037340213);
                    break;
                case "5mp":
                    user!.RemoveRoleAsync(1031503040768917575);
                    break;
            }
            
            return Task.CompletedTask;
        }

        private Task ReactionAdded(Cacheable<IUserMessage, ulong> msgId, Cacheable<IMessageChannel, ulong> channel,
            SocketReaction reaction)
        {
            if (_config.IdMessageServerRole != msgId.Id)
                return Task.CompletedTask;

            var user = _client.GetGuild(_config.IdServer).GetUser(reaction.UserId) as IGuildUser;

            switch (reaction.Emote.Name)
            {
                case "ServerAlpha":
                    user!.AddRoleAsync(1031502998037340213);
                    break;
                case "5mp":
                    user!.AddRoleAsync(1031503040768917575);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}