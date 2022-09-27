using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Services.DataBase.Interfaces;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.Translation.Interfaces;
using ChannelType = Discord_Bot.Models.Types.ChannelType;

namespace Discord_Bot.Modules
{
    [RequiredChannel(ChannelType.BotAdminCommand)]
    [Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly ITranslation _translation;
        private readonly CommandService _commandService;
        private readonly IRepository<MessageUser> _repository;

        private readonly Color _color = new(26, 148, 230);
        private readonly ulong _idLogChannel;

        public AdminModule(ITranslation translation,
            CommandService commandService, IRepository<MessageUser> repository, IJsonReader<Config> config)
        {
            _translation = translation;
            _commandService = commandService;
            _repository = repository;

            _idLogChannel = config.Load().ChannelIdForBotLog;
        }

        [Command("help")]
        [Discord.Commands.Summary("CMD_SUMMARY_HELP")]
        public async Task Help()
        {
            var commandInfos = _commandService
                .Commands
                .Where(x => x.Module.Name is "AdminModule" or "AdminPunishmentModule");

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

            await Context.Message.ReplyAsync(_translation.GetTranslationByTextID("CMD_ADMINS_COMMANDS"),
                embed: embed.Build());
        }

        [Command("infoserver")]
        [Discord.Commands.Summary("CMD_SUMMARY_SERVER_INFO")]
        [Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        [Discord.Commands.RequireBotPermission(GuildPermission.Administrator)]
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
        [Discord.Commands.Summary("CMD_SUMMARY_USER_INFO")]
        [Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        [Discord.Commands.RequireBotPermission(GuildPermission.Administrator)]
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
        [Discord.Commands.Summary("CMD_SUMMARY_ROLE_INFO")]
        [Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        [Discord.Commands.RequireBotPermission(GuildPermission.Administrator)]
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

        [Command("messagehistory")]
        [Discord.Commands.Summary("CMD_SUMMARY_HISTORY_MESSAGE")]
        [Discord.Commands.RequireUserPermission(GuildPermission.Administrator)]
        [Discord.Commands.RequireBotPermission(GuildPermission.Administrator)]
        public async Task MessageHistory(ulong idMessage)
        {
            var message = _repository.GetById(idMessage);
            if (message is null)
            {
                await Context.Message.ReplyAsync(_translation.GetTranslationByTextID("CMD_ADMINS_NOT_FOUND"));
                return;
            }

            var historyMessages = new StringBuilder(300);
            foreach (var msg in message.HistoryMessage)
                historyMessages.Append($"{msg}\n");
            var text = $"**CMD_ADMINS_SERVER_OWNER :**             {message.NickName}\n" +
                       $"**CMD_ADMINS_CHANNEL_NAME :**      {message.ChannelName}\n" +
                       $"**CMD_ADMINS_CHANNEL_ID :**        {message.ChannelId}\n" +
                       $"**CMD_ADMINS_SERVER_CREATED :**       {message.CreateMessage}\n" +
                       $"**CMD_ADMINS_SERVER_CREATED :**       {message.EditMessage}\n" +
                       $"**CMD_ADMINS_MESSAGE_EDIT :**      {message.EditCount}\n" +
                       $"**CMD_ADMINS_MESSAGE :**   {message.CurrentMessage}\n" +
                       $"**CMD_ADMINS_HISTORY_MESSAGE :**\n {historyMessages}" +
                       $"**CMD_ADMINS_DELETED_MESSAGE :**   {message.IsDeleted}\n" +
                       $"**CMD_ADMINS_DELETED_DATE_MESSAGE :**      {message.DeletedDate}\n";

            text = _translation.TranslationText(text);
            
            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithDescription(text);

            await Context.Message.ReplyAsync($"{_translation.GetTranslationByTextID("CMD_ADMINS_HISTORY_MESSAGE")}" +
                                             $" **{message.MessageId}**", embed: embed.Build());
        }
    }
}