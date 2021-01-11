using System.IO;

namespace VKParse
{
    internal static class Commands
    {
        private static readonly string Token;

        private static readonly string BaseUrl;

        static Commands()
        {
            Token = File.ReadAllText("./token.txt");
            BaseUrl = "https://api.vk.com/method/";
        }

        public static string UsersGet(string id)
        {
            var request = $"{BaseUrl}users.get?user_ids={id}&fields=city,nickname&access_token={Token}&v=5.126";
            return request;
        }

        public static string FriendsGet(string id)
        {
            var request = $"{BaseUrl}friends.get?user_id={id}&fields=city,nickname&access_token={Token}&v=5.126";
            return request;
        }
    }
}