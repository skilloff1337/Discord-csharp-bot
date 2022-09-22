using Discord_Bot.Models;

namespace Discord_Bot.Services.DataReader.Interfaces
{
    public interface IJsonReader<T>
    {
        public T Load();
    }
}