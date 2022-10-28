using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.DataWriter.Interfaces;

namespace Discord_Bot.Modules.Admins.Ranks
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class DeleteRankModule : ModuleBase<SocketCommandContext>
    {
        private readonly IJsonWriter<Config> _writer;
        private readonly Config _config;

        private readonly Dictionary<int, Rank> _listRanks;
        private readonly Color _color = new(26, 148, 230);

        public DeleteRankModule(Config config, IJsonWriter<Config> writer)
        {
            _writer = writer;
            _config = config;

            _listRanks = _config.Ranks;
        }

        [Command("deleteRank")]
        [Summary("[CMD_SUMMARY_DEL_RANK]")]
        public async Task DeleteRank(byte level)
        {
            if (Context.Guild.GetChannel(_config.ChannelIdForBotLog) is not IMessageChannel messageChannel)
            {
                Console.WriteLine("Invalid channel ID for logs");
                return;
            }

            if (!_config.Ranks.TryGetValue(level, out var result))
            {
                await Context.Message.ReplyAsync("No rank found by level.");
                return;
            }

            _config.Ranks.Remove(level);
            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithCurrentTimestamp()
                .WithDescription($"Admin: {Context.User.Username}#{Context.User.Discriminator}\n" +
                                 $"ID Rank: {result.Id}\n" +
                                 $"Name Rank: {result.NameRank}\n" +
                                 $"Level: {result.Level}\n" +
                                 $"ID Role: {result.RoleId}\n" +
                                 $"Role: {Context.Guild.GetRole(result.RoleId).Mention}\n" +
                                 $"Need EXP: {result.NeedExp}")
                .Build();
            await messageChannel.SendMessageAsync($"{Context.User.Mention} Delete rank",embed: embed);
        }
    }
}