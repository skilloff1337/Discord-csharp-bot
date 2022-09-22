using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Services;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.Translation.Interfaces;
using Console = System.Console;

namespace Discord_Bot.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        private readonly JsonConfigReader _configReader;
        private readonly JsonCommandTextReader _commandReader;
        private readonly ITranslation _translation;

        private Dictionary<string, string> _commandText;

        public InfoModule(JsonConfigReader configReader,
            ITranslation translation,
            JsonCommandTextReader commandReader)
        {
            _configReader = configReader;
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

            var text = _translation.TranslationText(result);

            await Context.Message.ReplyAsync(text);
        }

        [Command("hello")]
        public async Task Hello()
            => await ReplyAsync($"Hello there, **{Context.User.Username}**!");

        private void LoadCommandText()
        {
            _commandText = _commandReader.Load();
        }
    }
}