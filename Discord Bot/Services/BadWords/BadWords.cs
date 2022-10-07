using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Discord_Bot.Services.DataReader.Interfaces;

namespace Discord_Bot.Services.BadWords
{
    public class BadWords : IBadWords
    {
        private readonly Stopwatch _stopwatch = new();

        public string[] GetWords { get; }

        public BadWords(IJsonReader<string[]> reader)
        {
            GetWords = reader.Load();
        }

        public bool CheckForBadWords(string text)
        {
            _stopwatch.Restart();
            var res= GetWords.Select(badword => 
                new Regex(badword)).Select(regex => regex.Match(text.ToLower())).Any(match => match.Success);
            _stopwatch.Stop();
            Console.WriteLine(_stopwatch.Elapsed);
            return res;
        }

        public void SaveWords()
        {
            
        }

        public void AddNewWord(string word)
        {
            
        }

        public void DelWord(string word)
        {
            
        }
    }
}