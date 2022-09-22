using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Discord_Bot.Models
{
    public class MessageUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("channelId")]
        public ulong ChannelId { get; set; }

        [BsonElement("channelName")]
        public string ChannelName { get; set; }

        [BsonElement("messageId")]
        public ulong MessageId { get; set; }

        [BsonElement("nickName")]
        public string NickName { get; set; }

        [BsonElement("lastMessage")]
        public string CurrentMessage { get; set; }

        [BsonElement("createMessage")]
        public DateTime CreateMessage { get; set; }

        [BsonElement("editMessage")]
        public DateTime EditMessage { get; set; }


        [BsonElement("editCount")]
        public int EditCount { get; set; }

        [BsonElement("historyMessage")]
        public List<string> HistoryMessage { get; set; } = new();

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; }

        [BsonElement("deletedDate")]
        public DateTime DeletedDate { get; set; }
    }
}