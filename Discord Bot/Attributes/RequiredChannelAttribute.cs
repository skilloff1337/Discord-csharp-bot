using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot.Attributes
{
    public class RequiredChannelAttribute : PreconditionAttribute
    {
        private readonly ChannelType _channel;

        public RequiredChannelAttribute(ChannelType channel)
            => _channel = channel;


        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command,
            IServiceProvider services)
        {
            var config = services.GetRequiredService<IJsonReader<Config>>().Load();
            var client = services.GetRequiredService<DiscordSocketClient>();

            return _channel switch
            {
                ChannelType.Any => Task.FromResult(PreconditionResult.FromSuccess()),
                ChannelType.None => Task.FromResult(PreconditionResult.FromError("Unknown command.")),
                ChannelType.BotAdminCommand => Task.FromResult(
                    context.Channel != client.GetChannel(config.ChannelIdForBotAdminCommand)
                        ? PreconditionResult.FromError("Unknown command.")
                        : PreconditionResult.FromSuccess()),
                ChannelType.BotCommand => Task.FromResult(
                    context.Channel != client.GetChannel(config.ChannelIdForBotCommand)
                        ? PreconditionResult.FromError("Unknown Command.")
                        : PreconditionResult.FromSuccess()),
                ChannelType.BotNews => Task.FromResult(context.Channel != client.GetChannel(config.ChannelIdForBotNews)
                    ? PreconditionResult.FromError("Unknown Command.")
                    : PreconditionResult.FromSuccess()),
                _ => Task.FromResult(PreconditionResult.FromError($"Unknown type: {_channel}"))
            };
        }
    }
}