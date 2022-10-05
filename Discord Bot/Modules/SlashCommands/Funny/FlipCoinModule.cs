using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;

namespace Discord_Bot.Modules.SlashCommands.Funny
{
    public class FlipCoinModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Random _random = new();
        [SlashCommand("flip", "Flip a coin with a user")]
        public async Task FlipCoin(IUser user)
        {
            var result = $"{Context.User.Mention} - eagle, {user.Mention} - tails \n";
            
            result += _random.Next(0, 100) >= 50
                ? $"Dropped eagle, player wins {Context.User.Mention}"
                : $"Dropped tails, player wins {user.Mention}";
            await RespondAsync(result);
        }
    }
}