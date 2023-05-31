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
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace ParanoidAndroid.Modules
{
    public class Steam : ModuleBase<SocketCommandContext>
    {
        public TelemetryClient telemetry = new TelemetryClient(TelemetryConfiguration.CreateDefault());

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

            var vanityUri = "http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key=" + steamKey + "&vanityurl=" + HttpUtility.UrlEncode(categoryInput);
            var vanityResponse = await client.GetAsync(vanityUri);
            string vanityResponseString = await vanityResponse.Content.ReadAsStringAsync();
            JObject vanityResult = JObject.Parse(vanityResponseString);

            //If the vanityResult is null then the categoryInput is most likely the steamId number itself. 
            string steamId = vanityResult?["response"]?["steamid"]?.ToString() ?? categoryInput;

            try
            {
                var uri = "https://api.steampowered.com/IPlayerService/GetRecentlyPlayedGames/v0001/?key=" + steamKey + "&steamid=" + steamId + "/&format=json";
                var response = await client.GetAsync(uri);
                string responseString = await response.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(responseString);

                string mostPlayedGame = (string)result["response"]["games"][0]["name"];
                int mostPlayedGameMinutes = (int)result["response"]["games"][0]["playtime_2weeks"];
                int mostPlayedGameMinutesForever = (int)result["response"]["games"][0]["playtime_forever"];
                int mostPlayedGameMinutesMac = (int)result["response"]["games"][0]["playtime_mac_forever"];
                string imageNumber = (string)result["response"]["games"][0]["img_icon_url"];
                string appId = (string)result["response"]["games"][0]["appid"];


                //EmbedBuilder
                var steam = new EmbedBuilder
                {
                    // Embed property can be set within object initializer
                    Color = Color.Purple,
                    Title = "Most Played Game The Last Two Weeks: "
                    + Environment.NewLine
                    + mostPlayedGame,
                    Description = "Amount of hours played the last two weeks: " + (mostPlayedGameMinutes / 60)
                    + Environment.NewLine
                    + "Amount of hours spent on this game in total: " + (mostPlayedGameMinutesForever / 60)
                    + Environment.NewLine
                    + "Amount of hours spent on this game in total on Mac: " + (mostPlayedGameMinutesMac / 60) + " :flushed:",
                    ThumbnailUrl = "https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/apps/" + appId + "/" + imageNumber + ".jpg"
                };

                //Your embed needs to be built before it is able to be sent
                await ReplyAsync(embed: steam.Build());
                Console.WriteLine("Posted " + categoryInput + " statistics");
                telemetry.TrackEvent("steam", new Dictionary<string, string>()
                {
                    {"user", Context.User.Username}
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                telemetry.TrackException(ex);
                return;
            }
        }
    }
}
