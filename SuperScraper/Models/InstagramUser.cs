using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperScraper.Models
{
    public class InstagramUser
    {
        public string FullName { get; set; }
        public int FollowerCount { get; set; }
        public int FollowingCount { get; set; }

        /// <summary>
        /// Display this user in the console
        /// </summary>
        public void Display()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"-------------------------------------");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Full name: {FullName}");
            Console.WriteLine($"Followers: {FollowerCount}");
            Console.WriteLine($"Following: {FollowingCount}");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"-------------------------------------");
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
