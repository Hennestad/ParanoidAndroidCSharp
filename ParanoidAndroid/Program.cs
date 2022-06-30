using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ParanoidAndroid
{
	public class Program
	{
		public static Task Main(string[] args) => new Program().MainAsync();

        private DiscordSocketClient Client;

        public async Task MainAsync()
		{
            Client = new DiscordSocketClient();

            Client.Log += Log;

            // var token = File.ReadAllText("token.txt");
            var token = File.ReadAllText("../../../token.txt");

            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}

    }

}