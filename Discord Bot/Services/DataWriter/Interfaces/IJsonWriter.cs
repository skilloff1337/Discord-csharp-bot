namespace Discord_Bot.Services.DataWriter.Interfaces
{
    public interface IJsonWriter<T>
    {
        void WriteData(T data);
    }
}