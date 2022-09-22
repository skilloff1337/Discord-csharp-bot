using Discord;

namespace Discord_Bot.Models
{
    public class Config
    {
        public LanguageType Language { get; set; }
        public string Token { get; set; }
        public string Version { get; set; }
        public UserStatus Status { get; set; }
        public string GameName { get; set; }
        
        public ulong ChannelIdForBotCommand { get; set; }
        public ulong ChannelIdForBotWelcome { get; set; }
        
        public string CommandHelpText { get; set; }
        
    }
}