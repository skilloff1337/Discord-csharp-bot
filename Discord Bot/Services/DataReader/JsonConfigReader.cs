using System;
using System.Collections.Generic;
using System.IO;
using Discord;
using Discord_Bot.Models;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.DataReader.Interfaces;
using Newtonsoft.Json;

namespace Discord_Bot.Services.DataReader
{
    public class JsonConfigReader : IJsonReader<Config>
    {
        private readonly string _path;
        private readonly Lazy<Config> _config;
        
        private const string FILE_NAME = "Config.json";

        public JsonConfigReader()
        {
            _path = Path.Combine(AppContext.BaseDirectory, FILE_NAME);
            _config = new Lazy<Config>(LoadCore);
        }

        public Config Load() =>
            _config.Value;
        
        private Config LoadCore()
        {
            if (!File.Exists(_path))
            {
                CreateDefaultConfig();
                Console.WriteLine($"{FILE_NAME} not found on path {_path}. Default settings loaded.");
            }
            var data = File.ReadAllText(_path);
            
            return JsonConvert.DeserializeObject<Config>(data);
        }

        private void CreateDefaultConfig()
        {
            var test = new Config()
            {
                DataBaseName = "discordBot",
                ConnectionString = "mongodb://localhost:27017",
                GameName = "Development",
                Status = UserStatus.Online,
                Token = "MTAyMTc0NjIzNDM0MTQ3MDI0Mg.GIomAd.cLuQ-spPvNiiyUEBctGIQ1L-FiLgr1atyyLJBk",
                Version = "1.0.0",
                IdServer = 305330911040372737,
                ChannelIdForBotCommand = 1022189416728498186,
                ChannelIdForBotWelcome = 1022418678807023687,
                ChannelIdForBotLog = 1022438902809776128,
                ChannelIdForBotAdminCommand = 1023177360155086878,
                Language = LanguageType.Russian,
                AdministratorRoles = new List<IRole>(),
                Ranks = new List<Rank>()
            };
            var data = JsonConvert.SerializeObject(test, Formatting.Indented);
            File.AppendAllText(_path, data);
        }
    }
}