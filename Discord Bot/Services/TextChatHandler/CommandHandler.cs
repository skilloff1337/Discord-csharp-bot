using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Discord_Bot.Models;

namespace Discord_Bot.Services.TextChatHandler
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _command;
        private readonly IServiceProvider _provider;
        private readonly InteractionService _interaction;
        private readonly Config _config;

        public CommandHandler(CommandService command,
            DiscordSocketClient client,
            IServiceProvider provider,
            Config config, InteractionService interaction)
        {
            _command = command;
            _client = client;
            _provider = provider;
            _interaction = interaction;
            _config = config;
            
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

        private async Task Interaction(SocketInteraction interaction)
        {
            if(!RequiredServer(interaction.Channel))
            {
                await interaction.RespondAsync(
                    $"wrong channel, try in the channel - {_client.GetChannel(_config.ChannelIdForBotCommand)}",ephemeral:true);
                return;
            }
            
            var ctx = new SocketInteractionContext(_client, interaction);
            await _interaction.ExecuteCommandAsync(ctx, _provider);
        }

        private bool RequiredServer(ISocketMessageChannel channel)
            => channel == _client.GetChannel(_config.ChannelIdForBotCommand) 
               || channel == _client.GetChannel(_config.ChannelIdForBotAdminCommand);

        private async Task Ready()
        {
            await _command.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            await _interaction.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            await _interaction.RegisterCommandsToGuildAsync(_config.IdServer);
        }
    }
}