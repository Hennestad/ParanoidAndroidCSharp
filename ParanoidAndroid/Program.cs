using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Microsoft.ApplicationInsights;

namespace ParanoidAndroid
{
    public class Program
    {
        public TelemetryClient? telemetryClient;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using var services = ConfigureServices();

            telemetryClient = Telemtry.GetClient();

            Console.WriteLine("Ready for takeoff...");
            var client = services.GetRequiredService<DiscordSocketClient>();

            client.Log += Log;
            services.GetRequiredService<CommandService>().Log += Log;

            // Get the bot token from the Config.json file.
            JObject config = Functions.GetConfig();
            string? token = config["token"]!.Value<string>();

            // Log in to Discord and start the bot.
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            // Run the bot forever.
            await Task.Delay(-1);
        }

        public ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    MessageCacheSize = 500,
                    LogLevel = LogSeverity.Info
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    LogLevel = LogSeverity.Info,
                    DefaultRunMode = RunMode.Async,
                    CaseSensitiveCommands = false
                }))
                .AddSingleton<CommandHandlingService>()
                .BuildServiceProvider();
        }

        private Task Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }
}
