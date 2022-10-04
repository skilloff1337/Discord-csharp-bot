using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models.Types;

namespace Discord_Bot.Modules.Admins.Message
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class SendMessageByBotModule : ModuleBase<SocketCommandContext>
    {
        [Command("send")]
        [Summary("CMD_SUMMARY_SEND_MESSAGE")]
        public async Task SendMessage(IMessageChannel channel, IRole role, [Remainder] string text)
        {
            await channel.SendMessageAsync($"{role.Mention} {text}");
        }
    }
}