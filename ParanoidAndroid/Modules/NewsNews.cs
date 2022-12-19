using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParanoidAndroid.Modules
{
    public class NewsNews : ModuleBase<SocketCommandContext>
    {
        [Command("news")] // Command name.
        [Alias("newsnews")] // Aliases that will also trigger the command.
        [Summary("Make the bot post the latest news in english.")] // Command summary.

        public async Task NrkNews([Remainder] string categoryInput)
        {
            //Create the XmlDocument.
            XDocument nodeList = XDocument.Load("http://feeds.bbci.co.uk/news/world/rss.xml");

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
                        imageUrlText = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4e/BBC_News_2022_%28Alt%2C_boxed%29.svg/240px-BBC_News_2022_%28Alt%2C_boxed%29.svg.png";
                    }

                    var news = new EmbedBuilder
                    {
                        // Embed property can be set within object initializer
                        Color = Color.DarkRed,
                        Title = titleText,
                        Description = descriptionText,
                        Url = urlText,
                        ImageUrl = imageUrlText,
                    };

                    // Or with methods
                    news.WithAuthor("Latest News – BBC");


                    //Your embed needs to be built before it is able to be sent
                    await ReplyAsync(embed: news.Build());

                }
            }

        }
    }
}
