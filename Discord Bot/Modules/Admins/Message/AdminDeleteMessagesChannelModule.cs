using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.Translation.Interfaces;
using ChannelType = Discord_Bot.Models.Types.ChannelType;

namespace Discord_Bot.Modules.Admins.Message
{
    [Summary("Admin")]
    [RequiredChannel(ChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class AdminDeleteMessagesChannelModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly ITranslation _translation;


        private readonly Color _color = new(26, 148, 230);
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly ulong _idLogChat;

        public AdminDeleteMessagesChannelModule(DiscordSocketClient client, IJsonReader<Config> reader,
            ITranslation translation)
        {
            _client = client;
            _translation = translation;
            _idLogChat = reader.Load().ChannelIdForBotLog;
        }

        [Command("deleteMessageChannel")]
        [Summary("CMD_SUMMARY_CLEAR_ALL_MESSAGE")]
        public Task DeleteAllMessageChannel(ulong idChannel, int countMessages = 100)
        {
            if (countMessages == -1)
                countMessages = 100_000;
            
            _ = Task.Run(async () =>
            {
                if (_client.GetChannel(_idLogChat) is not IMessageChannel logChannel)
                {
                    Console.WriteLine($"Invalid channel ID for logs");
                    return;
                }
                if (_client.GetChannel(idChannel) is not IMessageChannel channel)
                {
                    Console.WriteLine($"Invalid channel ID for delete messages");
                    return;
                }

                await Context.Message.ReplyAsync(
                    $"{_translation.GetTranslationByTextID("CMD_ADMINS_START_DEL_MESSAGES")} " +
                    idChannel);
                _stopwatch.Restart();

                var messagesEnumerable = channel.GetMessagesAsync(countMessages);
                var count = 0;

                await foreach (var messages in messagesEnumerable)
                {
                    foreach (var msg in messages)
                    {
                        await msg.DeleteAsync();
                        count++;
                        await Task.Delay(1000);
                    }
                }

                var text = $"CMD_ADMINS_COUNT_DELETED_MESSAGES : {count}/{countMessages}\n" +
                           $"CMD_ADMINS_ELAPSED_DELETED_MESSAGES : {_stopwatch.Elapsed}\n" +
                           $"CMD_ADMINS_CHANNEL_ID : {idChannel}\n" +
                           $"CMD_ADMINS_START_DELETED_MESSAGES : {Context.User.Username}#{Context.User.Discriminator}\n";

                text = _translation.TranslationText(text);

                _stopwatch.Stop();
                var embed = new EmbedBuilder()
                    .WithColor(_color)
                    .WithCurrentTimestamp()
                    .WithDescription(text);
                

                await logChannel.SendMessageAsync(
                    $"{Context.User.Mention} {_translation.GetTranslationByTextID("CMD_ADMINS_END_DELETED_MESSAGES")}.",
                    embed: embed.Build());
            });
            return Task.CompletedTask;
        }
    }
}