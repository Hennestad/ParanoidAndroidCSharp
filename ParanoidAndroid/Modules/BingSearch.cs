using Discord.Commands;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanoidAndroid.Modules
{
    public class BingSearch : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Recieve a ping message.")]
        public async Task HandlePingCommand()
        {
            await RespondAsync("Ping");
        }
    }


}
