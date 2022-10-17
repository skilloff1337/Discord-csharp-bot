using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.Translation.Interfaces;

namespace Discord_Bot.Services.Translation
{
    public class Translation : ITranslation
    {
        private readonly IJsonReader<Dictionary<string,TranslationWord>> _loader;
        
        private Dictionary<string, TranslationWord> _currentLanguage = new();
        private static readonly Regex _regex = new(@"(?:[A-Z]+_|[A-Z]:?){5,}", RegexOptions.Compiled);

        public Translation(IJsonReader<Dictionary<string,TranslationWord>> loader)
        {
            _loader = loader;
            
            LoadTranslationWords();
        }

        public string GetTranslationByTextId(string textId)
        {
            if (_currentLanguage.TryGetValue(textId, out var result))
                return result.TranslationText;
            
            Console.WriteLine($"Not found TEXT_ID: {textId}, result: {result}");
            return "Unknown";
        }

        public string TranslationText(string text)
        {
            var matches = _regex.Matches(text);
            foreach (Match match in matches)
            {
                if (!_currentLanguage.TryGetValue(match.ToString(), out var replace))
                {
                    replace.TranslationText = "Unknown";
                    Console.WriteLine($"Not found match {match}");
                }
                
                text = text.Replace(match.ToString(), replace.TranslationText);
            }

            return text;
        }

        private void LoadTranslationWords()
        {
            _currentLanguage = _loader.Load();
        }
    }
}