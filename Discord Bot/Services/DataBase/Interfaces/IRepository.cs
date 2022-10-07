using System.Collections.Generic;
using MongoDB.Bson;

namespace Discord_Bot.Services.DataBase.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetByObjectId(ObjectId id);
        T GetById(ulong id);
        bool Create(T data);
        bool CreateMany(List<T> data);
        void Update(T data);
        void Delete(ObjectId id);
        bool Connection();
    }
}