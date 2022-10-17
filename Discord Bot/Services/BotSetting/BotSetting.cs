using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.Models;
using Discord_Bot.Services.BotSetting.Interfaces;

namespace Discord_Bot.Services.BotSetting
{
    public class BotSetting : IBotSetting
    {
        private readonly DiscordSocketClient _client;
        private readonly Config _config;
        
        public BotSetting(Config config, DiscordSocketClient client)
        {
            _client = client;
            _config = config;
        }

        public async Task InstallSettingsAsync()
        {
            await _client.SetGameAsync(_config.GameName);
            await _client.SetStatusAsync(_config.Status);

            await Task.CompletedTask;
        }
        
    }
}