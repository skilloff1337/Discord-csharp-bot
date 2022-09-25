using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.Translation.Interfaces;
using ChannelType = Discord_Bot.Models.Types.ChannelType;

namespace Discord_Bot.Modules
{
    [RequiredChannel(ChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly ITranslation _translation;
        private readonly CommandService _commandService;
        private readonly DiscordSocketClient _client;

        private readonly Dictionary<string, CommandText> _commandText = new();

        private readonly ulong _idChannel;

        public AdminModule(IJsonReader<Config> configReader,
            ITranslation translation,
            IJsonReader<Dictionary<string, CommandText>> commandReader, CommandService commandService, DiscordSocketClient client)
        {
            _translation = translation;
            _commandService = commandService;
            _client = client;
            
            _idChannel = configReader.Load().ChannelIdForBotAdminCommand;  
            _commandText = commandReader.Load();
        }

        [Command("ban")]
        [Summary("test")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task BanUser([Summary("user")]IUser user, [Summary("days")]int days, [Summary("reason")]string reason)
        {
            if(Context.Channel != _client.GetChannel(_idChannel))
            {
                await ReplyAsync("Wrong channel!");
                return;
            }
            
            await ReplyAsync($"Ban user, **{user.Username}, for {days} day, reason: {reason}**!");
        }
    }
}