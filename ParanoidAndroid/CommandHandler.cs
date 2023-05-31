using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace ParanoidAndroid
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {

            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            // Event handlers
            _client.SlashCommandExecuted += SlashCommandHandler;
            _client.Ready += ClientReadyAsync;
            _client.MessageReceived += HandleCommandAsync;
            _client.JoinedGuild += SendJoinMessageAsync;
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await command.RespondAsync($"You executed {command.Data.Name}");
        }

        private async Task HandleCommandAsync(SocketMessage rawMessage)
        {
            if (rawMessage.Author.IsBot || !(rawMessage is SocketUserMessage message) || message.Channel is IDMChannel)
                return;

            var context = new SocketCommandContext(_client, message);

            //Prefix commands

            int argPos = 0;

            JObject config = Functions.GetConfig();
            string[]? prefixes = JsonConvert.DeserializeObject<string[]>(config["prefixes"]!.ToString());

            // Check if message has any of the prefixes or mentiones the bot.
            if (prefixes!.Any(x => message.HasStringPrefix(x, ref argPos)) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                // Execute the command.
                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess && result.Error.HasValue)
                    await context.Channel.SendMessageAsync($":x: {result.ErrorReason}");
            }

            //Slash commands
            ulong ripskinzId = config["ripskinzId"]!.Value<ulong>();
            var guild = _client.GetGuild(ripskinzId);
            var guildCommand = new SlashCommandBuilder();

            // Command names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
            guildCommand.WithName("first-command");
            guildCommand.WithDescription("This is my first guild slash command!");

            try
            {
                await guild.CreateApplicationCommandAsync(guildCommand.Build());
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }

        private async Task SendJoinMessageAsync(SocketGuild guild)
        {
            JObject config = Functions.GetConfig();
            string? joinMessage = config["join_message"]!.Value<string>();

            if (string.IsNullOrEmpty(joinMessage))
                return;

            // Send the join message in the first channel where the bot can send messsages.
            foreach (var channel in guild.TextChannels.OrderBy(x => x.Position))
            {
                var botPerms = channel.GetPermissionOverwrite(_client.CurrentUser).GetValueOrDefault();

                if (botPerms.SendMessages == PermValue.Deny)
                    continue;

                try
                {
                    await channel.SendMessageAsync(joinMessage);
                    return;
                }
                catch
                {
                    continue;
                }
            }
        }

        private async Task ClientReadyAsync()
            => await Functions.SetBotStatusAsync(_client);

        public async Task InitializeAsync()
            => await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }
}