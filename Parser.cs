using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace VKParse
{
    internal static class Parser
    {
        private static readonly HttpClient Client = new HttpClient();

        private static Person _zeroPerson;
        private static readonly List<Person> Friends = new List<Person>();

        private static async Task GetZeroPerson(string id)
        {
            var request = Commands.UsersGet(id);
            var response = await Client.GetAsync(request);

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json)["response"]?.First;

            var firstName = jsonObject?["first_name"]?.ToString();
            var lastName = jsonObject?["last_name"]?.ToString();
            var middleName = jsonObject?["nickname"]?.ToString();
            var city = jsonObject?["city"]?["title"]?.ToString();

            _zeroPerson = new Person(id, firstName, lastName, middleName, city);
        }

        private static async Task GetFriendsDataAsync(string id, List<Person> parentFriends, List<Person> childFriends, Person zeroPerson = null)
        {
            var request = Commands.FriendsGet(id);
            var response = await Client.GetAsync(request);

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json)?["response"]?["items"];

            var listIds = new List<string>();

            if (jsonObject != null)
            {
                foreach (var item in jsonObject)
                {
                    listIds.Add(item["id"]?.ToString());

                    if (childFriends != null)
                    {
                        var currentId = item?["id"]?.ToString();
                        var firstName = item?["first_name"]?.ToString();
                        var lastName = item?["last_name"]?.ToString();
                        var middleName = item?["nickname"]?.ToString();
                        var city = item?["city"]?["title"]?.ToString();
                    
                        childFriends.Add(new Person(currentId, firstName, lastName, middleName, city));
                    }
                }
                if (zeroPerson != null)
                    _zeroPerson.FriendsIds.AddRange(listIds);
                else
                {
                    var currentPerson = parentFriends?.Find((person => person.Id == id));
                    currentPerson?.FriendsIds.AddRange(listIds);
                }
            }
        }

        private static void Write(List<Person> listFriends)
        {
            var info = _zeroPerson.ToString();
            File.AppendAllText("text.txt", info);

            foreach (var item in Friends)
            {
                info = item.ToString();
                File.AppendAllText("text.txt", info);
            }

            foreach (var item in listFriends)
            {
                info = item.ToString();
                File.AppendAllText("text.txt", info);
            }
        }

        public static async Task Run(string id)
        {
            await GetZeroPerson(id);
            await GetFriendsDataAsync(id, null, Friends, _zeroPerson);

            var listFriends = new List<Person>();
            foreach (var item in Friends)
                await GetFriendsDataAsync(item.Id, Friends, listFriends);
            
            foreach (var subItem in listFriends)
                await GetFriendsDataAsync(subItem.Id, listFriends, null);
            
            Write(listFriends);
        }
    }
}