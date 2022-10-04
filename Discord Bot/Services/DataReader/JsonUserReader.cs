using System;
using System.Collections.Generic;
using System.IO;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.DataWriter.Interfaces;
using Discord_Bot.Services.PathProvider.Interfaces;
using Newtonsoft.Json;

namespace Discord_Bot.Services.DataReader
{
    public class JsonUserReader : IJsonReader<List<User>>
    {
        private readonly string _path;
        private const string FILE_NAME = "Users.json";

        public JsonUserReader(IPathProvider provider)
        {
            _path = provider.GetDataPath(FILE_NAME);
        }
        public List<User> Load()
        {
            if (!File.Exists(_path))
            {
                Console.WriteLine($"{FILE_NAME} not found on path {_path}. Default settings loaded.");
                return new List<User>();
            }
            var text = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<User>>(text);
        }
    }
}