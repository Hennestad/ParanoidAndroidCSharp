using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ParanoidAndroid
{
    public class FunSample : ModuleBase<SocketCommandContext>
    {

        [Command("hello")] // Command name.
        [Alias("hei")] // Aliases that will also trigger the command.
        [Summary("Say hello to the bot.")] // Command summary.
        public async Task Hello()
        {
            if (Context.Message.Author.Id == 184787955238436864)
                await ReplyAsync($"Hei Anders! Visste du dette?: Mao Zedong (tradisjonell kinesisk: 毛澤東, " +
                    $"forenklet kinesisk: 毛泽东, pinyin: Máo Zédōng; Wade-Giles: Mao Tse-tung," +
                    $"født 26. desember 1893 i Shaoshan i Hunan i Kina, død 9. september 1976 i " +
                    $"Beijing) var en kinesisk kommunist, statsleder, revolusjonær og" +
                    $"teoretiker. Han ledet Kinas kommunistparti til seier i den kinesiske " +
                    $"borgerkrigen og grunnla Folkerepublikken Kina i 1949, som han ledet fram " +
                    $"til sin død i 1976. Mao er også grunnleggeren av en retning innen " +
                    $"marxismen-leninismen kjent som maoismen." +
                    $"Her har du ett bilde av han! :)" +
                    $"\n" +
                    $"https://media.snl.no/media/67119/standard_compressed_sz8318b2.jpg");
            else
                await ReplyAsync($"Hello there, **{Context.User.Username}**!");
        }

        [Command("pick")]
        [Alias("choose")]
        [Summary("Pick something.")]
        public async Task Pick([Remainder] string message = "")
        {
            string[] options = message.Split(new string[] { " or " }, StringSplitOptions.RemoveEmptyEntries);
            string selection = options[new Random().Next(options.Length)];

            // ReplyAsync() is a shortcut for Context.Channel.SendMessageAsync()
            await ReplyAsync($"I choose **{selection}**");
        }

        [Command("admin???")]
        [Summary("Check your administrator status")]
        public async Task AmIAdmin()
        {
            if ((Context.User as SocketGuildUser).GuildPermissions.Administrator)
                await ReplyAsync($"Yes, **{Context.User.Username}**, you're an admin!");
            else
                await ReplyAsync($"No, **{Context.User.Username}**, you're **not** an admin!");
        }
    }
}