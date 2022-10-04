using System;
using MongoDB.Bson;

namespace Discord_Bot.Models
{
    public class User
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public ulong DiscordId { get; set; }
        public byte Level { get; set; }
        public uint CurrentExp { get; set; }
    }
}