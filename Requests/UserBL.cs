using Microsoft.Json.Pointer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NextCloudAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NextCloudAPI.Requests
{
    public class UserBL : BaseBL
    {
        public UserBL(User user) : base(user)
        {

        }

        public async Task<List<User>> GetUsersInRoom(Room Room)
        {
            List<User> users = new List<User>();

            HttpResponseMessage response = await GETRequest(String.Format
                ("/room/{0}/participants", Room.token));

            if (response != null)
                users = DeserializeObjects<User>(XDocument.Parse(await response.Content.ReadAsStringAsync()), standardPointer);

            return users;
        }
    }
}
