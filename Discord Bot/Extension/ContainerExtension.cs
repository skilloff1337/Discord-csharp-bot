using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Services.BadWords;
using Discord_Bot.Services.BotSetting;
using Discord_Bot.Services.BotSetting.Interfaces;
using Discord_Bot.Services.DataBase;
using Discord_Bot.Services.DataBase.Interfaces;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.PathProvider;
using Discord_Bot.Services.PathProvider.Interfaces;
using Discord_Bot.Services.TextChatHandler;
using Discord_Bot.Services.TextChatHandler.Interfaces;
using Discord_Bot.Services.Translation;
using Discord_Bot.Services.Translation.Interfaces;
using Discord_Bot.Services.UserHandler;
using Discord_Bot.Services.UserHandler.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RunMode = Discord.Commands.RunMode;

namespace Discord_Bot.Extension
{
    public static class ContainerExtension
    {
        public static IServiceCollection BindingGeneral(this IServiceCollection collection)
        {
            return collection
                .AddSingleton<Config>()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    AlwaysDownloadUsers = true,
                    MessageCacheSize = 1000,
                    GatewayIntents = GatewayIntents.All
                }))
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton(new CommandService(new CommandServiceConfig()
                {
                    DefaultRunMode = RunMode.Async
                }))
                .AddSingleton<ICommandHandler, CommandHandler>()
                .AddSingleton<IUserHandler, UserHandler>()
                .AddSingleton<IBotSetting, BotSetting>()
                .AddSingleton<IWelcomeHandler, WelcomeHandler>()
                .AddSingleton<ITranslation, Translation>()
                .AddSingleton<AdminCommandErrorHandler>()
                .AddSingleton<CommandErrorHandler>()
                .AddSingleton<IRepository<MessageUser>, MessageRepository>()
                .AddSingleton<IPathProvider, PathProvider>()
                .AddSingleton<IBadWordsHandler, BadWordsHandler>()
                .AddSingleton<IBadWords, BadWords>();
        }
    }
}