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

namespace Discord_Bot.Modules.Admins.Message
{
    [Summary("Admin")]
    [RequiredChannel(ChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class AdminMessageHistoryModule : ModuleBase<SocketCommandContext>
    {
        private readonly IRepository<MessageUser> _repository;
        private readonly ITranslation _translation;
        private readonly DiscordSocketClient _client;
        
        private readonly Color _color = new(26, 148, 230);

        public AdminMessageHistoryModule(IRepository<MessageUser> repository, ITranslation translation,
            DiscordSocketClient client, IJsonReader<Config> reader)
        {
            _translation = translation;
            _client = client;
        }

        [Command("messageHistory")]
        [Summary("CMD_SUMMARY_HISTORY_MESSAGE")]
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
            var text = $"**CMD_ADMINS_SERVER_OWNER :** {message.NickName}\n" +
                       $"**CMD_ADMINS_CHANNEL_NAME :** {message.ChannelName}\n" +
                       $"**CMD_ADMINS_CHANNEL_ID :** {message.ChannelId}\n" +
                       $"**CMD_ADMINS_SERVER_CREATED :** {message.CreateMessage}\n" +
                       $"**CMD_ADMINS_SERVER_CREATED :** {message.EditMessage}\n" +
                       $"**CMD_ADMINS_MESSAGE_EDIT :** {message.EditCount}\n" +
                       $"**CMD_ADMINS_MESSAGE :** {message.CurrentMessage}\n" +
                       $"**CMD_ADMINS_HISTORY_MESSAGE :**\n {historyMessages}" +
                       $"**CMD_ADMINS_DELETED_MESSAGE :** {message.IsDeleted}\n" +
                       $"**CMD_ADMINS_DELETED_DATE_MESSAGE :** {message.DeletedDate}\n";

            text = _translation.TranslationText(text);

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithDescription(text);

            await Context.Message.ReplyAsync($"{_translation.GetTranslationByTextID("CMD_ADMINS_HISTORY_MESSAGE")}" +
                                             $" **{message.MessageId}**", embed: embed.Build());
        }
    }
}