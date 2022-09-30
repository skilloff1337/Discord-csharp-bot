using Discord;
using MongoDB.Bson;

namespace Discord_Bot.Models
{
    public class Rank
    {
        public ObjectId Id { get; set; }
        public string NameRank { get; set; }
        public byte Level { get; set; }
        public IRole Role { get; set; }
    }
}