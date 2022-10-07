using System;

namespace Discord_Bot.Services.BadWords
{
    public interface IBadWords
    {
        string[] GetWords { get; }
        bool CheckForBadWords(string text);
        void AddNewWord(string word);
        void DelWord(string word);
        void SaveWords();
    }
}