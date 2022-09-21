using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Discord_Bot
{
    class Program
    {
        private DiscordSocketClient _client;
        private const string token = "MTAyMTc0NjIzNDM0MTQ3MDI0Mg.GIomAd.cLuQ-spPvNiiyUEBctGIQ1L-FiLgr1atyyLJBk";

        static void Main(string[] args) => new Program().MainAsync();


        private async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }
        
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}