using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.Translation.Interfaces;

namespace Discord_Bot.Modules.Admins.Message
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class DeleteMessagesUserModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly ITranslation _translation;
        private readonly Config _config;


        private readonly Color _color = new(26, 148, 230);

        public DeleteMessagesUserModule(DiscordSocketClient client, Config config,
            ITranslation translation)
        {
            _client = client;
            _config = config;
            _translation = translation;
        }

        [Command("deleteMessageUser")]
        [Summary("[CMD_SUMMARY_CLEAR_USER_MESSAGE]")]
        public async Task DeleteMessagesUser(IUser user, int checkLastMessages = 100)
        {
            if (_client.GetChannel(_config.ChannelIdForBotLog) is not IMessageChannel logChannel)
            {
                Console.WriteLine($"Invalid channel ID for logs");
                return;
            }

            var channels = Context.Guild.TextChannels;

            var allMessages = checkLastMessages * channels.Count;

            var text =
                $"[CMD_ADMINS_START_DELETED_MESSAGES] : {Context.User.Username}#{Context.User.Discriminator}\n" +
                $"[CMD_CLEAN_FOR_USER] : {user.Username}#{user.Discriminator}\n" +
                $"[CMD_ADMINS_CHANNELS_IN_CLEAN] : {channels.Count}\n";

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithCurrentTimestamp()
                .WithDescription(text);

            await logChannel.SendMessageAsync(
                $"{Context.User.Mention} " +
                $"{_translation.GetTranslationByTextId("[CMD_ADMINS_START_USER_DELETED_MESSAGES]")}.",
                embed: embed.Build());

            var allDeletedMessages = 0;
            var allMsg = 0;
            foreach (var t in channels)
            {
                if (t is not IMessageChannel channel)
                    continue;
                var messages = channel.GetMessagesAsync(checkLastMessages);
                var countInCurrentChannel = 0;
                Console.WriteLine(channel);
                await foreach (var message in messages)
                {
                    foreach (var msg in message)
                    {
                        if (msg.Author != user)
                            continue;

                        countInCurrentChannel++;
                        await msg.DeleteAsync();
                        await Task.Delay(1000);
                    }
                }

                allMsg += await messages.CountAsync();
                allDeletedMessages += countInCurrentChannel;
                await logChannel.SendMessageAsync(
                    $"{Context.User.Mention} [CMD_ADMINS_END_DELETED_CHANNEL_MESSAGES] {channel}," +
                    $" [CMD_ADMINS_MESSAGES_DELETED] {user.Username} : {countInCurrentChannel}");
                await Task.Delay(1000);
            }

            text = $"[CMD_ADMINS_START_DELETED_MESSAGES] : {user.Username}#{user.Discriminator}\n" +
                   $"[CMD_ADMINS_CHANNELS_IN_CLEAN] : {channels.Count}\n" +
                   $"[CMD_ADMINS_VIEWED_MESSAGES] : {allMsg}\n" +
                   $"[CMD_ADMINS_COUNT_DELETED_MESSAGES] : {allDeletedMessages}";

            embed.WithDescription(text);


            await logChannel.SendMessageAsync(
                $"{Context.User.Mention} {_translation.GetTranslationByTextId("[CMD_ADMINS_END_USER_DELETED_MESSAGES]")}.",
                embed: embed.Build());
        }
    }
}