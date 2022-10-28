using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;

namespace Discord_Bot.Modules.Admins.Message
{
    [Summary("Admin")]
    [RequiredChannel(DiscordChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class AddServerReactionMessage: ModuleBase<SocketCommandContext>
    {
        private readonly Config _config;

        public AddServerReactionMessage(Config config)
        {
            _config = config;
        }

        [Command("addBotServerReaction")]
        [Summary("[CMD_SUMMARY_ADD_SERVER_REACTION_TO_MESSAGE]")]
        public async Task AddBotServerReaction(ulong idChannel, ulong idMessage)
        {

            var chn = Context.Guild.GetChannel(idChannel) as IMessageChannel;
            var msg = chn!.GetMessageAsync(idMessage).Result;
            
            var emojiAlpha = Context.Guild.GetEmoteAsync(1031494505821646858).Result;
            var emojiBeta = Context.Guild.GetEmoteAsync(1031494309238804540).Result;
            
            await msg.AddReactionAsync(emojiAlpha);
            await msg.AddReactionAsync(emojiBeta);
        }
    }
}