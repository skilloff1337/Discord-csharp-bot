using System.Collections.Generic;
using System.IO;
using Discord_Bot.Services.DataWriter.Interfaces;
using Discord_Bot.Services.PathProvider.Interfaces;
using Newtonsoft.Json;

namespace Discord_Bot.Services.DataWriter
{
    public class JsonBadWordsWriter : IJsonWriter<List<string>>
    {
        private readonly string _path;
        private const string FILE_NAME = "BadWords.json";

        public JsonBadWordsWriter(IPathProvider pathProvider)
        {
            _path = pathProvider.GetDataPath(FILE_NAME);
        }

        public void WriteData(List<string> data)
        {
            var text = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_path, text);
        }
    }
}