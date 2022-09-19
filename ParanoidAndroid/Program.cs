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
            XmlNodeList nodeList = doc.GetElementsByTagName("item");
            foreach (XmlNode xmlNode in nodeList)
            {
                string titleText = xmlNode["title"].InnerText;
                string descriptionText = xmlNode["description"].InnerText;
                string urlText = xmlNode["link"].InnerText;
                string pubDateText = xmlNode["pubDate"].InnerText;
                string categoryText = xmlNode["category"].InnerText;
                string mediaUrl = xmlNode["media:content"].Attributes["url"].Value;



                var embed = new EmbedBuilder
                {
                    // Embed property can be set within object initializer
                    Color = Color.Blue,
                    Title = titleText,
                    Description = descriptionText,
                    Url = urlText,
                    ImageUrl = mediaUrl,

                    //Description = "I am a description set by initializer."
                };
                // Or with methods
                //embed.AddField("Title", "Field value. I also support [hyperlink markdown](https://example.com)!")
                //    .WithAuthor(Context.Client.CurrentUser)
                embed.WithFooter(footer => footer.Text = pubDateText + $"\n" + categoryText);
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
