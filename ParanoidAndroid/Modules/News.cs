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
                //string categoryText = xmlNode["category"].InnerText;
                //string thumbnailUrlText = "https://upload.wikimedia.org/wikipedia/commons/8/87/NRK_Nyheter.png";
                string imageUrlText = "No Image"; 

                //If image attribute is null, replace it with a default image. 
                if (xmlNode["media:content"]!=null)
                {
                    imageUrlText = xmlNode["media:content"].Attributes["url"].Value;
                }
                else
                {
                    imageUrlText = "https://static.nrk.no/nrkno/serum/2.0.476/common/img/nrk-logo-white-72x26.png";
                }
                    
                var embed = new EmbedBuilder
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
                embed.WithAuthor("NRK");
                embed.WithFooter(footer => footer.Text = pubDateText);
                //    embed.WithCurrentTimestamp();


                //Your embed needs to be built before it is able to be sent
                await ReplyAsync(embed: embed.Build());
            }

        }
    }
}
