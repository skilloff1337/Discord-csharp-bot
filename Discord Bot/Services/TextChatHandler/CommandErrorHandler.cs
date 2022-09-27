﻿using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.Interfaces;

namespace Discord_Bot.Services.TextChatHandler
{
    public class CommandErrorHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly ulong _idBotChannel;

        public CommandErrorHandler(CommandService command, DiscordSocketClient client, IJsonReader<Config> configReader)
        {
            _client = client;
            _idBotChannel = configReader.Load().ChannelIdForBotCommand;
            command.CommandExecuted += ErrorHandler;
        }

        private async Task ErrorHandler(Optional<CommandInfo> info, ICommandContext context, IResult result)
        {
            if (context.Channel != _client.GetChannel(_idBotChannel))
                return;

            switch (result.Error)
            {
                case CommandError.BadArgCount:
                    var textError =
                        $"***Error.*** The input text has too few parameters.\n***Example:*** !{info.Value.Name}";
                    textError = info.Value.Parameters.Aggregate(textError,
                        (current, parameter) => current + $" {parameter} ");
                    await context.Message.ReplyAsync(textError);
                    break;
                case CommandError.UnknownCommand:
                case CommandError.ParseFailed:
                case CommandError.ObjectNotFound:
                case CommandError.MultipleMatches:
                case CommandError.UnmetPrecondition:
                case CommandError.Exception:
                case CommandError.Unsuccessful:
                default:
                    if(!string.IsNullOrEmpty(result.ErrorReason))
                        await context.Message.ReplyAsync(result.ErrorReason);
                    break;
            }
        }
    }
}