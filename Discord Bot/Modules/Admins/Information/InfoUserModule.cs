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
    public class InfoUserModule : ModuleBase<SocketCommandContext>
    {
        private readonly ITranslation _translation;

        private readonly Color _color = new(26, 148, 230);

        public InfoUserModule(ITranslation translation)
        {
            _translation = translation;
        }
        

        [Command("infoUser")]
        [Summary("[CMD_SUMMARY_USER_INFO]")]
        public async Task InfoUser(SocketGuildUser user)
        {
            var textRoles = user.Roles.Aggregate("", (current, role) => current + $"{role}\n");
            var textActivity = user.Activities.Aggregate("",
                (current, activity) => current + $"**{activity.Type}** : {activity}\n");

            var text = $"[CMD_ADMINS_USER_USER_ID] : {user.Id}\n" +
                       $"[CMD_ADMINS_USER_BOT] : {user.IsBot}\n" +
                       $"[CMD_ADMINS_USER_STATUS] : {user.Status}\n" +
                       $"[CMD_ADMINS_SERVER_CREATED] : {user.CreatedAt:d}\n" +
                       $"\n***[CMD_ADMINS_USER_ROLES] :***\n {textRoles}" +
                       $"\n***[CMD_ADMINS_USER_ACTIVITIES] :***\n {textActivity}";

            text = _translation.TranslationText(text);

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithImageUrl(user.GetAvatarUrl())
                .WithDescription(text);

            await Context.Message.ReplyAsync($"***{user.Username}#{user.Discriminator}***" +
                                             $" {_translation.GetTranslationByTextId("[CMD_ADMINS_USER_INFO]")} \n",
                embed: embed.Build());
        }
    }
}