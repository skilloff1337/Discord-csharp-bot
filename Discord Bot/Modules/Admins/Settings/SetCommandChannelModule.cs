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
    public class SetCommandChannelModule: ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly Config _config;
        private readonly IJsonWriter<Config> _writer;

        public SetCommandChannelModule(DiscordSocketClient client, Config config, IJsonWriter<Config> writer)
        {
            _client = client;
            _config = config;
            _writer = writer;
        }

        [Command("setCommandChannel")]
        [Summary("[CMD_SUMMARY_SET_COMMAND_CHANNEL]")]
        public async Task SetCommandChannel(IMessageChannel channel)
        {
            var logChannel = await _client.GetChannelAsync(_config.ChannelIdForBotLog) as IMessageChannel;
            _config.ChannelIdForBotCommand = channel.Id;
            _writer.WriteData(_config);
            await ReplyAsync($"Admin channel changed to {channel}");
            await logChannel!.SendMessageAsync($"{Context.User.Mention} changed admin channel to {channel}");
        }
    }
}