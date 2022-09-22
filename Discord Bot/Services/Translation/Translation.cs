using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.Translation.Interfaces;

namespace Discord_Bot.Services.Translation
{
    public class Translation : ITranslation
    {
        private readonly JsonLanguageReader _loader;
        
        private Dictionary<string, string> _currentLanguage = new();
        private static readonly Regex _regex = new(@"(?:[A-Z]+_|[A-Z]:?){5,}", RegexOptions.Compiled);

        public Translation(JsonLanguageReader loader)
        {
            _loader = loader;
            
            LoadTranslationWords();
        }

        public string GetTranslationByTextID(string textID)
        {
            if (_currentLanguage.TryGetValue(textID, out var result))
                return result;
            
            System.Console.WriteLine($"Not found TEXT_ID: {textID}, result: {result}");
            return "Unknown";
        }

        public string TranslationText(string text)
        {
            var matches = _regex.Matches(text);
            
            foreach (Match match in matches)
            {
                if (!_currentLanguage.TryGetValue(match.ToString(), out var replace))
                {
                    replace = "Unknown";
                    System.Console.WriteLine($"Not found match {match}");
                }
                
                text = text.Replace(match.ToString(), replace);
            }

            return text;
        }

        private void LoadTranslationWords()
        {
            _currentLanguage = _loader.Load();
        }
    }
}