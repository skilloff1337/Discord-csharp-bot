using System;
using System.Collections.Generic;
using System.IO;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader.Interfaces;
using Newtonsoft.Json;

namespace Discord_Bot.Services.DataReader
{
    public class JsonLanguageReader : IJsonReader<Dictionary<string,TranslationWord>>
    {
        private readonly string _path;
        private readonly Lazy<Dictionary<string,TranslationWord>> _translationWords;
        
        private readonly string _fileName = "TranslationWords_";

        public JsonLanguageReader(IJsonReader<Config> configReader)
        {
            var language = configReader.Load().Language;
            _translationWords = new Lazy<Dictionary<string, TranslationWord>>(LoadCore);

            _fileName += $"{language.ToString()}.json";
            _path = Path.Combine(AppContext.BaseDirectory, _fileName);
        }

        private Dictionary<string, TranslationWord> LoadCore() =>
            _translationWords.Value;

        public Dictionary<string, TranslationWord> Load()
        {
            if (!File.Exists(_path))
            {
                Console.WriteLine($"{_fileName} not found on path {_path}. Default settings loaded.");
                throw new Exception();
            }
            var data = File.ReadAllText(_path);
            
            return JsonConvert.DeserializeObject<Dictionary<string,TranslationWord>>(data);
        }
    }
}