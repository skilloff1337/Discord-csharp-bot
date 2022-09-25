namespace Discord_Bot.Models
{
    public class TranslationWord
    {
        public string TranslationText { get; set; }

        public TranslationWord(string translationText)
        {
            TranslationText = translationText;
        }
    }
}