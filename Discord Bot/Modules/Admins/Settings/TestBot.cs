using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;

namespace Discord_Bot.Modules.Admins.Settings
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class TestBot : ModuleBase<SocketCommandContext>
    {
        private DiscordSocketClient _client;
        private Config _config;

        public TestBot(DiscordSocketClient client, Config config)
        {
            _client = client;
            _config = config;
        }

        [Command("SendRoleServerMessage")]
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