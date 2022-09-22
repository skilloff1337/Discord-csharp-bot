using System;
using System.Collections.Generic;
using System.IO;
using Discord_Bot.Services.DataReader.Interfaces;
using Newtonsoft.Json;

namespace Discord_Bot.Services.DataReader
{
    public class JsonLanguageReader : IJsonReader<Dictionary<string,string>>
    {
        private readonly string _path;
        private readonly Lazy<Dictionary<string,string>> _translationWords;
        
        private readonly string _fileName = "TranslationWords_";

        public JsonLanguageReader(JsonConfigReader configReader)
        {
            var language = configReader.Load().Language;
            _translationWords = new Lazy<Dictionary<string, string>>(LoadCore);

            _fileName += $"{language.ToString()}.json";
            _path = Path.Combine(AppContext.BaseDirectory, _fileName);
        }

        private Dictionary<string, string> LoadCore() =>
            _translationWords.Value;

        public Dictionary<string, string> Load()
        {
            if (!File.Exists(_path))
            {
                System.Console.WriteLine($"{_fileName} not found on path {_path}. Default settings loaded.");
                throw new Exception();
            }
            var data = File.ReadAllText(_path);
            
            return JsonConvert.DeserializeObject<Dictionary<string,string>>(data);
        }
    }
}