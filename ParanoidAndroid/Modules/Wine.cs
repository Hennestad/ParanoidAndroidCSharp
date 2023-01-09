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
using Newtonsoft.Json;
using System;
using System.Net.NetworkInformation;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net;

namespace ParanoidAndroid.Modules
{
    public class Wine : ModuleBase<SocketCommandContext>
    {
        [Command("wine")] // Command name.
        [Alias("winewine")] // Aliases that will also trigger the command.
        [Summary("Find the closes wine monopoly.")] // Command summary.

        public async Task WineWine([Remainder] string categoryInput)
        {
            // Get the bot token from the Config.json file.
            JObject config = Functions.GetConfig();
            string wineToken = config["wineApi"].Value<string>();

            var client = new HttpClient();


            // Request headers

            client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", wineToken);

            var uri = "https://apis.vinmonopolet.no/stores/v0/details?storeNameContains=" + categoryInput;
            var response = await client.GetAsync(uri);
            string responseString = await response.Content.ReadAsStringAsync();
            JArray result = JArray.Parse(responseString);

            bool foundStore = false;

            foreach (JObject item in result)

            {
                string street = (string)item["address"]["street"];
                string postalCode = (string)item["address"]["postalCode"];
                string city = (string)item["address"]["city"];

                //EmbedBuilder
                var store = new EmbedBuilder
                {
                    // Embed property can be set within object initializer
                    Color = Color.DarkTeal,
                    ThumbnailUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d8/Vinmonopolets_logo.jpg/600px-Vinmonopolets_logo.jpg"
                };
                if (item["storeName"] != null)
                    store.Title = item["storeName"]?.Value<String>();
                if (item["address"] != null)
                    store.Description = street + Environment.NewLine + postalCode + Environment.NewLine + city;



                //Your embed needs to be built before it is able to be sent
                await ReplyAsync(embed: store.Build());

                foundStore = true;
            }


            if (foundStore == false)
            {
                //EmbedBuilder
                var noStore = new EmbedBuilder
                {
                    // Embed property can be set within object initializer
                    Color = Color.DarkTeal,
                    ThumbnailUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d8/Vinmonopolets_logo.jpg/600px-Vinmonopolets_logo.jpg"
                };

                noStore.Title = "Ingen butikker i " + categoryInput;
                noStore.Description = "Her må du brygge din egen alkhol!";

                //Your embed needs to be built before it is able to be sent
                await ReplyAsync(embed: noStore.Build());
            }

        }
    }
}
