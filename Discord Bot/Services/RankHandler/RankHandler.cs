using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.DataWriter.Interfaces;
using Discord_Bot.Services.RankHandler.Interfaces;

namespace Discord_Bot.Services.RankHandler
{
    public class RankHandler : IRankHandler
    {
        private readonly IJsonWriter<List<User>> _jsonWriter;
        private readonly DiscordSocketClient _client;
        private readonly IExperienceService _experience;
        private readonly Config _config;

        private readonly List<User> _users;
        private readonly Color _color = new(26, 148, 230);

        private DateTime _nextSave = DateTime.Now.AddHours(1);

        public int CountUsers =>
            _users.Count;

        public RankHandler(IJsonReader<List<User>> reader, IJsonWriter<List<User>> jsonWriter,
            DiscordSocketClient client, IExperienceService experience, Config config)
        {
            _jsonWriter = jsonWriter;
            _client = client;
            _experience = experience;
            _config = config;
            _users = reader.Load();
            Console.WriteLine($"Users: {_users.Count}");
        }

        public async Task InstallEventsAsync()
        {
            _client.MessageReceived += MessageReceived;
            await Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Author.IsBot || message.Channel is IDMChannel)
                return;

            var user = _users.FirstOrDefault(x => x.DiscordId == message.Author.Id);
            if (user is not null)
            {
                user.CurrentExp++;
                if (_experience.EnoughExperience(user.CurrentExp, user.Level))
                {
                    user.Level++;
                    user.CurrentExp = 0;

                    var guild = _client.GetGuild(_config.IdServer);
                    var roleId = _config.Ranks[user.Level].RoleId;

                    var embed = new EmbedBuilder()
                        .WithColor(_color)
                        .WithCurrentTimestamp()
                        .WithDescription(
                            $"New rank name: {_config.Ranks[user.Level].NameRank}\n" +
                            $"New level: {user.Level}\n" +
                            $"New Exp: {user.CurrentExp}\n" +
                            $"New Role: {guild.GetRole(roleId).Name}")
                        .Build();
                    var discordUser = guild.GetUser(user.DiscordId);
                    await discordUser!.AddRoleAsync(roleId);
                    await discordUser.SendMessageAsync($"You have leveled up, congratulations!",embed:embed);
                }
            }
            else
            {
                var newUser = new User()
                {
                    CurrentExp = 1,
                    Level = 0,
                    DiscordId = message.Author.Id
                };
                _users.Add(newUser);
            }
            
            if (DateTime.Now > _nextSave)
                SaveUsers();
        }

        public int[] CountUserInLevels()
        {
            var list = new List<int>();
            for (var i = 0; i < _config.Ranks.Count; i++)
            {
                list.Add(_users.Count(x => x.Level == i));
            }

            return list.ToArray();
        }
        


        private void SaveUsers()
        {
            _nextSave = DateTime.Now.AddHours(1);
            _jsonWriter.WriteData(_users);
        }
    }
}