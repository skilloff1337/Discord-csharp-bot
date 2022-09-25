using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Services;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.Translation.Interfaces;
using ChannelType = Discord_Bot.Models.Types.ChannelType;
using Console = System.Console;

namespace Discord_Bot.Modules
{
    [RequiredChannel(ChannelType.BotCommand)]
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
     //   private readonly DiscordSocketClient _client;
        private readonly IJsonReader<Dictionary<string,CommandText>> _commandReader;
        private readonly ITranslation _translation;

        private Dictionary<string, CommandText> _commandText;
        

        public InfoModule(ITranslation translation, IJsonReader<Dictionary<string,CommandText>> commandReader)
        {
            _translation = translation;
            _commandReader = commandReader;
            LoadCommandText();
        }
        [Command("help")]
        public async Task Help()
        {
            if (!_commandText.TryGetValue("INFO_COMMAND_HELP", out var result))
            {
                Console.WriteLine("Error! Not found text for help command!");
                throw new Exception();
            }

            var text = _translation.TranslationText(result.TextCommand);

            await Context.Message.ReplyAsync(text);
        }
        [Command("test")]
        public async Task Test(IUser user)
        {
            await ReplyAsync($"test {user.Username}**!");
        }
        [Command("hello")]
        public async Task Hello()
        {
            await ReplyAsync($"BanUser there, **{Context.User.Username}**!");
        }

        private void LoadCommandText()
        {
            _commandText = _commandReader.Load();
        }
    }
}