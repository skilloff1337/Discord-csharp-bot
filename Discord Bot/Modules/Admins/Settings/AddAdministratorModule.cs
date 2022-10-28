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
    public class AddAdministratorModule: ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly Config _config;
        private readonly IJsonWriter<Config> _writer;

        public AddAdministratorModule(DiscordSocketClient client, Config config, IJsonWriter<Config> writer)
        {
            _client = client;
            _config = config;
            _writer = writer;
        }

        [Command("addAdmin")]
        [Summary("[CMD_SUMMARY_SET_ADMIN_CHANNEL]")]
        public async Task SetAdminChannel(IUser user)
        {
            var logChannel = await _client.GetChannelAsync(_config.ChannelIdForBotLog) as IMessageChannel;
            _config.AdministratorsID.Add(user.Id);
            _writer.WriteData(_config);
            await ReplyAsync($"Add admin to system.");
            await logChannel!.SendMessageAsync($"{Context.User.Mention} add admin {user.Mention} to system");
        }
    }
}