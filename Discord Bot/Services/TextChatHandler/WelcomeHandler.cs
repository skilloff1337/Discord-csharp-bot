using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.Interfaces;

namespace Discord_Bot.Services.TextChatHandler
{
    public class WelcomeHandler : IWelcomeHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _command;
        private readonly IServiceProvider _provider;

        private readonly ulong _idBotChannel;

        public WelcomeHandler(CommandService command,
            DiscordSocketClient client,
            IServiceProvider provider,
            IJsonReader<Config> jsonReader)
        {
            _command = command;
            _client = client;
            _provider = provider;
            _idBotChannel = jsonReader.Load().ChannelIdForBotWelcome;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += WelcomeHandlerAsync;
            await Task.CompletedTask;
        }

        private async Task WelcomeHandlerAsync(SocketMessage messageParam)
        {
            var channel = _client.GetChannel(_idBotChannel);
            if (messageParam.Channel != _client.GetChannel(_idBotChannel))
                return;
            await messageParam.Author.SendMessageAsync("Welcome to my discord server dude!");
        }
    }
}