using System.Linq;
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
    [RequireUserPermission(GuildPermission.MuteMembers)]
    [RequireBotPermission(GuildPermission.MuteMembers)]
    public class UnMuteModule: ModuleBase<SocketCommandContext>
    {
        private readonly ITranslation _translation;
        private readonly DiscordSocketClient _client;
        private readonly Config _config;

        public UnMuteModule(Config config, 
            ITranslation translation, DiscordSocketClient client)
        {
            _config = config;
            _translation = translation;
            _client = client;
        }
        
        [Command("unmute")]
        [Summary("[CMD_SUMMARY_UNMUTE]")]
        public async Task UnMuteUser(SocketGuildUser user)
        {
            var channel = await _client.GetChannelAsync(_config.ChannelIdForBotLog) as IMessageChannel;
            
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Mute");
            var text =
                $"[CMD_ADMINS_ADMINISTRATOR] ***{Context.User.Username}*** [CMD_ADMINS_UNMUTE] " +
                $"***{user.Username}#{user.Discriminator}*** [ID:{user.Id}]";

            text = _translation.TranslationText(text);
            
            await user.RemoveTimeOutAsync();
            await Context.Message.ReplyAsync(text);
            await channel!.SendMessageAsync(text);
        }
    }
}