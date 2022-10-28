using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord_Bot.Models;
using Discord_Bot.Services.RankHandler.Interfaces;
using Discord_Bot.Services.Translation.Interfaces;

namespace Discord_Bot.Modules.SlashCommands.Information
{
    public class MyRankModule : InteractionModuleBase<SocketInteractionContext>
    {
        private Config _config;
        private readonly IRankHandler _rankHandler;

        private readonly Color _color = new(26, 148, 230);

        public MyRankModule(IRankHandler rankHandler, Config config)
        {
            _rankHandler = rankHandler;
            _config = config;
        }

        [SlashCommand("myrank", "Show information for your rank")]
        public async Task MyRankCmd()
        {
            var user = _rankHandler.GetUser(Context.User.Id);
            if (user is null)
            {
                await RespondAsync("Error, you don't have a rank. Write any message in text chat to get a new rank.");
                return;
            }

            var level = user.Level;
            var text =
                $"Current level: {level}\n" +
                $"Current exp: {user.CurrentExp}\n" +
                $"Current rank role: {_config.Ranks[user.Level].NameRank}\n\n" +
                $"Next level:\n\n";

            var textNextLevel = level == 10
                ? "Your level is max."
                : $"Need exp: {_config.Ranks[level + 1].NeedExp}\n" +
                  $"Rank name: {_config.Ranks[level + 1].NameRank}\n" +
                  $"Rank role: {Context.Guild.GetRole(_config.Ranks[level + 1].RoleId)}";

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithCurrentTimestamp()
                .WithDescription(text + textNextLevel)
                .WithImageUrl(Context.User.GetAvatarUrl())
                .Build();

            await RespondAsync("Information for your rank", embed: embed);
        }
    }
}