using System;
using System.Collections.Generic;
using System.IO;
using Discord;
using Discord.WebSocket;
using Discord_Bot.Models.Types;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.PathProvider.Interfaces;
using Newtonsoft.Json;

namespace Discord_Bot.Models
{
    public class Config
    {
        public bool FirstLoading = true;
        public string Token { get; set; } = "None";
        public string Version { get; set; } = "0.0.1";

        public List<IRole> AdministratorRoles { get; set; } = new();
        public Dictionary<int, Rank> Ranks { get; set; } = new();
        public string DataBaseName { get; set; } = "DiscordBot";
        public string CollectionMessage { get; set; } = "MessageUser";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";

        public LanguageType Language { get; set; } = LanguageType.English;

        public UserStatus Status { get; set; } = UserStatus.Online;
        public string GameName { get; set; } = "None";
        public bool EnableWordProtection { get; set; } = true;


        public ulong IdServer { get; set; }
        public ulong ChannelIdForBotCommand { get; set; }
        public ulong ChannelIdForBotAdminCommand { get; set; }
        public ulong ChannelIdForBotWelcome { get; set; }
        public ulong ChannelIdForBotLog { get; set; }
    }
}