namespace Discord_Bot.Services.PathProvider.Interfaces
{
    public interface IPathProvider
    {
        string GetDataPath(params string[] path);
    }
}