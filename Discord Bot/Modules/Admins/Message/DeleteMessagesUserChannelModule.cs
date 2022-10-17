using System;
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
    public class DeleteMessagesUserChannelModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly ITranslation _translation;
        private readonly Config _config;


        private readonly Color _color = new(26, 148, 230);

        public DeleteMessagesUserChannelModule(DiscordSocketClient client, Config config,
            ITranslation translation)
        {
            _client = client;
            _config = config;
            _translation = translation;
        }

        [Command("deleteMessageUserInChannel")]
        [Summary("CMD_SUMMARY_CLEAR_USER_MESSAGE")]
        public async Task DeleteMessagesUserChannel(IUser user, IMessageChannel channel, int checkLastMessages = 100)
        {
            if (_client.GetChannel(_config.ChannelIdForBotLog) is not IMessageChannel logChannel)
            {
                Console.WriteLine($"Invalid channel ID for logs");
                return;
            }

            if (channel is null)
            {
                Console.WriteLine("Invalid channel");
                await Context.Message.ReplyAsync("Invalid channel");
                return;
            }

            var text =
                $"CMD_ADMINS_START_USER_IN_CHANNEL_DELETED_MESSAGES : {Context.User.Username}#{Context.User.Discriminator}\n" +
                $"CMD_CLEAN_FOR_USER : {user.Username}#{user.Discriminator}\n" +
                $"CMD_ADMINS_CHANNEL_NAME : {channel}\n" +
                $"CMD_ADMINS_CLEAN_CHECK_LAST_MESSAGES : {checkLastMessages}\n";

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithCurrentTimestamp()
                .WithDescription(text);

            await logChannel.SendMessageAsync(
                $"{Context.User.Mention} " +
                $"{_translation.GetTranslationByTextId("CMD_ADMINS_START_USER_IN_CHANNEL_DELETED_MESSAGES")}",
                embed: embed.Build());

            var countDeletedMessages = 0;
            var messages = channel.GetMessagesAsync(checkLastMessages);

            await foreach (var message in messages)
            {
                foreach (var msg in message)
                {
                    if (msg.Author != user)
                        continue;

                    await msg.DeleteAsync();

                    countDeletedMessages++;

                    await Task.Delay(1000);
                }
            }

            text = $"CMD_ADMINS_MESSAGES_DELETED : {countDeletedMessages}";
            embed.WithDescription(text);

            await logChannel.SendMessageAsync(
                $"{Context.User.Mention} " +
                $"{_translation.GetTranslationByTextId("CMD_ADMINS_END_USER_IN_CHANNEL_DELETED_MESSAGES")} {channel}",
                embed: embed.Build());
        }
    }
}