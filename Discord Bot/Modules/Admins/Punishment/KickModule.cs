using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.Translation.Interfaces;

namespace Discord_Bot.Modules.Admins.Punishment
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.KickMembers)]
    [RequireBotPermission(GuildPermission.KickMembers)]
    public class KickModule : ModuleBase<SocketCommandContext>
    {
        private readonly ITranslation _translation;
        private readonly DiscordSocketClient _client;
        private readonly Config _config;

        public KickModule(ITranslation translation, Config config, DiscordSocketClient client)
        {
            _translation = translation;
            _config = config;
            _client = client;
        }

        [Command("kick")]
        [Summary("[CMD_SUMMARY_KICK]")]
        public async Task KickUser(SocketGuildUser user, string reason)
        {
            var channel = await _client.GetChannelAsync(_config.ChannelIdForBotLog) as IMessageChannel;
            var text =
                $"[CMD_ADMINS_ADMINISTRATOR] ***{Context.User.Username}*** [CMD_ADMINS_KICK] " +
                $"***{user.Username}#{user.Discriminator}*** [ID:{user.Id}], [CMD_ADMINS_REASON] : ***{reason}***";

            text = _translation.TranslationText(text);

            await user.KickAsync(text);
            await Context.Message.ReplyAsync(text);
            await channel!.SendMessageAsync(text);
        }
    }
}