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
    public class NewNews : ModuleBase<SocketCommandContext>
    {
        [Command("newnews")] // Command name.
        [Alias("nynyheter")] // Aliases that will also trigger the command.
        [Summary("Make the bot post the latest news.")] // Command summary.

        public async Task NrkNews()
        {
            //Create the XmlDocument.
            XDocument nodeList = XDocument.Load("https://www.nrk.no/nyheter/siste.rss");

            //Get all the descendant elements of the item elements.
            foreach (XElement element in nodeList.Descendants("item").Where(x => x.Element("category")?.Value == "Krigen i Ukraina"))
            {
                if (element != null)
                {
                    XNamespace media = "http://search.yahoo.com/mrss/";

                string titleText = element.Element("title").Value;
                string descriptionText = element.Element("description").Value;
                string urlText = element.Element("link").Value;
                //string pubDateText = element.Element("pubdate").Value;
                //string categoryText = element.Element("category").Value;
               // string thumbnailUrlText = "https://upload.wikimedia.org/wikipedia/commons/8/87/NRK_Nyheter.png";
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
                    //ThumbnailUrl = thumbnailUrlText,
                    ImageUrl = imageUrlText,
                };
                // Or with methods
                news.WithAuthor("NRK");
                //embed.WithFooter(footer => footer.Text = pubDateText);
                //    embed.WithCurrentTimestamp();


                //Your embed needs to be built before it is able to be sent
                await ReplyAsync(embed: news.Build());

                }

                if (element == null)
                {
                    var noNews = new EmbedBuilder
                    {
                        Color = Color.Red,
                        Title = "No News",
                        Description = "No news on this topic right now.",
                    };
                    noNews.WithAuthor("NRK");

                    await ReplyAsync(embed: noNews.Build());
                }
            }

        }
    }
}
