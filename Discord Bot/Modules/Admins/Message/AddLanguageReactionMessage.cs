using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;

namespace Discord_Bot.Modules.Admins.Message
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class AddLanguageReactionMessage: ModuleBase<SocketCommandContext>
    {
        private readonly Config _config;

        public AddLanguageReactionMessage(Config config)
        {
            _config = config;
        }

        [Command("addBotLanguageReaction")]
        [Summary("CMD_SUMMARY_ADD_LANGUAGE_REACTION_TO_MESSAGE")]
        public async Task Test(ulong idChannel,ulong idMessage)
        {
            var chn = Context.Guild.GetChannel(idChannel) as IMessageChannel;
            var msg = chn.GetMessageAsync(idMessage).Result;
            var emo = new Emoji("🇷🇺");
            var emo2 = new Emoji("🇺🇸");
            await msg.AddReactionAsync(emo);
            await msg.AddReactionAsync(emo2);
        }
    }
}