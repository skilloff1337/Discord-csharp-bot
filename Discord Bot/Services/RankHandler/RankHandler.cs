using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.DataWriter.Interfaces;

namespace Discord_Bot.Services.RankHandler
{
    public class RankHandler : IRankHandler
    {
        private readonly IJsonWriter<List<User>> _jsonWriter;
        private readonly DiscordSocketClient _client;
        private readonly IExperienceService _experience;
        private readonly Config _config;

        private readonly List<User> _users;

        private DateTime _nextSave = DateTime.Now.AddHours(1);

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
            if (message.Author.IsBot)
                return;

            var user = _users.FirstOrDefault(x => x.DiscordId == message.Author.Id);
            if (user is not null)
            {
                user.CurrentExp++;
                if (_experience.EnoughExperience(user.CurrentExp, user.Level))
                {
                    user.Level++;
                    user.CurrentExp = 0;

                    var discordUser = _client.GetGuild(_config.IdServer).GetUser(user.DiscordId);
                    await discordUser!.AddRoleAsync(_config.Ranks[user.Level].RoleId);
                    await discordUser.SendMessageAsync($"You have leveled up, congratulations!");
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

            Console.WriteLine($"User: id {_users[0].Id} level: {_users[0].Level} exp: {_users[0].CurrentExp}");
            if (DateTime.Now > _nextSave)
                SaveUsers();
        }


        private void SaveUsers()
        {
            _nextSave = DateTime.Now.AddHours(1);
            _jsonWriter.WriteData(_users);
        }
    }
}