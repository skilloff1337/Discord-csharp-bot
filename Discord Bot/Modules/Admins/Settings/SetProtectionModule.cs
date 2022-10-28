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
    public class SetProtectionModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly Config _config;
        private IJsonWriter<Config> _writer;
        public SetProtectionModule(IJsonWriter<Config> writer, Config config, DiscordSocketClient client)
        {
            _writer = writer;
            _config = config;
            _client = client;
        }
        
        [Command("setProtectionBadWords")]
        [Summary("[CMD_SUMMARY_SET_BAD_WORDS]")]
        public async Task SetProtectionChannel(bool value)
        {
            var logChannel = await _client.GetChannelAsync(_config.ChannelIdForBotLog) as IMessageChannel;
            _config.EnableWordProtection = value;
            _writer.WriteData(_config);
            await ReplyAsync($"Protection bad words changed to {value}");
            await logChannel!.SendMessageAsync($"{Context.User.Mention} changed protection bad words to {value}");
        }
    }
}