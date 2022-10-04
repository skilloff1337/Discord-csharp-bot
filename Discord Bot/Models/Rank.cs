using Discord;
using MongoDB.Bson;

namespace Discord_Bot.Models
{
    public class Rank
    {
        public string Id { get; set; }
        public string NameRank { get; set; }
        public byte Level { get; set; }
        public uint NeedExp { get; set; }
        public ulong RoleId { get; set; }
    }
}