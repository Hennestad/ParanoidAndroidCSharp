using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Web;

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

            //Are there any news?
            bool foundNews = false;

            //Get all the descendant elements of the item elements.
            foreach (XElement element in nodeList.Descendants("item")
                .Where(x => x.Element("description")?
                .Value.Contains(HttpUtility.UrlEncode(categoryInput), StringComparison
                .OrdinalIgnoreCase) == true))
            {
                XNamespace media = "http://search.yahoo.com/mrss/";

                //EmbedBuilder
                var news = new EmbedBuilder
                {
                    // Embed property can be set within object initializer
                    Color = Color.Blue,
                    ThumbnailUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4e/BBC_News_2022_%28Alt%2C_boxed%29.svg/240px-BBC_News_2022_%28Alt%2C_boxed%29.svg.png"
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

                news.WithAuthor("Latest News – BBC");


                //Your embed needs to be built before it is able to be sent
                await ReplyAsync(embed: news.Build());

                foundNews = true;
            }

            if (foundNews == false)
            {
                //EmbedBuilder
                var noNews = new EmbedBuilder
                {
                    // Embed property can be set within object initializer
                    Color = Color.Blue,
                    ThumbnailUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4e/BBC_News_2022_%28Alt%2C_boxed%29.svg/240px-BBC_News_2022_%28Alt%2C_boxed%29.svg.png"
                };

                noNews.Title = "No news about " + categoryInput;

                noNews.WithAuthor("The Latest News – BBC");

                //Your embed needs to be built before it is able to be sent
                await ReplyAsync(embed: noNews.Build());
            }

        }
    }
}
