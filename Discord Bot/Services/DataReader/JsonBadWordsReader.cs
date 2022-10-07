using System.Collections.Generic;
using System.IO;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.PathProvider.Interfaces;
using Newtonsoft.Json;

namespace Discord_Bot.Services.DataReader
{
    public class JsonBadWordsReader : IJsonReader<List<string>>
    {
        private readonly string _path;
        private const string FILE_NAME = "BadWords.json";
        public JsonBadWordsReader(IPathProvider provider)
        {
            _path = provider.GetDataPath(FILE_NAME);
        }
        public List<string> Load()
        {
            var text = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<string>>(text);
        }
    }
}