using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Discord_Bot.Extension;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.IoC.Extension;
using Discord_Bot.Services.DataWriter.IoC.Extension;
using Discord_Bot.Services.RankHandler.Interfaces;
using Discord_Bot.Services.RankHandler.IoC.Extension;
using Discord_Bot.Services.TextChatHandler;
using Discord_Bot.Services.TextChatHandler.Interfaces;
using Discord_Bot.Services.UserHandler.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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

            await Test();

            await Task.Delay(Timeout.Infinite);
        }

        private async Task Test()
        {

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
                .BindingRankSystem()
                .BuildServiceProvider();
        }

        private async Task ServicesWakeUp(ServiceProvider provider)
        {
            provider.GetRequiredService<JsonConfigReader>();
            provider.GetRequiredService<AdminCommandErrorHandler>();
            provider.GetRequiredService<CommandErrorHandler>();
            provider.GetRequiredService<InteractionService>();
            await provider.GetRequiredService<IRankHandler>().InstallEventsAsync();
            await provider.GetRequiredService<ICommandHandler>().InstallCommandsAsync();
            await provider.GetRequiredService<IUserHandler>().InstallEventsAsync();
            await provider.GetRequiredService<IWelcomeHandler>().InstallCommandsAsync();
            await provider.GetRequiredService<IBadWordsHandler>().Install();
        }
    }
}