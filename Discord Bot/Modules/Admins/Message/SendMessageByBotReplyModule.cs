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
    public class SendMessageByBotReplyModule : ModuleBase<SocketCommandContext>
    {
        [Command("sendreply")]
        [Summary("CMD_SUMMARY_SEND_REPLY_MESSAGE")]
        public async Task SendMessage(IMessageChannel channel, ulong idMessage, [Remainder] string text)
        {
            var message = await channel.GetMessageAsync(idMessage) as SocketUserMessage;
            await message.ReplyAsync($"{message!.Author.Mention} {text}");
        }
    }
}