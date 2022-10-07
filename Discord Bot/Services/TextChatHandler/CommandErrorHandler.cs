using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Models;

namespace Discord_Bot.Services.TextChatHandler
{
    public class CommandErrorHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly Config _config;

        public CommandErrorHandler(CommandService command, DiscordSocketClient client, Config config)
        {
            _client = client;
            _config = config;
            command.CommandExecuted += ErrorHandler;
        }

        private async Task ErrorHandler(Optional<CommandInfo> info, ICommandContext context, IResult result)
        {
            if (context.Channel != _client.GetChannel(_config.ChannelIdForBotCommand))
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