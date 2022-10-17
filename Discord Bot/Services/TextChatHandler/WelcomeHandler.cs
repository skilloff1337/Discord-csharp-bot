using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.Models;

namespace Discord_Bot.Services.TextChatHandler
{
    public class WelcomeHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly Config _config;

        public WelcomeHandler(DiscordSocketClient client, Config config)
        {
            _client = client;
            _config = config;
            
            _client.MessageReceived += WelcomeHandlerAsync;
        }

        private async Task WelcomeHandlerAsync(SocketMessage messageParam)
        {
            var channel = _client.GetChannel(_config.ChannelIdForBotWelcome);
            if (messageParam.Channel != _client.GetChannel(_config.ChannelIdForBotWelcome))
                return;
            await messageParam.Author.SendMessageAsync("Welcome to my discord server dude!");
        }
    }
}