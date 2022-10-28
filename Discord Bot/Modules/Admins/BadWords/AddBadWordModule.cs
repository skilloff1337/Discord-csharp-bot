using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.BadWords.Interfaces;

namespace Discord_Bot.Modules.Admins.BadWords
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class AddBadWordModule : ModuleBase<SocketCommandContext>
    {
        private readonly Config _config;
        private readonly IBadWords _badWords;

        public AddBadWordModule(Config config, IBadWords badWords)
        {
            _config = config;
            _badWords = badWords;
        }

        [Command("addBadWord")]
        [Summary("[CMD_SUMMARY_ADD_BAD_WORD]")]
        public async Task UpdateRank(string word)
        {
            var channel = Context.Guild.GetChannel(_config.ChannelIdForBotLog) as IMessageChannel;
            _badWords.AddNewWord(word);
            if (channel != null)
                await channel.SendMessageAsync($"{Context.User.Mention} Added new bad word **{word}**");
        }
    }
}