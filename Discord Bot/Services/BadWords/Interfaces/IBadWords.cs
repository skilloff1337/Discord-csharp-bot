using System.Collections.Generic;

namespace Discord_Bot.Services.BadWords.Interfaces
{
    public interface IBadWords
    {
        List<string> Words { get; }
        bool CheckForBadWords(string text);
        void AddNewWord(string word);
        void DelWord(string word);
        void SaveWords();
    }
}