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
    [RequireUserPermission(GuildPermission.BanMembers)]
    [RequireBotPermission(GuildPermission.BanMembers)]
    public class BanModule : ModuleBase<SocketCommandContext>
    {
        private readonly ITranslation _translation;
        private readonly DiscordSocketClient _client;
        private readonly Config _config;
        
        private readonly Color _color = new(255, 0, 0);

        public BanModule(Config config, 
            ITranslation translation, DiscordSocketClient client)
        {
            _config = config;
            _translation = translation;
            _client = client;
        }

        [Command("ban")]
        [Summary("CMD_SUMMARY_BAN")]
        public async Task BanUser(IUser user, int days, string reason)
        {
            var channel = await _client.GetChannelAsync(_config.ChannelIdForBotLog) as IMessageChannel;
            
            var text =
                $"CMD_ADMINS_ADMINISTRATOR ***{Context.User.Username}*** CMD_ADMINS_BANNED " +
                $"***{user.Username}#{user.Discriminator}*** [ID:{user.Id}] " +
                $"CMD_ADMINS_FOR ***{days}*** CMD_ADMINS_DAYS, CMD_ADMINS_REASON : ***{reason}***";

            text = _translation.TranslationText(text);

            await Context.Guild.AddBanAsync(user, days, reason);
            await Context.Message.ReplyAsync(text);
            await channel!.SendMessageAsync(text);
        } 
    }
}