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
    [Discord.Commands.Summary("General")]
    public class InfoModule : InteractionModuleBase<SocketInteractionContext>
    {

        private readonly ITranslation _translation;
        private readonly CommandService _commandService;
        private readonly InteractionService _interaction;
        
        private readonly Color _color = new(26, 148, 230);

        public InfoModule(ITranslation translation, CommandService commandService, InteractionService interaction)
        {
            _translation = translation;
            _commandService = commandService;
            _interaction = interaction;
        }
        [SlashCommand("help", "show commands")]
        public async Task Help()
        {
            
            var test = _interaction;
            var result = new StringBuilder(500);
            foreach (var module in _interaction.Modules)
            {
                foreach (var cmd in module.SlashCommands)
                {
                    result.Append($"**!{cmd.Name}** ");
                    
                    foreach (var parameter in cmd.Parameters)
                        result.Append($"{parameter.Name} ");

                    result.Append($" - {cmd.Description}\n\n");
                }
            }
            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithDescription(_translation.TranslationText(result.ToString()));

            await RespondAsync(_translation.GetTranslationByTextID("CMD_USER_COMMANDS"),
                embed: embed.Build());
        }
    }
}