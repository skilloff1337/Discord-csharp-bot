using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Extension;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.DataReader.IoC.Extension;
using Discord_Bot.Services.DataWriter.Interfaces;
using Discord_Bot.Services.DataWriter.IoC.Extension;
using Discord_Bot.Services.TextChatHandler;
using Discord_Bot.Services.TextChatHandler.Interfaces;
using Discord_Bot.Services.UserHandler.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace Discord_Bot
{
    internal class Program
    {
        private static async Task Main(string[] args) => await new Program().MainAsync();


        private async Task MainAsync()
        {
            await using var provider = ConfigureServices();
            await ServicesWakeUp(provider);
            var config = provider.GetRequiredService<Config>();
            var client = provider.GetRequiredService<DiscordSocketClient>();

            client.Log += Log;

            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .BindingGeneral()
                .BindingReaders()
                .BindingWriters()
                .BuildServiceProvider();
        }

        private async Task ServicesWakeUp(ServiceProvider provider)
        {
            provider.GetRequiredService<JsonConfigReader>();
            provider.GetRequiredService<AdminCommandErrorHandler>();
            provider.GetRequiredService<CommandErrorHandler>();
            provider.GetRequiredService<InteractionService>();
            await provider.GetRequiredService<ICommandHandler>().InstallCommandsAsync();
            await provider.GetRequiredService<IUserHandler>().InstallEventsAsync();
            await provider.GetRequiredService<IWelcomeHandler>().InstallCommandsAsync();
        }
    }
}