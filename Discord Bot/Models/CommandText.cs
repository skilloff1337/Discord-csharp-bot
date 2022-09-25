namespace Discord_Bot.Models
{
    public class CommandText
    {
        public string TextCommand { get; set; }

        public CommandText(string textCommand)
        {
            TextCommand = textCommand;
        }
    }
}