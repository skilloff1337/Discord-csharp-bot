using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.DataBase.Interfaces;
using Discord_Bot.Services.Translation.Interfaces;

namespace Discord_Bot.Modules.Admins.Message
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class MessageHistoryModule : ModuleBase<SocketCommandContext>
    {
        private readonly IRepository<MessageUser> _repository;
        private readonly ITranslation _translation;

        private readonly Color _color = new(26, 148, 230);

        public MessageHistoryModule(IRepository<MessageUser> repository, ITranslation translation)
        {
            _repository = repository;
            _translation = translation;
        }

        [Command("messageHistory")]
        [Summary("[CMD_SUMMARY_HISTORY_MESSAGE]")]
        public async Task MessageHistory(ulong idMessage)
        {
            var message = _repository.GetById(idMessage);
            if (message is null)
            {
                await Context.Message.ReplyAsync(_translation.GetTranslationByTextId("CMD_ADMINS_NOT_FOUND"));
                return;
            }

            var historyMessages = new StringBuilder(300);
            foreach (var msg in message.HistoryMessage)
                historyMessages.Append($"{msg}\n");
            var text = $"**[CMD_ADMINS_SERVER_OWNER] :** {message.NickName}\n" +
                       $"**[CMD_ADMINS_CHANNEL_NAME] :** {message.ChannelName}\n" +
                       $"**[CMD_ADMINS_CHANNEL_ID] :** {message.ChannelId}\n" +
                       $"**[CMD_ADMINS_SERVER_CREATED] :** {message.CreateMessage}\n" +
                       $"**[CMD_ADMINS_MESSAGE_EDIT] :** {message.EditMessage}\n" +
                       $"**[CMD_ADMINS_MESSAGE_EDIT] :** {message.EditCount}\n" +
                       $"**[CMD_ADMINS_MESSAGE] :** {message.CurrentMessage}\n" +
                       $"**[CMD_ADMINS_HISTORY_MESSAGE] :**\n {historyMessages}";

                       text = _translation.TranslationText(text);

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithDescription(text);

            await Context.Message.ReplyAsync($"{_translation.GetTranslationByTextId("[CMD_ADMINS_HISTORY_MESSAGE]")}" +
                                             $" **{message.MessageId}**", embed: embed.Build());
        }
    }
}