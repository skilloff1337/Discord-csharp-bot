using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using Discord_Bot.Models.Types;

namespace Discord_Bot.Modules.Admins.Message
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class SendPrivateMessageByBotModule: ModuleBase<SocketCommandContext>
    {
        [Command("sendPrivate")]
        [Summary("[CMD_SUMMARY_SEND_PRIVATE_MESSAGE]")]
        public async Task SendMessage(IUser user, [Remainder] string text)
        {
            await user.SendMessageAsync(text);
        }
    }
}