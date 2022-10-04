using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.Translation.Interfaces;

namespace Discord_Bot.Modules.Admins
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    public class AdminPunishmentModule : ModuleBase<SocketCommandContext>
    {
        private readonly ITranslation _translation;
        private readonly DiscordSocketClient _client;
        private readonly Config _config;

        public AdminPunishmentModule(Config config, 
            ITranslation translation, DiscordSocketClient client)
        {
            _config = config;
            _translation = translation;
            _client = client;
        }

        [Command("ban")]
        [Summary("CMD_SUMMARY_BAN")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUser(IUser user, int days, string reason)
        {
            var text =
                $"CMD_ADMINS_ADMINISTRATOR ***{Context.User.Username}*** CMD_ADMINS_BANNED " +
                $"***{user.Username}#{user.Discriminator}*** [ID:{user.Id}] " +
                $"CMD_ADMINS_FOR ***{days}*** CMD_ADMINS_DAYS, CMD_ADMINS_REASON : ***{reason}***";
            
            text = _translation.TranslationText(text);
            
            await WriteTextToBotLog(text);
        }

        [Command("unban")]
        [Summary("CMD_SUMMARY_UNBAN")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task UnBanUser(IUser user)
        {
            var text =
                $"CMD_ADMINS_ADMINISTRATOR  ***{Context.User.Username}*** CMD_ADMINS_UNBANNED " +
                $"***{user.Username}#{user.Discriminator}*** [ID:{user.Id}] ";

            text = _translation.TranslationText(text);
            
            await Context.Guild.RemoveBanAsync(user);
            await WriteTextToBotLog(text);
        }

        [Command("kick")]
        [Summary("CMD_SUMMARY_KICK")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task KickUser(SocketGuildUser user, string reason)
        {
            var text =
                $"CMD_ADMINS_ADMINISTRATOR ***{Context.User.Username}*** CMD_ADMINS_KICK " +
                $"***{user.Username}#{user.Discriminator}*** [ID:{user.Id}], CMD_ADMINS_REASON : ***{reason}***";

            text = _translation.TranslationText(text);
            
            await user.KickAsync(text);
            await WriteTextToBotLog(text);
        }

        [Command("mute")]
        [Summary("CMD_SUMMARY_MUTE")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        [RequireBotPermission(GuildPermission.MuteMembers)]
        public async Task MuteUser(SocketGuildUser user, string reason)
        {
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Mute");
            
            var text =
                $"CMD_ADMINS_ADMINISTRATOR ***{Context.User.Username}*** CMD_ADMINS_MUTE" +
                $" ***{user.Username}#{user.Discriminator}*** [ID:{user.Id}], " +
                $"CMD_ADMINS_REASON : ***{reason}***";

            text = _translation.TranslationText(text);
            
            await user.AddRoleAsync(role);
            await WriteTextToBotLog(text);
        }

        [Command("unmute")]
        [Summary("CMD_SUMMARY_UNMUTE")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        [RequireBotPermission(GuildPermission.MuteMembers)]
        public async Task UnMuteUser(SocketGuildUser user)
        {
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Mute");
            var text =
                $"CMD_ADMINS_ADMINISTRATOR ***{Context.User.Username}*** CMD_ADMINS_UNMUTE " +
                $"***{user.Username}#{user.Discriminator}*** [ID:{user.Id}]";

            text = _translation.TranslationText(text);
            await user.RemoveRoleAsync(role);
            await WriteTextToBotLog(text);
        }


        private async Task WriteTextToBotLog(string text)
        {
            var channel = await _client.GetChannelAsync(_config.ChannelIdForBotLog) as IMessageChannel;

            Console.WriteLine(text.Replace('*', ' '));
            await Context.Message.ReplyAsync(text);
            await channel!.SendMessageAsync(text);
        }
    }
}