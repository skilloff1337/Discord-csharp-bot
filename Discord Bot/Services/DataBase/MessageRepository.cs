using System.Collections.Generic;
using Discord_Bot.Models;
using Discord_Bot.Services.DataBase.Interfaces;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Discord_Bot.Services.DataBase
{
    public class MessageRepository : IRepository<MessageUser>
    {
        private readonly MongoClient _client;
        private readonly IMongoCollection<MessageUser> _collection;
        private readonly IMongoDatabase _database;

        public MessageRepository(IJsonReader<Config> configReader)
        {
            var databaseName = configReader.Load().DataBaseName;
            var connectionString = configReader.Load().ConnectionString;
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
            _collection = _database.GetCollection<MessageUser>("messageUser");
        }

        public IEnumerable<MessageUser> GetAll()
            => _collection.Find(x => true).ToList().ToArray();

        public MessageUser GetByObjectId(ObjectId id)
            => _collection.Find(Builders<MessageUser>.Filter.Eq("_id", id)).SingleOrDefault();

        public MessageUser GetById(ulong id)
            => _collection.Find(Builders<MessageUser>.Filter.Eq("messageId", id)).SingleOrDefault();


        public bool Create(MessageUser data)
        {
            if (IsAlreadyHaveEntryInDateBase(ObjectId.Parse(data.Id)))
                return false;

            _collection.InsertOne(data);
            return true;
        }

        public void Update(MessageUser data)
        {
            var test = GetByObjectId(ObjectId.Parse(data.Id));
            var res = _collection.ReplaceOne(Builders<MessageUser>.Filter.Eq("_id", ObjectId.Parse(data.Id)), data);
        }


        public void Delete(ObjectId id)
            => _collection.DeleteOne(Builders<MessageUser>.Filter.Eq("_id", id));

        public bool Connection()
        {
            return _database.RunCommandAsync((Command<BsonDocument>) "{ping:1}").Wait(1000);
        }

        private bool IsAlreadyHaveEntryInDateBase(ObjectId id)
            => GetByObjectId(id) != null;
    }
}