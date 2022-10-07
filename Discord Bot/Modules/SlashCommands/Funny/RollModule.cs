using System;
using System.Threading.Tasks;
using Discord.Interactions;

namespace Discord_Bot.Modules.SlashCommands.Funny
{
    public class RollModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Random _random = new();
        [SlashCommand("roll", "Roll number")]
        public async Task Roll(int min = 0, int max = 100)
        {
            var num = _random.Next(min, max);
            var result = $"{Context.User.Mention} roll [{min}/{max}], result: {num}";
            await RespondAsync(result);
        }
    }
}