using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.Translation.Interfaces;

namespace Discord_Bot.Modules.Admins.Information
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class InfoRoleModule : ModuleBase<SocketCommandContext>
    {
        private readonly ITranslation _translation;

        private readonly Color _color = new(26, 148, 230);

        public InfoRoleModule(ITranslation translation)
        {
            _translation = translation;
        }

        [Command("inforole")]
        [Summary("CMD_SUMMARY_ROLE_INFO")]
        public async Task InfoRole(SocketRole role)
        {
            if (role.IsEveryone)
                return;

            var text = $"CMD_ADMINS_ROLE_ROLE_ID : {role.Id}\n" +
                       $"CMD_ADMINS_SERVER_USERS : {role.Members.Count()}\n" +
                       $"CMD_ADMINS_ROLE_HOISTED : {role.IsHoisted}\n" +
                       $"CMD_ADMINS_SERVER_CREATED : {role.CreatedAt:d}\n" +
                       $"CMD_ADMINS_ROLE_POSITION : {role.Position}\n" +
                       $"CMD_ADMINS_ROLE_MENTIONABLE : {role.IsMentionable}\n" +
                       $"CMD_ADMINS_ROLE_PERMISSIONS : {role.Permissions}\n" +
                       $"CMD_ADMINS_ROLE_COLOR : {role.Color}";

            text = _translation.TranslationText(text);

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithDescription(text);

            await Context.Message.ReplyAsync(
                $"***{role.Name}*** {_translation.GetTranslationByTextId("CMD_ADMINS_USER_INFO")}\n",
                embed: embed.Build());
        }
    }
}