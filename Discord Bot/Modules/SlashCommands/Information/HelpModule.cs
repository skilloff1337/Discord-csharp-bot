using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord_Bot.Services.Translation.Interfaces;

namespace Discord_Bot.Modules.SlashCommands.Information
{
    public class HelpModule : InteractionModuleBase<SocketInteractionContext>
    {

        private readonly ITranslation _translation;
        private readonly InteractionService _interaction;
        
        private readonly Color _color = new(26, 148, 230);

        public HelpModule(ITranslation translation, InteractionService interaction)
        {
            _translation = translation;
            _interaction = interaction;
        }
        [SlashCommand("help", "Show commands")]
        public async Task Help()
        {
            var result = new StringBuilder(500);
            foreach (var module in _interaction.Modules)
            {
                foreach (var cmd in module.SlashCommands)
                {
                    result.Append($"**/{cmd.Name}** ");
                    
                    foreach (var parameter in cmd.Parameters)
                        result.Append($"{parameter.Name} ");

                    result.Append($" - {cmd.Description}\n\n");
                }
            }
            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithDescription(_translation.TranslationText(result.ToString()));

            await RespondAsync(_translation.GetTranslationByTextId("[CMD_USER_COMMANDS]"),
                embed: embed.Build());
        }
    }
}