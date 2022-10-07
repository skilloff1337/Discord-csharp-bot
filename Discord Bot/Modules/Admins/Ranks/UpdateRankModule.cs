using System;
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
    public class UpdateRankModule : ModuleBase<SocketCommandContext>
    {
        private readonly IJsonWriter<Config> _writer;
        private readonly Config _config;
        
        private readonly Color _color = new(26, 148, 230);

        public UpdateRankModule(Config config, IJsonWriter<Config> writer)
        {
            _writer = writer;
            _config = config;
        }

        [Command("updateRank")]
        [Summary("CMD_SUMMARY_UPDATE_RANK")]
        public async Task UpdateRank(int levelRank, IRole role, uint needExp, [Remainder] string name)
        {
            if (Context.Guild.GetChannel(_config.ChannelIdForBotLog) is not IMessageChannel messageChannel)
            {
                Console.WriteLine("Invalid channel ID for logs");
                return;
            }

            if (!_config.Ranks.TryGetValue(levelRank, out var result))
            {
                await Context.Message.ReplyAsync($"No rank found for the specified level");
                return;
            }
            
            result.RoleId = role.Id;
            result.NeedExp = needExp;
            result.NameRank = name;
            
            _config.Ranks[result.Level] = result;
            _writer.WriteData(_config);
            
            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithCurrentTimestamp()
                .WithDescription(
                    $"Admin: {Context.User.Username}#{Context.User.Discriminator}\n" +
                    $"ID Rank: {result.Id}\n" +
                    $"Name Rank: {result.NameRank}\n" +
                    $"Level: {result.Level}\n" +
                    $"ID Role: {result.RoleId}\n" +
                    $"Role: {Context.Guild.GetRole(result.RoleId).Mention}\n" +
                    $"Need EXP: {result.NeedExp}");

            await messageChannel.SendMessageAsync($"{Context.User.Mention} update rank", embed: embed.Build());
        }

    }
}