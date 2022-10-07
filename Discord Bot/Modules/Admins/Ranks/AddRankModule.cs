using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.DataWriter.Interfaces;
using MongoDB.Bson;

namespace Discord_Bot.Modules.Admins.Ranks
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class AddRankModule : ModuleBase<SocketCommandContext>
    {
        private readonly IJsonWriter<Config> _writer;
        private readonly Config _config;

        private readonly Dictionary<int, Rank> _listRanks;
        private readonly Color _color = new(26, 148, 230);

        public AddRankModule(Config config, IJsonWriter<Config> writer)
        {
            _writer = writer;
            _config = config;

            _listRanks = _config.Ranks;
        }

        [Command("addRank")]
        [Summary("CMD_SUMMARY_ADD_RANK")]
        public async Task AddRoleToRank(IRole role, uint needExp,byte level, [Remainder] string name)
        {
            if (Context.Guild.GetChannel(_config.ChannelIdForBotLog) is not IMessageChannel messageChannel)
            {
                Console.WriteLine("Invalid channel ID for logs");
                return;
            }

            var newRank = new Rank
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Level = level,
                NeedExp = needExp,
                RoleId = role.Id,
                NameRank = name,
            };

            if (!_config.Ranks.TryAdd(level, newRank))
            {
                await Context.Message.ReplyAsync($"ERROR! This level is already in use");
            }
            _writer.WriteData(_config);

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithCurrentTimestamp()
                .WithDescription(
                    $"Admin: {Context.User.Username}#{Context.User.Discriminator}\n" +
                    $"ID Rank: {newRank.Id}\n" +
                    $"Name Rank: {newRank.NameRank}\n" +
                    $"Level: {newRank.Level}\n" +
                    $"ID Role: {newRank.RoleId}\n" +
                    $"Role: {Context.Guild.GetRole(newRank.RoleId).Mention}\n" +
                    $"Need EXP: {newRank.NeedExp}");

            await messageChannel.SendMessageAsync($"{Context.User.Mention} Create new rank", embed: embed.Build());
        }
    }
}