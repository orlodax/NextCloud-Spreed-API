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
    public class RoomBL : BaseBL
    {
        public RoomBL(User user) : base(user)
        {

        }

        public async Task<List<Room>> GetRoomsForCurrentUser()
        {
            List<Room> rooms = new List<Room>();

            HttpResponseMessage response = await GETRequest("/room");

            if (response != null)
                rooms = DeserializeObjects<Room>(XDocument.Parse(await response.Content.ReadAsStringAsync()), standardPointer);

            return rooms;
        }
        public async Task<Room> GetRoomForCurrentUser(Room Room)
        {
            Room room = new Room();

            HttpResponseMessage response = await GETRequest(String.Format("/room/{0}", Room.token));

            if (response != null)
                room = DeserializeObjects<Room>(XDocument.Parse(await response.Content.ReadAsStringAsync()), standardPointer)[0];

            return room;
        }

        class NewConversation
        {
            public int roomType;
            public string invite;
            public string roomName;
        }
        //roomName/groupName = string.Empty if ConversationType.OneToOne
        public async Task<JObject> CreateRoom(Constants.ConversationTypes conversationType, string userId, string groupName)
        {
            NewConversation nc = new NewConversation()
            {
                roomType = Convert.ToInt32(conversationType),
                invite = userId,
                roomName = groupName
            };

            var jContent = JsonConvert.SerializeObject(nc);

            HttpResponseMessage response = await POSTRequest("/room", jContent);

            if(response!=null)
                return DeserializeResponse(XDocument.Parse(await response.Content.ReadAsStringAsync()));
            else
                return null;
        }
        public async Task<JObject> DeleteRoom(Room Room)
        {
            HttpResponseMessage response = await DELETERequest(string.Format("/room/{0}/participants/self", Room.token));

            if (response != null)
                return DeserializeResponse(XDocument.Parse(await response.Content.ReadAsStringAsync()));
            else
                return null;
        }

    }
}
