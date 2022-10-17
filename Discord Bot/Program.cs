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
using Discord_Bot.Services.ReactionHandler;
using Discord_Bot.Services.TextChatHandler;
using Discord_Bot.Services.UserHandler;
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

            await Task.Delay(Timeout.Infinite);
        }


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task ServicesWakeUp(ServiceProvider provider)
        {
            provider.GetRequiredService<JsonConfigReader>();
            provider.GetRequiredService<AdminCommandErrorHandler>();
            provider.GetRequiredService<CommandErrorHandler>();
            provider.GetRequiredService<InteractionService>();
            provider.GetRequiredService<ReactionLanguageHandler>();
            provider.GetRequiredService<ReactionServerHandler>();
            provider.GetRequiredService<IRankHandler>();
            provider.GetRequiredService<CommandHandler>();
            provider.GetRequiredService<UserHandler>();
            provider.GetRequiredService<WelcomeHandler>();
            provider.GetRequiredService<BadWordsHandler>();
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
    }
}