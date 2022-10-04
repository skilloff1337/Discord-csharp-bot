using System;
using System.Collections.Generic;
using System.IO;
using Discord_Bot.Models;
using Discord_Bot.Services.DataWriter.Interfaces;
using Discord_Bot.Services.PathProvider.Interfaces;
using Newtonsoft.Json;

namespace Discord_Bot.Services.DataWriter
{
    public class JsonUserWriter : IJsonWriter<List<User>>
    {
        private readonly string _path;
        private const string FILE_NAME = "Users.json";

        public JsonUserWriter(IPathProvider pathProvider)
        {
            _path = pathProvider.GetDataPath(FILE_NAME);
        }

        public void WriteData(List<User> data)
        {
            var text = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_path,text);
        }
    }
}