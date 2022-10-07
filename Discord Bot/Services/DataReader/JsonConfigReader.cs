using System;
using System.IO;
using Discord_Bot.Models;
using Discord_Bot.Services.PathProvider.Interfaces;
using Newtonsoft.Json;

namespace Discord_Bot.Services.DataReader
{
    public class JsonConfigReader
    {
        private readonly Config _config;
        
        private readonly string _path;
        private const string FILE_NAME = "Config.json";

        public JsonConfigReader(IPathProvider pathProvider, Config config)
        {
            _config = config;
            _path = pathProvider.GetDataPath(FILE_NAME);

            Load();
        }

        private void Load()
        {
            if (!File.Exists(_path))
            {
                Console.WriteLine($"{FILE_NAME} not found on path {_path}. Default settings loaded.");
                return;
            }

            var text = File.ReadAllText(_path);
            var data = JsonConvert.DeserializeObject<Config>(text);

            if (data == null)
                return;
            
            _config.Token = data.Token;
            _config.Version = data.Version;

            _config.AdministratorsID = data.AdministratorsID;
            _config.Ranks = data.Ranks;

            _config.DataBaseName = data.DataBaseName;
            _config.CollectionMessage = data.CollectionMessage;
            _config.ConnectionString = data.ConnectionString;

            _config.Language = data.Language;
            _config.Status = data.Status;
            _config.GameName = data.GameName;

            _config.IdServer = data.IdServer;
            _config.ChannelIdForBotAdminCommand = data.ChannelIdForBotAdminCommand;
            _config.ChannelIdForBotCommand = data.ChannelIdForBotCommand;
            _config.ChannelIdForBotWelcome = data.ChannelIdForBotWelcome;
            _config.ChannelIdForBotLog = data.ChannelIdForBotLog;
        }
    }
}