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
    public class SetLanguageModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly Config _config;
        private readonly IJsonWriter<Config> _writer;

        public SetLanguageModule(DiscordSocketClient client, Config config, IJsonWriter<Config> writer)
        {
            _client = client;
            _config = config;
            _writer = writer;
        }

        [Command("setLanguage")]
        [Summary("[CMD_SUMMARY_SET_LANGUAGE]")]
        public async Task SetLanguage(LanguageType language)
        {
            var channel = await _client.GetChannelAsync(_config.ChannelIdForBotLog) as IMessageChannel;
            _config.Language = language;
            _writer.WriteData(_config);
            await ReplyAsync($"Language changed to {language}");
            await channel!.SendMessageAsync($"{Context.User.Mention} changed language to {language}");
        }
    }
}