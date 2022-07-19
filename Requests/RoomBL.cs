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
    public class RoomBL
    {
        private RequestsBL RequestsBL;

        private JsonPointer ocsPointer = new JsonPointer("/ocs");
        private JsonPointer elementPointer = new JsonPointer("/ocs/data/element");

        public RoomBL(RequestsBL requestsBL)
        {
            RequestsBL = requestsBL;
        }

        public async Task<List<Room>> GetRoomsForCurrentUser()
        {
            List<Room> rooms = new List<Room>();

            HttpResponseMessage response = await RequestsBL.GETRequest("/room", RequestsBL.Constants.BaseRequestStub);

            if (response != null)
                if (response.IsSuccessStatusCode)
                    rooms = RequestsBL.DeserializeObjects<Room>(XDocument.Parse(await response.Content.ReadAsStringAsync()), elementPointer);

            return rooms;
        }
        public async Task<Room> GetRoomForCurrentUser(Room Room)
        {
            Room room = new Room();

            HttpResponseMessage response = await RequestsBL.GETRequest(String.Format("/room/{0}", Room.token), RequestsBL.Constants.BaseRequestStub);

            if (response != null)
                if (response.IsSuccessStatusCode)
                    room = RequestsBL.DeserializeObjects<Room>(XDocument.Parse(await response.Content.ReadAsStringAsync()), elementPointer)[0];

            return room;
        }

        class NewConversation
        {
            public int roomType;
            public string invite;
            public string roomName;
        }
        //roomName/groupName = string.Empty if ConversationType.OneToOne
        public async Task<string> CreateRoom(Constants.ConversationTypes conversationType, string userId, string groupName)
        {
            NewConversation nc = new NewConversation()
            {
                roomType = Convert.ToInt32(conversationType),
                invite = userId,
                roomName = groupName
            };

            var jContent = JsonConvert.SerializeObject(nc);

            HttpResponseMessage response = await RequestsBL.POSTRequest("/room", jContent, RequestsBL.Constants.BaseRequestStub);

            if (response != null)
            {
                string jResponse = JsonConvert.SerializeXNode(XDocument.Parse(await response.Content.ReadAsStringAsync()));
                JToken documentToken = JToken.Parse(jResponse);

                var roomContainer = documentToken["ocs"]["data"]["token"].AsJEnumerable();

                if (roomContainer != null)
                    return roomContainer.ToString();
                else
                    return null;
            }
            else
                return null;
        }
        public async Task<JObject> DeleteRoom(Room Room)
        {
            HttpResponseMessage response = await RequestsBL.DELETERequest(string.Format("/room/{0}/participants/self", Room.token), RequestsBL.Constants.BaseRequestStub);

            if (response != null)
                if (response.IsSuccessStatusCode)
                    return RequestsBL.DeserializeResponse(XDocument.Parse(await response.Content.ReadAsStringAsync()), ocsPointer);
                else
                    return null;
            else
                return null;
        }
    }
}
