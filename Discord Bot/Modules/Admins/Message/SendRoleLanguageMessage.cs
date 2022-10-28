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
    public class SendRoleLanguageMessage: ModuleBase<SocketCommandContext>
    {
        [Command("sendRoleLanguageMessage")]
        [Summary("[CMD_SUMMARY_SEND_GIVING_LANGUAGE_ROLE_MESSAGE]")]
        public async Task SendRoleLanguage()
        {
            var chn = Context.Guild.GetChannel(1031459953124323328) as IMessageChannel;
            
            var text =
                "[RU] Добро пожаловать в дискорд **5mp.gg**\n" +
                "Здесь вы сможете следить за новостями проекта, серверов, мультиплеера.\n" +
                "Узнавать одним из первых, все новости. \n" +
                "**Для получения доступа к каналам, нажмите на эмодзи(:flag_ru:) под этим сообщением.**" +
                "\n\n\n" +
                "[EN] Welcome to the discord **5mp.gg**\n" +
                "Here you can follow the news of the project, servers, multiplayer.\n" +
                "Be the first to know all the news.\n" +
                "**To access the channels, click on the emoji(:flag_us:) below this message.**";
            
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