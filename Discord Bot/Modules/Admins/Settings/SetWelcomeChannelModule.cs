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
    public class SetWelcomeChannelModule: ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly Config _config;
        private readonly IJsonWriter<Config> _writer;

        public SetWelcomeChannelModule(DiscordSocketClient client, Config config, IJsonWriter<Config> writer)
        {
            _client = client;
            _config = config;
            _writer = writer;
        }

        [Command("setWelcomeChannel")]
        [Summary("[CMD_SUMMARY_SET_COMMAND_CHANNEL]")]
        public async Task SetCommandChannel(IMessageChannel channel)
        {
            if(channel != _client.GetGuild(_config.IdServer).SystemChannel)
            {
                await ReplyAsync($"The specified channel does not match the system channel.");
                return;
            }
            var logChannel = await _client.GetChannelAsync(_config.ChannelIdForBotLog) as IMessageChannel;
            
            _config.ChannelIdForBotWelcome = channel.Id;
            await ReplyAsync($"Welcome channel changed to {channel}");
            await logChannel!.SendMessageAsync($"{Context.User.Mention} changed welcome channel to {channel}");
        }
    }
}