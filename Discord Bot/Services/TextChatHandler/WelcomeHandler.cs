using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.TextChatHandler.Interfaces;

namespace Discord_Bot.Services.TextChatHandler
{
    public class WelcomeHandler : IWelcomeHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly Config _config;

        public WelcomeHandler(DiscordSocketClient client, Config config)
        {
            _client = client;
            _config = config;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += WelcomeHandlerAsync;
            await Task.CompletedTask;
        }

        private async Task WelcomeHandlerAsync(SocketMessage messageParam)
        {
            var channel = _client.GetChannel(_config.ChannelIdForBotWelcome);
            if (messageParam.Channel != _client.GetChannel(_config.ChannelIdForBotWelcome))
                return;
            await messageParam.Author.SendMessageAsync("Welcome to my discord server dude!");
        }
    }
}