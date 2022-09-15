using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Reflection.Emit;

namespace ParanoidAndroid.Modules
{
    public class News : ModuleBase<SocketCommandContext>
    {
        [Command("news")] // Command name.
        [Alias("nyheter")] // Aliases that will also trigger the command.
        [Summary("Make the bot post the latest news.")] // Command summary.

        public async Task NrkNews()
        {
            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load("https://www.nrk.no/nyheter/siste.rss");

            //Display all the titles.
            XmlNodeList titleList = doc.GetElementsByTagName("title");
            foreach (XmlNode title in titleList)
            {
                string titleText = title.InnerText;

                var embed = new EmbedBuilder
                {
                    // Embed property can be set within object initializer
                    Title = titleText,
                    Color = Color.Blue,
                    //Description = "I am a description set by initializer."
                };
                // Or with methods
                //embed.AddField("Title", "Field value. I also support [hyperlink markdown](https://example.com)!")
                //    .WithAuthor(Context.Client.CurrentUser)
                //    .WithFooter(footer => footer.Text = "I am a footer.")
                //    .WithColor(Color.DarkRed)
                //    .WithDescription("I am a description.")
                //    .WithUrl("https://example.com")
                //    .WithCurrentTimestamp();


                //Your embed needs to be built before it is able to be sent
                await ReplyAsync(embed: embed.Build());
            }

        }
    }
}
