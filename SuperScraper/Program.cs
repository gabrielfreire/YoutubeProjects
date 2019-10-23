using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperScraper.Models;

namespace SuperScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // receive a profile name
            var tasks = new List<Task<InstagramUser>>();
            foreach(var arg in args)
            {
                if (string.IsNullOrEmpty(arg)) continue;
                var profileName = arg;
                var url = $"https://instagram.com/{profileName}";
                tasks.Add(ScrapeInstagram(url));
            }

            try
            {
                var instagramUsers = await Task.WhenAll<InstagramUser>(tasks);
                foreach(var iu in instagramUsers)
                {
                    iu.Display();
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public static async Task<InstagramUser> ScrapeInstagram(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    // create html document
                    var htmlBody = await response.Content.ReadAsStringAsync();
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlBody);

                    // select script tags
                    var scripts = htmlDocument.DocumentNode.SelectNodes("/html/body/script");

                    // preprocess result
                    var uselessString = "window._sharedData = ";
                    var scriptInnerText = scripts[0].InnerText
                        .Substring(uselessString.Length)
                        .Replace(";", "");

                    // serialize objects and fetch the user data
                    dynamic jsonStuff = JObject.Parse(scriptInnerText);
                    dynamic userProfile = jsonStuff["entry_data"]["ProfilePage"][0]["graphql"]["user"];

                    // create an InstagramUser
                    var instagramUser = new InstagramUser
                    {
                        FullName = userProfile.full_name,
                        FollowerCount = userProfile.edge_followed_by.count,
                        FollowingCount = userProfile.edge_follow.count
                    };
                    return instagramUser;
                } else
                {
                    throw new Exception($"Something wrong happened {response.StatusCode} - {response.ReasonPhrase} - {response.RequestMessage}");
                }
            }
        }
    }
}
