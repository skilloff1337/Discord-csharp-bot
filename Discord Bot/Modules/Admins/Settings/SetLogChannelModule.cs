using System.Reflection;
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
    public class SetLogChannelModule: ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly Config _config;
        private readonly IJsonWriter<Config> _writer;

        public SetLogChannelModule(DiscordSocketClient client, Config config, IJsonWriter<Config> writer)
        {
            _client = client;
            _config = config;
            _writer = writer;
        }

        [Command("setLogChannel")]
        [Summary("CMD_SUMMARY_SET_LOG_CHANNEL")]
        public async Task SetLogChannel(IMessageChannel channel)
        {
            _config.ChannelIdForBotLog = channel.Id;
            _writer.WriteData(_config);
            await ReplyAsync($"Log channel changed to {channel}");
            await channel!.SendMessageAsync($"{Context.User.Mention} changed log channel to {channel}");
        }
    }
}