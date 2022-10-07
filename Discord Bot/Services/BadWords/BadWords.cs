using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Discord_Bot.Services.BadWords.Interfaces;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.DataWriter.Interfaces;

namespace Discord_Bot.Services.BadWords
{
    public class BadWords : IBadWords
    {
        private readonly IJsonWriter<List<string>> _writer;

        public List<string> Words { get; }

        public BadWords(IJsonReader<List<string>> reader, IJsonWriter<List<string>> writer)
        {
            _writer = writer;
            Words = reader.Load().ToList();
        }

        public bool CheckForBadWords(string text)
        {
            return Words.Select(badword => 
                new Regex(badword)).Select(regex => regex.Match(text.ToLower())).Any(match => match.Success);
        }

        public void SaveWords()
        {
            _writer.WriteData(Words);
        }

        public void AddNewWord(string word)
        {
            Words.Add(word);
            SaveWords();
        }

        public void DelWord(string word)
        {
            Words.Remove(word);
            SaveWords();
        }
    }
}