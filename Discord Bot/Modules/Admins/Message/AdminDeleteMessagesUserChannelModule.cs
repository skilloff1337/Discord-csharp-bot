﻿using System;
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
    public class AdminDeleteMessagesUserChannelModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly ITranslation _translation;


        private readonly Color _color = new(26, 148, 230);
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly ulong _idLogChat;

        public AdminDeleteMessagesUserChannelModule(DiscordSocketClient client, IJsonReader<Config> reader,
            ITranslation translation)
        {
            _client = client;
            _translation = translation;
            _idLogChat = reader.Load().ChannelIdForBotLog;
        }

        [Command("deleteMessageUserInChannel")]
        [Summary("CMD_SUMMARY_CLEAR_USER_MESSAGE")]
        public Task DeleteMessagesUserChannel(IUser user, IMessageChannel channel, int checkLastMessages = 100)
        {
            _ = Task.Run(async () =>
            {
                if (_client.GetChannel(_idLogChat) is not IMessageChannel logChannel)
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

                var text = $"CMD_ADMINS_START_USER_IN_CHANNEL_DELETED_MESSAGES : {Context.User.Username}#{Context.User.Discriminator}\n" +
                           $"CMD_CLEAN_FOR_USER : {user.Username}#{user.Discriminator}\n" +
                           $"CMD_ADMINS_CHANNEL_NAME : {channel}\n" +
                           $"CMD_ADMINS_CLEAN_CHECK_LAST_MESSAGES : {checkLastMessages}\n";

                var embed = new EmbedBuilder()
                    .WithColor(_color)
                    .WithCurrentTimestamp()
                    .WithDescription(text);

                await logChannel.SendMessageAsync(
                    $"{Context.User.Mention} " +
                    $"{_translation.GetTranslationByTextID("CMD_ADMINS_START_USER_IN_CHANNEL_DELETED_MESSAGES")}",
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
                    $"{_translation.GetTranslationByTextID("CMD_ADMINS_END_USER_IN_CHANNEL_DELETED_MESSAGES")} {channel}",
                    embed: embed.Build());
            });
            return Task.CompletedTask;
        }
    }
}