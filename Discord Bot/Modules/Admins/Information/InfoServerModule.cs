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
    public class InfoServerModule : ModuleBase<SocketCommandContext>
    {
        private readonly ITranslation _translation;

        private readonly Color _color = new(26, 148, 230);

        public InfoServerModule(ITranslation translation)
        {
            _translation = translation;
        }

        [Command("infoServer")]
        [Summary("CMD_SUMMARY_SERVER_INFO")]
        public async Task InfoServer()
        {
            var text = $"**CMD_ADMINS_SERVER_NAME :** {Context.Guild.Name}\n" +
                       $"**CMD_ADMINS_SERVER_ID :** {Context.Guild.Id}\n" +
                       $"**CMD_ADMINS_SERVER_CREATED :** {Context.Guild.CreatedAt:d}\n" +
                       $"**CMD_ADMINS_SERVER_OWNER :** {Context.Guild.Owner}\n" +
                       $"**CMD_ADMINS_SERVER_USERS :** {Context.Guild.MemberCount}\n" +
                       $"**CMD_ADMINS_SERVER_CHANNELS :** {Context.Guild.Channels.Count}\n" +
                       $"**CMD_ADMINS_SERVER_ROLES :** {Context.Guild.Roles.Count}\n" +
                       $"**CMD_ADMINS_SERVER_EMOTES : ** {Context.Guild.Emotes.Count}\n\n" +
                       $"**CMD_ADMINS_SERVER_SECURITY :** {Context.Guild.VerificationLevel}";

            text = _translation.TranslationText(text);

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithImageUrl(Context.Guild.IconUrl)
                .WithDescription(text);

            await Context.Message.ReplyAsync($"{_translation.GetTranslationByTextId("CMD_ADMINS_SERVER_INFO")}" +
                                             $" **{Context.Guild.Name}**", embed: embed.Build());
        }
    }
}