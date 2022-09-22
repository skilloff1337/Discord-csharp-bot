using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.TextChat.Interfaces;

namespace Discord_Bot.Services.TextChatHandler
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _command;
        private readonly IServiceProvider _provider;

        private readonly ulong _idBotChannel;

        public CommandHandler(CommandService command,
            DiscordSocketClient client,
            IServiceProvider provider,
            JsonConfigReader jsonReader)
        {
            _command = command;
            _client = client;
            _provider = provider;
            _idBotChannel = jsonReader.Load().ChannelIdForBotCommand;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            _command.CommandExecuted += OnCommandExecutedAsync;

            await _command.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        private async Task OnCommandExecutedAsync(Optional<CommandInfo> info, ICommandContext context, IResult result)
        {
            if (!string.IsNullOrEmpty(result?.ErrorReason))
            {
                Console.WriteLine($"{DateTime.Now:T} INFO \t{context.User.Username}#{context.User.Discriminator}" +
                                  $" write {result.ErrorReason} {context.Message}");
                await context.Message.ReplyAsync(result.ErrorReason);
            }
            else
            {
                Console.WriteLine($"{DateTime.Now:T} INFO \t{context.User.Username}#{context.User.Discriminator}" +
                                  $" write {context.Message}");
            }
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            if (messageParam.Channel != _client.GetChannel(_idBotChannel))
                return;

            if (messageParam.Author.IsBot || messageParam is not SocketUserMessage message
                                          || message.Channel is IDMChannel)
                return;

            var argPos = 0;

            if (!message.HasCharPrefix('!', ref argPos) 
                || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
                return;

            var context = new SocketCommandContext(_client, message);

            await _command.ExecuteAsync(context, argPos, _provider);
        }
    }
}