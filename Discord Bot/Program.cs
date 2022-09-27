using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Extension;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.DataReader.IoC.Extension;
using Discord_Bot.Services.TextChat.Interfaces;
using Discord_Bot.Services.TextChatHandler;
using Discord_Bot.Services.UserHandler.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot
{
    internal class Program
    {
        private DiscordSocketClient _client;
        private IJsonReader<Config> _reader;
        private ICommandHandler _command;
        private IUserHandler _userHandler;
        private IWelcomeHandler _welcomeHandler;


        private static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();


        private async Task MainAsync()
        {
            await using var services = ConfigureServices();
            
            _client = services.GetRequiredService<DiscordSocketClient>();
            _reader = services.GetRequiredService<IJsonReader<Config>>();
            _command = services.GetRequiredService<ICommandHandler>();
            _userHandler = services.GetRequiredService<IUserHandler>();
            _welcomeHandler = services.GetRequiredService<IWelcomeHandler>();
            var test = services.GetRequiredService<AdminCommandErrorHandler>();
            var test2 = services.GetRequiredService<CommandErrorHandler>();

            await _command.InstallCommandsAsync();
            await _userHandler.InstallEventsAsync();
            await _welcomeHandler.InstallCommandsAsync();
            
            _client.Log += Log;

            var token = _reader.Load().Token;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }



        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .BindingGeneral()
                .BindingReaders()
                .BuildServiceProvider();
        }
    }
}