using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot.Attributes
{
    public class RequiredChannelAttribute : PreconditionAttribute
    {
        private readonly DiscordChannelType _discordChannel;

        public RequiredChannelAttribute(DiscordChannelType discordChannel)
            => _discordChannel = discordChannel;


        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command,
            IServiceProvider services)
        {
            var config = services.GetRequiredService<Config>();
            var client = services.GetRequiredService<DiscordSocketClient>();

            return _discordChannel switch
            {
                DiscordChannelType.Any => Task.FromResult(PreconditionResult.FromSuccess()),
                DiscordChannelType.None => Task.FromResult(PreconditionResult.FromError("Unknown command.")),
                DiscordChannelType.BotAdminCommand => Task.FromResult(
                    context.Channel != client.GetChannel(config.ChannelIdForBotAdminCommand)
                        ? PreconditionResult.FromError("Unknown command.")
                        : PreconditionResult.FromSuccess()),
                DiscordChannelType.BotCommand => Task.FromResult(
                    context.Channel != client.GetChannel(config.ChannelIdForBotCommand)
                        ? PreconditionResult.FromError("Unknown Command.")
                        : PreconditionResult.FromSuccess()),
                _ => Task.FromResult(PreconditionResult.FromError($"Unknown type: {_discordChannel}"))
            };
        }
    }
}