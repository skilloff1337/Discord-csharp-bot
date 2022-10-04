using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.Translation.Interfaces;

namespace Discord_Bot.Modules.Admins.Message
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class DeleteMessagesChannelModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly ITranslation _translation;
        private readonly Config _config;


        private readonly Color _color = new(26, 148, 230);
        private readonly Stopwatch _stopwatch = new();

        public DeleteMessagesChannelModule(DiscordSocketClient client, Config config,
            ITranslation translation)
        {
            _client = client;
            _config = config;
            _translation = translation;
        }

        [Command("test")]
        [Summary("CMD_SUMMARY_CLEAR_ALL_MESSAGE")]
        public async Task Test(string name)
        {
            for (var i = 0; i < 50; i++)
            {
                await Context.Message.ReplyAsync($"{name} : {i}");
                Console.WriteLine($"{name} : {i}");
                await Task.Delay(1000);
            }
        }

        [Command("deleteMessageChannel")]
        [Summary("CMD_SUMMARY_CLEAR_ALL_MESSAGE")]
        public async Task DeleteAllMessageChannel(IMessageChannel channel, int countMessages = 100)
        {
            if (_client.GetChannel(_config.ChannelIdForBotLog) is not IMessageChannel logChannel)
            {
                Console.WriteLine($"Invalid channel ID for logs");
                return;
            }

            await logChannel.SendMessageAsync(
                $"{_translation.GetTranslationByTextID("CMD_ADMINS_START_DEL_MESSAGES")} " +
                channel.Id);
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

            _stopwatch.Stop();

            var text = $"CMD_ADMINS_COUNT_DELETED_MESSAGES : {count}/{countMessages}\n" +
                       $"CMD_ADMINS_ELAPSED_DELETED_MESSAGES : {_stopwatch.Elapsed}\n" +
                       $"CMD_ADMINS_CHANNEL_ID : {channel.Id}\n" +
                       $"CMD_ADMINS_CHANNEL_NAME : {channel.Name}\n" +
                       $"CMD_ADMINS_START_DELETED_MESSAGES : {Context.User.Username}#{Context.User.Discriminator}\n";

            text = _translation.TranslationText(text);

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithCurrentTimestamp()
                .WithDescription(text);


            await logChannel.SendMessageAsync(
                $"{Context.User.Mention} {_translation.GetTranslationByTextID("CMD_ADMINS_END_DELETED_MESSAGES")}.",
                embed: embed.Build());
        }
    }
}