using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.Translation.Interfaces;

namespace Discord_Bot.Modules.Admins.Information
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class AdminHelpModule: ModuleBase<SocketCommandContext>
    {
        private readonly ITranslation _translation;
        private readonly CommandService _commandService;

        private readonly Color _color = new(26, 148, 230);

        public AdminHelpModule(ITranslation translation,
            CommandService commandService)
        {
            _translation = translation;
            _commandService = commandService;
        }

        [Command("help")]
        [Summary("CMD_SUMMARY_HELP")]
        public async Task Help()
        {
            var commandInfos = _commandService
                .Commands
                .Where(x => x.Module.Summary is "Admin");

            var result = new StringBuilder(500);
            foreach (var cmd in commandInfos)
            {
                result.Append($"**!{cmd.Name}** ");

                foreach (var param in cmd.Parameters)
                    result.Append($"[{param}] ");

                result.Append($" - {cmd.Summary}\n\n");
            }

            var text = _translation.TranslationText(result.ToString());
            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithDescription(text)
                .Build();

            await Context.Message.ReplyAsync(_translation.GetTranslationByTextId("CMD_ADMINS_COMMANDS"),
                embed: embed);
        }
    }
}