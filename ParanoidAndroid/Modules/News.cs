using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Reflection.Emit;
using System.Linq;
using System.Xml.Linq;

namespace ParanoidAndroid.Modules
{
    public class News : ModuleBase<SocketCommandContext>
    {
        [Command("news")] // Command name.
        [Alias("nyheter")] // Aliases that will also trigger the command.
        [Summary("Make the bot post the latest news.")] // Command summary.

        public async Task NrkNews([Remainder] string categoryInput)
        {
            //Create the XmlDocument.
            XDocument nodeList = XDocument.Load("https://www.nrk.no/nyheter/siste.rss");

            //Get all the descendant elements of the item elements.
            foreach (XElement element in nodeList.Descendants("item").Where(x => x.Element("description")?.Value.Contains(categoryInput, StringComparison.OrdinalIgnoreCase) == true))
            {
                if (element != null)
                {
                    XNamespace media = "http://search.yahoo.com/mrss/";

                    string titleText = element.Element("title").Value;
                    string descriptionText = element.Element("description").Value;
                    string urlText = element.Element("link").Value;
                    string imageUrlText = "No Image";

                    //If image attribute is null, replace it with a default image. 
                    if (element.Element(media + "content") != null)
                    {
                        imageUrlText = element.Element(media + "content").Attribute("url").Value;
                    }
                    else
                    {
                        imageUrlText = "https://static.nrk.no/nrkno/serum/2.0.476/common/img/nrk-logo-white-72x26.png";
                    }

                    var news = new EmbedBuilder
                    {
                        // Embed property can be set within object initializer
                        Color = Color.Blue,
                        Title = titleText,
                        Description = descriptionText,
                        Url = urlText,
                        ImageUrl = imageUrlText,
                    };

                    // Or with methods
                    news.WithAuthor("Siste nytt – NRK");


                    //Your embed needs to be built before it is able to be sent
                    await ReplyAsync(embed: news.Build());

                }
            }

        }
    }
}
