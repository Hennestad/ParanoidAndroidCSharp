using System;
using System.Threading; // 1) Add this namespace
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

public class TimerService
{
    private readonly Timer _timer; // 2) Add a field like this
    // This example only concerns a single timer.
    // If you would like to have multiple independant timers,
    // you could use a collection such as List<Timer>,
    // or even a Dictionary<string, Timer> to quickly get
    // a specific Timer instance by name.

    public TimerService(DiscordSocketClient client)
    {
        _timer = new Timer(async _ =>
        {
            // 3) Any code you want to periodically run goes here, for example:
            var chan = client.GetChannel(812099435143495681) as IMessageChannel;
            if (chan != null)
                await chan.SendMessageAsync("hi");
        },
        null,
        TimeSpan.FromMinutes(10),  // 4) Time that message should fire after the timer is created
        TimeSpan.FromMinutes(30)); // 5) Time after which message should repeat (use `Timeout.Infinite` for no repeat)
    }

    public void Stop() // 6) Example to make the timer stop running
    {
        _timer.Change(Timeout.Infinite, Timeout.Infinite);
    }

    public void Restart() // 7) Example to restart the timer
    {
        _timer.Change(TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(30));
    }
}

public class TimerModule : ModuleBase
{
    private readonly TimerService _service;

    public TimerModule(TimerService service) // Make sure to configure your DI with your TimerService instance
    {
        _service = service;
    }

    // Example commands
    [Command("stoptimer")]
    public async Task StopCmd()
    {
        _service.Stop();
        await ReplyAsync("Timer stopped.");
    }

    [Command("starttimer")]
    public async Task RestartCmd()
    {
        _service.Restart();
        await ReplyAsync("Timer (re)started.");
    }
}