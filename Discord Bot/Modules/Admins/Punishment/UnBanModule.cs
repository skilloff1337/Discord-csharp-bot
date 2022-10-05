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
    public class UnBanModule: ModuleBase<SocketCommandContext>
    {
        private readonly ITranslation _translation;
        private readonly DiscordSocketClient _client;
        private readonly Config _config;

        public UnBanModule(ITranslation translation, Config config, DiscordSocketClient client)
        {
            _translation = translation;
            _config = config;
            _client = client;
        }
        
        [Command("unban")]
        [Summary("CMD_SUMMARY_UNBAN")]
        public async Task UnBanUser(IUser user)
        {
            var channel = await _client.GetChannelAsync(_config.ChannelIdForBotLog) as IMessageChannel;
            
            var text =
                $"CMD_ADMINS_ADMINISTRATOR  ***{Context.User.Username}*** CMD_ADMINS_UNBANNED " +
                $"***{user.Username}#{user.Discriminator}*** [ID:{user.Id}] ";

            text = _translation.TranslationText(text);
            
            await Context.Guild.RemoveBanAsync(user);
            await Context.Message.ReplyAsync(text);
            await channel!.SendMessageAsync(text);
        }

    }
}