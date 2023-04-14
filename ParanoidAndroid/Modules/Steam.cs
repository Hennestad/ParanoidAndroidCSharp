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
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic;
using System.Text.Encodings.Web;
using System.Web;

namespace ParanoidAndroid.Modules
{
    public class Steam : ModuleBase<SocketCommandContext>
    {
        [Command("steam")] // Command name.
        [Alias("damp")] // Aliases that will also trigger the command.
        [Summary("Make the bot post information about a steam user.")] // Command summary.

        public async Task SteamUser([Remainder] string categoryInput)
        {
            // Get the bot token from the Config.json file.
            JObject config = Functions.GetConfig();
            string steamKey = config["steamApi"].Value<string>();

            var client = new HttpClient();


            // Request headers

            client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", steamKey);

            var uri = "https://api.steampowered.com/IPlayerService/GetRecentlyPlayedGames/v0001/?key=68D7B04CE54A0A920F3ED50829E56703&steamid=" + HttpUtility.UrlEncode(categoryInput) + "/&format=json";
            var response = await client.GetAsync(uri);
            string responseString = await response.Content.ReadAsStringAsync();
            JObject result = JObject.Parse(responseString);
            string mostPlayedGame = result["response"]["games"][0]["name"].ToString();
            string mostPlayedGameMinutes = result["response"]["games"][0]["playtime_2weeks"].ToString();
            string mostPlayedGameMinutesForever = result["response"]["games"][0]["playtime_forever"].ToString();
            string mostPlayedGameMinutesMac = result["response"]["games"][0]["playtime_mac_forever"].ToString();
            string imageNumber = result["response"]["games"][0]["img_icon_url"].ToString();
            string appId = result["response"]["games"][0]["appid"].ToString();


            //EmbedBuilder
            var steam = new EmbedBuilder
            {
                // Embed property can be set within object initializer
                Color = Color.Purple,
                Title = "Most Played Game The Last Two Weeks: "
                + Environment.NewLine
                + mostPlayedGame,
                Description = "Amount of minutes played the last two weeks: " + mostPlayedGameMinutes
                + Environment.NewLine
                + "Amount of minutes spent on this game in total: " + mostPlayedGameMinutesForever
                + Environment.NewLine
                + "Amount of minutes spent on this game in total on Mac: " + mostPlayedGameMinutesMac + " :flushed:",
                ThumbnailUrl = "https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/apps/" + appId + "/" + imageNumber + ".jpg"
            };

            //Your embed needs to be built before it is able to be sent
            await ReplyAsync(embed: steam.Build());
            Console.WriteLine("Posted " + categoryInput + " statistics");
        }
    }
}
