using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.BadWords;

namespace Discord_Bot.Modules.Admins.BadWords
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class ListBadWordsModule : ModuleBase<SocketCommandContext>
    {
        private readonly Config _config;
        private readonly IBadWords _badWords;

        public ListBadWordsModule(Config config, IBadWords badWords)
        {
            _config = config;
            _badWords = badWords;
        }

        [Command("listBadWords")]
        [Summary("CMD_SUMMARY_LIST_BAD_WORD")]
        public async Task ListBadWords()
        {
            var text = new StringBuilder(500);
            text.Append($"{Context.User.Mention} list bad words: \n");
            foreach (var word in _badWords.GetWords)
            {
                text.Append($"{word}\n");
            }
        }
    }
}