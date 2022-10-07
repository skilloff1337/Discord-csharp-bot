using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.BadWords;
using Discord_Bot.Services.BadWords.Interfaces;

namespace Discord_Bot.Modules.Admins.BadWords
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class DelBadWordModule : ModuleBase<SocketCommandContext>
    {
        private readonly Config _config;
        private readonly IBadWords _badWords;

        public DelBadWordModule(Config config, IBadWords badWords)
        {
            _config = config;
            _badWords = badWords;
        }

        [Command("delBadWord")]
        [Summary("CMD_SUMMARY_DEL_BAD_WORD")]
        public async Task UpdateRank(string word)
        {
            var channel = Context.Guild.GetChannel(_config.ChannelIdForBotLog) as IMessageChannel;
            _badWords.DelWord(word);
            await channel.SendMessageAsync($"{Context.User.Mention} Deleted bad word **{word}**");
        }
    }
}