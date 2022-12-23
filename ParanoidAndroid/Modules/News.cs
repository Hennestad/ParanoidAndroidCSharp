using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Reflection.Emit;
using System.Linq;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace ParanoidAndroid.Modules
{
    public class News : ModuleBase<SocketCommandContext>
    {
        [Command("nyhet")] // Command name.
        [Alias("nyheter")] // Aliases that will also trigger the command.
        [Summary("Make the bot post the latest news in norwegian.")] // Command summary.

        public async Task NrkNews([Remainder] string categoryInput)
        {
            //Create the XmlDocument.
            XDocument nodeList = XDocument.Load("https://www.nrk.no/nyheter/siste.rss");

            //Get all the descendant elements of the item elements.
            foreach (XElement element in nodeList.Descendants("item")
                .Where(x => x.Element("description")?
                .Value.Contains(categoryInput, StringComparison
                .OrdinalIgnoreCase) == true))

            if (!element.IsEmpty)
            {
                XNamespace media = "http://search.yahoo.com/mrss/";

                //EmbedBuilder
                var news = new EmbedBuilder
                {
                    // Embed property can be set within object initializer
                    Color = Color.Blue,
                    ThumbnailUrl = "https://static.nrk.no/nrkno/serum/2.0.482/type/page/img/default.jpg"
                };

                // Or with methods
                if (element.Element(media + "content") != null)
                    news.ImageUrl = element.Element(media + "content")?.Attribute("url")?.Value;

                if (element.Element("title") != null)
                    news.Title = element.Element("title")?.Value;

                if (element.Element("description") != null)
                    news.Description = element.Element("description")?.Value;

                if (element.Element("link") != null)
                    news.Url = element.Element("link")?.Value;

                news.WithAuthor("Siste nytt – NRK");


                //Your embed needs to be built before it is able to be sent
                await ReplyAsync(embed: news.Build());

            }
        }
    }
}
