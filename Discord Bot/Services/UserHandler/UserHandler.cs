using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Services.UserHandler.Interfaces;

namespace Discord_Bot.Services.UserHandler
{
    public class UserHandler : IUserHandler
    {
        private DiscordSocketClient _client;
        private readonly CommandService _command;
        private readonly IServiceProvider _provider;

        public UserHandler(DiscordSocketClient client, CommandService command, IServiceProvider provider)
        {
            _client = client;
            _command = command;
            _provider = provider;
        }
        
        public async Task InstallEventsAsync()
        {

            await Task.CompletedTask;
        }
        

    }
}