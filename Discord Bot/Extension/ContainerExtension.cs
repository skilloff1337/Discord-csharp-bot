using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Services.BotSetting;
using Discord_Bot.Services.DataBase;
using Discord_Bot.Services.DataBase.Interfaces;
using Discord_Bot.Services.TextChat.Interfaces;
using Discord_Bot.Services.TextChatHandler;
using Discord_Bot.Services.Translation;
using Discord_Bot.Services.Translation.Interfaces;
using Discord_Bot.Services.UserHandler;
using Discord_Bot.Services.UserHandler.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot.Extension
{
    public static class ContainerExtension
    {
        public static IServiceCollection BindingGeneral(this IServiceCollection collection)
        {
            return collection
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    AlwaysDownloadUsers = true,
                    MessageCacheSize = 1000,
                    GatewayIntents = GatewayIntents.All
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig()))
                .AddSingleton<ICommandHandler, CommandHandler>()
                .AddSingleton<IUserHandler, UserHandler>()
                .AddSingleton<IBotSetting, BotSetting>()
                .AddSingleton<IWelcomeHandler, WelcomeHandler>()
                .AddSingleton<ITranslation, Translation>()
                .AddSingleton<AdminCommandErrorHandler>()
                .AddSingleton<CommandErrorHandler>()
                .AddSingleton<IRepository<MessageUser>, MessageRepository>();
        }
    }
}