using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.DataWriter.Interfaces;

namespace Discord_Bot.Modules.Admins.Settings
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class SetNewNickNameBot : ModuleBase<SocketCommandContext>
    {
        private DiscordSocketClient _client;
        private Config _config;

        public SetNewNickNameBot(DiscordSocketClient client, Config config)
        {
            _client = client;
            _config = config;
        }

        [Command("setBotName")]
        [Summary("CMD_SUMMARY_BOT_NEW_NAME")]
        public async Task SetBotName([Remainder] string newName)
        {
            var bot = _client.GetGuild(_config.IdServer).GetUser(1021746234341470242);
            await bot.ModifyAsync(x => { x.Nickname = newName; });
        }
    }
}