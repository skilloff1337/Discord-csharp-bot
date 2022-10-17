using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.Translation.Interfaces;

namespace Discord_Bot.Modules.Admins.Information
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class AdminListModule : ModuleBase<SocketCommandContext>
    {
        private readonly Config _config;
        private readonly DiscordSocketClient _client;

        private readonly Color _color = new(26, 148, 230);

        public AdminListModule(DiscordSocketClient client, Config config)
        {
            _client = client;
            _config = config;
        }

        [Command("admins")]
        [Summary("CMD_SUMMARY_ADMINS_LIST")]
        public async Task Admins()
        {
            var text = new StringBuilder(200);
            text.Append($"List of administrators:\n");
            foreach (var user in _config.AdministratorsID.Select(adminId => _client.GetUser(adminId)))
            {
                text.Append($"{user.Mention} \n");
            }

            await ReplyAsync(text.ToString());
        }
    }
}