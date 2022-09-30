using System;
using System.IO;
using System.Net.Mime;
using Discord_Bot.Services.PathProvider.Interfaces;

namespace Discord_Bot.Services.PathProvider
{
    public class PathProvider : IPathProvider
    {
        private readonly string _basePath;
        
        public PathProvider()
        {
            var path = AppContext.BaseDirectory;
            _basePath = Path.Combine(path, "Data");
        }

        public string GetDataPath(params string[] path)
            => Path.Combine(_basePath, Path.Combine(path));
    }
}