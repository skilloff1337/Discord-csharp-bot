using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot.Attributes;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.DataWriter.Interfaces;
using MongoDB.Bson;
using ChannelType = Discord_Bot.Models.Types.ChannelType;

namespace Discord_Bot.Modules.Admins.Ranks
{
    [Summary("Admin")]
    [RequiredChannel(ChannelType.BotAdminCommand)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class AddRoleToSystemModule : ModuleBase<SocketCommandContext>
    {
        private readonly IJsonWriter<Config> _writer;
        private readonly Config _config;

        private readonly List<Rank> _listRanks;
        private readonly Color _color = new(26, 148, 230);
        private readonly ulong _idLogChannel;

        public AddRoleToSystemModule(Config config, IJsonWriter<Config> writer)
        {
            _writer = writer;
            _config = config;
            
            _listRanks = _config.Ranks;
            _idLogChannel = _config.ChannelIdForBotLog;
        }

        [Command("addRoleToRank")]
        [Summary("CMD_SUMMARY_CLEAR_ALL_MESSAGE")]
        public Task AddRoleToRank(IRole role, uint needExp, [Remainder] string name)
        {
            _ = Task.Run(async () =>
            {
                if (Context.Guild.GetChannel(_idLogChannel) is not IMessageChannel messageChannel)
                {
                    Console.WriteLine("Invalid channel ID for logs");
                    return;
                }
                if (_listRanks.Exists(x => x.NameRank == name))
                {
                    await Context.Message.ReplyAsync($"This rank name is already occupied in the ranking system.");
                    return;
                }
                
                var newRank = new Rank
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Level = (byte)(_config.Ranks.Count + 1),
                    NeedExp = needExp,
                    RoleID = role.Id,
                    NameRank = name,
                };
                UpdateRanks(newRank);

                var embed = new EmbedBuilder()
                    .WithColor(_color)
                    .WithCurrentTimestamp()
                    .WithDescription(
                        $"Admin: {Context.User.Username}#{Context.User.Discriminator}\n" +
                        $"ID Rank: {newRank.Id}\n" +
                        $"Name Rank: {newRank.NameRank}\n" +
                        $"Level: {newRank.Level}\n" +
                        $"ID Role: {newRank.RoleID}\n" +
                        $"Need EXP: {newRank.NeedExp}");

               await messageChannel.SendMessageAsync($"{Context.User.Mention} Create new rank",embed:embed.Build());
            });
            return Task.CompletedTask;
        }

        private void UpdateRanks(Rank newRank)
        {
            _config.Ranks.Add(newRank);
            _writer.WriteData(_config);
        }
    }
}