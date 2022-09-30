using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using ChannelType = Discord_Bot.Models.Types.ChannelType;

namespace Discord_Bot.Modules.Admins.Message
{
    [Summary("Admin")]
    [RequiredChannel(ChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class AdminSendMessageByBotModule : ModuleBase<SocketCommandContext>
    {
        [Command("send")]
        [Summary("CMD_SUMMARY_CLEAR_ALL_MESSAGE")]
        public Task SendMessage(IMessageChannel channel, IRole role, [Remainder] string text)
        {
            _ = Task.Run(async () =>
            {
                await channel.SendMessageAsync($"{role.Mention} {text}");
            });
            return Task.CompletedTask;
        }
    }
}