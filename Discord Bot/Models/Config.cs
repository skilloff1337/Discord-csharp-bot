using Discord;

namespace Discord_Bot.Models
{
    public class Config
    {
        
        public string DataBaseName { get; set; }
        public string ConnectionString { get; set; }
        public LanguageType Language { get; set; }
        public string Token { get; set; }
        public string Version { get; set; }
        public UserStatus Status { get; set; }
        public string GameName { get; set; }
        
        public ulong ChannelIdForBotCommand { get; set; }
        public ulong ChannelIdForBotAdminCommand { get; set; }
        public ulong ChannelIdForBotWelcome { get; set; }
        public ulong ChannelIdForBotLog { get; set; }
        public ulong ChannelIdForBotNews { get; set; }
        
        public string CommandHelpText { get; set; }
        
    }
}