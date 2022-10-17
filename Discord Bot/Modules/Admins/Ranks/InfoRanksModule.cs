using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.RankHandler.Interfaces;

namespace Discord_Bot.Modules.Admins.Ranks
{
    [Summary("Admin")] 
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class InfoRanksModule : ModuleBase<SocketCommandContext>
    {
        private readonly IRankHandler _rank;
        private readonly Config _config;

        public InfoRanksModule(IRankHandler rank, Config config)
        {
            _rank = rank;
            _config = config;
        }

        [Command("infoRanks")]
        [Summary("CMD_SUMMARY_LIST_USER_RANKS")]
        public async Task InfoRanks()
        {
            var countUser = _rank.CountUsers;
            var userInLevels = _rank.CountUserInLevels();
            var text = new StringBuilder(500);
            text.Append($"Total users in the system: {countUser}\n\n");
            text.Append($"[Level] | Name Level | Count\n");
            for (var i = 0; i < userInLevels.Length; i++)
            {
                text.Append($"[{i}] | {_config.Ranks[i].NameRank} | {userInLevels[i]}\n");
            }

            await ReplyAsync(text.ToString());
        }
    }
}