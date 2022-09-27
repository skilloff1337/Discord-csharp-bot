using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        private readonly ITranslation _translation;
        private readonly CommandService _commandService;
        
        private readonly Color _color = new(26, 148, 230);

        public InfoModule(ITranslation translation, CommandService commandService)
        {
            _translation = translation;
            _commandService = commandService;
        }
        [Command("help")]
        public async Task Help()
        {

            var commandInfos = _commandService
                .Commands
                .Where(x => x.Module.Name != "AdminModule" && x.Module.Name != "AdminPunishmentModule");

            var result = new StringBuilder(500);
            foreach (var cmd in commandInfos)
            {
                result.Append($"**!{cmd.Name}** ");

                foreach (var param in cmd.Parameters)
                    result.Append($"{param} ");

                result.Append($" - {cmd.Summary}\n\n");
            }

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithDescription(_translation.TranslationText(result.ToString()));

            await Context.Message.ReplyAsync(_translation.GetTranslationByTextID("CMD_USER_COMMANDS"),
                embed: embed.Build());
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
    }
}