using System.Threading.Tasks;
using Discord.Interactions;

namespace Discord_Bot.Modules
{
    public class TestModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("test-slash", "Echo an input")]
        public async Task Echo(string input)
        {
            if (Context.User.Id != 305330357937635329)
            {
                await RespondAsync("No permission", ephemeral: true);
                return;
            }
            await RespondAsync("slash");
        }
        
        [SlashCommand("test-admin", "Echo an input")]
        public async Task TestAdmin()
        {
            if (Context.User.Id != 305330357937635329)
            {
                await RespondAsync("No permission", ephemeral: true);
                return;
            }
            await RespondAsync("admin");
        }
    }
}