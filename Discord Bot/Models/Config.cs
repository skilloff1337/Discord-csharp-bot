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
        public string Token { get; set; }
        public string Version { get; set; }

        public List<IRole> AdministratorRoles { get; set; } = new();
        public Dictionary<int, Rank> Ranks { get; set; } = new();
        public string DataBaseName { get; set; }
        public string CollectionMessage { get; set; }
        public string ConnectionString { get; set; }

        public LanguageType Language { get; set; }

        public UserStatus Status { get; set; }
        public string GameName { get; set; }


        public ulong IdServer { get; set; }
        public ulong ChannelIdForBotCommand { get; set; }
        public ulong ChannelIdForBotAdminCommand { get; set; }
        public ulong ChannelIdForBotWelcome { get; set; }
        public ulong ChannelIdForBotLog { get; set; }
        
    }
}