namespace Discord_Bot.Services.Translation.Interfaces
{
    public interface ITranslation
    {
        public string GetTranslationByTextId(string textID);
        
        public string TranslationText(string text);
    }
}