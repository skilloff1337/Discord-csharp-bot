using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Interactions;

namespace Discord_Bot.Modules.SlashCommands.Funny
{
    public class EightBallModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Random _random = new();

        private readonly string[] _answers = new[]
        {
            "Yes",
            "No",
            "Maybe",
            "Foggy answer try again",
            "Are you kidding?",
            "Forward!",
            "It's possible"
        };

        [SlashCommand("8ball", "Ask your question and get an answer")]
        public async Task EightBall([Remainder]string question)
        {
            var answerNum = _random.Next(0, _answers.Length);
            var result = $"You asked: ***{question}***, and your answer is: **{_answers[answerNum]}**";
            await RespondAsync(result);
        }
    }
}