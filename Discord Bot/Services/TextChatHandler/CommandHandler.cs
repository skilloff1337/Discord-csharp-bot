using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.TextChatHandler.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot.Services.TextChatHandler
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _command;
        private readonly IServiceProvider _provider;
        private readonly InteractionService _interaction;

        private readonly ulong _idBotChannel;
        private readonly ulong _idAdminCommandChannel;
        private readonly ulong _idServer;

        public CommandHandler(CommandService command,
            DiscordSocketClient client,
            IServiceProvider provider,
            IJsonReader<Config> jsonReader, InteractionService interaction)
        {
            _command = command;
            _client = client;
            _provider = provider;
            _interaction = interaction;
            _idBotChannel = jsonReader.Load().ChannelIdForBotCommand;
            _idAdminCommandChannel = jsonReader.Load().ChannelIdForBotAdminCommand;
            _idServer = jsonReader.Load().IdServer;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            _client.Ready += Ready;
            _client.InteractionCreated += Interaction;
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            if (!RequiredServer(messageParam.Channel))
                return;

            if (messageParam.Author.IsBot || messageParam is not SocketUserMessage message
                                          || message.Channel is IDMChannel)
                return;

            var argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) )
                || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
                return;

            var context = new SocketCommandContext(_client, message);

            await _command.ExecuteAsync(context, argPos, _provider);
        }

        private bool RequiredServer(ISocketMessageChannel channel)
            => channel == _client.GetChannel(_idBotChannel) 
               || channel == _client.GetChannel(_idAdminCommandChannel);

        private async Task Interaction(SocketInteraction interaction)
        {
            var ctx = new SocketInteractionContext(_client, interaction);
            await _interaction.ExecuteCommandAsync(ctx, _provider);
        }

        private async Task Ready()
        {
            await _command.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            await _interaction.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            await _interaction.RegisterCommandsToGuildAsync(305330911040372737);
        }
    }
}