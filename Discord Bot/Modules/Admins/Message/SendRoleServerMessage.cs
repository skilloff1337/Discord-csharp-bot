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
    public class SendRoleServerMessage : ModuleBase<SocketCommandContext>
    {
        [Command("sendRoleServerMessage")]
        [Summary("CMD_SUMMARY_SEND_GIVING_SERVER_ROLE_MESSAGE")]
        public async Task Test()
        {
            var chn = Context.Guild.GetChannel(1031459953124323328) as IMessageChannel;
            var emojiAlpha = Context.Guild.GetEmoteAsync(1031494505821646858).Result;
            var emojiBeta = Context.Guild.GetEmoteAsync(1031494309238804540).Result;
            var text =
                "[RU] Выбери сервер, на котором играешь!\n" +
                "\n\n\n" +
                "[EN] Choose the server you play on!\n\n" +
                $"{emojiAlpha} - Server Alpha №1\n"+
                $"{emojiBeta} - Server Beta №2\n";
            
            var embed = new EmbedBuilder()
                .WithColor(new Color(0, 96, 255))
                .WithCurrentTimestamp()
                .WithImageUrl("https://i.imgur.com/b2IEDmx.png")
                .WithDescription(text)
                .Build();
            
            if (chn != null) 
                await chn.SendMessageAsync(embed: embed);
        }
    }
}