using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;

namespace Discord_Bot.Modules.Admins.Ranks
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class ListRanksModule : ModuleBase<SocketCommandContext>
    {
        private readonly Config _config;
        private readonly Color _color = new(26, 148, 230);

        public ListRanksModule(Config config)
        {
            _config = config;
        }

        [Command("listRanks")]
        [Summary("CMD_SUMMARY_LIST_RANKS")]
        public async Task ListRanks()
        {
            var text = new StringBuilder(2000);

            foreach (var rank in _config.Ranks)
            {
                text.Append($"Name rank: {rank.Value.NameRank}\n" +
                            $"Level: {rank.Value.Level}\n" +
                            $"Exp: {rank.Value.NeedExp}\n" +
                            $"Role: {Context.Guild.GetRole(rank.Value.RoleId).Mention}\n" +
                            $"Role ID: {rank.Value.RoleId}\n" +
                            $"Rank ID: {rank.Value.Id}\n\n");
            }
            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithCurrentTimestamp()
                .WithDescription(text.ToString())
                .Build();

            await Context.Message.ReplyAsync("List ranks", embed: embed);
        }
    }
}