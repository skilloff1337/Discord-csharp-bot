using System.Collections.Generic;
using Discord;
using Discord_Bot.Models.Types;

namespace Discord_Bot.Models
{
    public class Config
    {
        public string Token { get; set; }
        public string Version { get; set; }
        
        public List<IRole> AdministratorRoles { get; set; }
        public List<Rank> Ranks { get; set; } 
        public string DataBaseName { get; set; }
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