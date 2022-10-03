using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Services.DataBase.Interfaces;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.Translation.Interfaces;
using ChannelType = Discord_Bot.Models.Types.ChannelType;

namespace Discord_Bot.Modules.Admins
{
    [Summary("Admin")]
    [RequiredChannel(ChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly ITranslation _translation;
        private readonly CommandService _commandService;

        private readonly Color _color = new(26, 148, 230);

        public AdminModule(ITranslation translation,
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

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithDescription(_translation.TranslationText(result.ToString()));

            await Context.Message.ReplyAsync(_translation.GetTranslationByTextID("CMD_ADMINS_COMMANDS"),
                embed: embed.Build());
        }

        [Command("infoserver")]
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

            await Context.Message.ReplyAsync($"{_translation.GetTranslationByTextID("CMD_ADMINS_SERVER_INFO")}" +
                                             $" **{Context.Guild.Name}**", embed: embed.Build());
        }

        [Command("infouser")]
        [Summary("CMD_SUMMARY_USER_INFO")]
        public async Task InfoUser(SocketGuildUser user)
        {
            var textRoles = user.Roles.Aggregate("", (current, role) => current + $"{role}\n");
            var textActivity = user.Activities.Aggregate("",
                (current, activity) => current + $"**{activity.Type}** : {activity}\n");

            var text = $"CMD_ADMINS_USER_USER_ID : {user.Id}\n" +
                       $"CMD_ADMINS_USER_BOT : {user.IsBot}\n" +
                       $"CMD_ADMINS_USER_STATUS : {user.Status}\n" +
                       $"CMD_ADMINS_SERVER_CREATED : {user.CreatedAt:d}\n" +
                       $"\n***CMD_ADMINS_USER_ROLES :***\n {textRoles}" +
                       $"\n***CMD_ADMINS_USER_ACTIVITIES :***\n {textActivity}";

            text = _translation.TranslationText(text);

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithImageUrl(user.GetAvatarUrl())
                .WithDescription(text);

            await Context.Message.ReplyAsync($"***{user.Username}#{user.Discriminator}***" +
                                             $" {_translation.GetTranslationByTextID("CMD_ADMINS_USER_INFO")} \n",
                embed: embed.Build());
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
                $"***{role.Name}*** {_translation.GetTranslationByTextID("CMD_ADMINS_USER_INFO")}\n",
                embed: embed.Build());
        }
    }
}