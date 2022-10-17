using System.IO;
using Discord_Bot.Models;
using Discord_Bot.Services.DataWriter.Interfaces;
using Discord_Bot.Services.PathProvider.Interfaces;
using Newtonsoft.Json;

namespace Discord_Bot.Services.DataWriter
{
    public class JsonConfigWriter : IJsonWriter<Config>
    {
        private readonly string _path;
        
        private const string FILE_NAME = "Config.json";
        private readonly Config _config;
        
        public JsonConfigWriter(IPathProvider pathProvider, Config config)
        {
            _config = config;
            _path = pathProvider.GetDataPath(FILE_NAME);
        }
        public void WriteData(Config data = null)
        {
            var save = JsonConvert.SerializeObject(_config, Formatting.Indented);
            File.WriteAllText(_path,save);
        }
    }
}