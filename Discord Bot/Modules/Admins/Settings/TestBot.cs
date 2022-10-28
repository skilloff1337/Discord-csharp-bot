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

        [Command("testt")]
        [Summary("[CMD_SUMMARY_SEND_GIVING_SERVER_ROLE_MESSAGE]")]
        public async Task Test()
        {

        }
    }
}