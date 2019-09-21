using Microsoft.Json.Pointer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NextCloudAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NextCloudAPI.Requests
{
    public class RequestsBL
    {
        #region PROPERTIES

        private User User;
        private string RequestStub;
        private string NotificationsRequestStub;
        private string AuthorizationHeader;
        private HttpClient HttpClient = new HttpClient();

        internal JsonPointer standardPointer = new JsonPointer("/ocs/data/element");
        internal JsonPointer responsePointer = new JsonPointer("/ocs");

        #endregion

        #region CONSTRUCTOR
        public RequestsBL(User user)
        {
            User = user;

            RequestStub = String.Format("https://{0}{1}", Constants.BaseUrl, Constants.EndPoint);
            NotificationsRequestStub = String.Format("https://{0}{1}", Constants.BaseUrl, Constants.NotificationsEndPoint);

            AuthorizationHeader = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", User.userId, User.password)));
        }
        #endregion

        #region UNROLLERS
        internal List<T> DeserializeObjects<T>(XDocument xResponse)
        {
            List<T> objects = new List<T>();

            string jResponse = JsonConvert.SerializeXNode(xResponse);               //transforms xml to json string
            JToken documentToken = JToken.Parse(jResponse);                         //loads it as jtoken (jobject)
            JToken elementToken = standardPointer.Evaluate(documentToken);          //selects only the element node(s)

            if (elementToken is JArray)
                objects = JsonConvert.DeserializeObject<List<T>>(elementToken.ToString());      //if multiple nodes (array)
            else if (elementToken is JObject)
                objects.Add(JsonConvert.DeserializeObject<T>(elementToken.ToString()));         //if single nodes

            return objects;
        }

        internal JObject DeserializeResponse(XDocument xResponse)
        {
            string jResponse = JsonConvert.SerializeXNode(xResponse);
            JToken documentToken = JToken.Parse(jResponse);
            JToken genericToken = responsePointer.Evaluate(documentToken);

            return (JObject)JsonConvert.DeserializeObject(genericToken.ToString());
        }
        #endregion

        #region BASE REQUESTS
            #region Common Methods for all requests
            HttpRequestMessage AddHeaders(HttpRequestMessage req)
            {
                req.Version = HttpVersion.Version11;
                req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", AuthorizationHeader);
                req.Headers.Connection.Add("keep-alive");
                req.Headers.Add("Accept-Encoding", "gzip,deflate");
                req.Headers.TryAddWithoutValidation("OCS-APIRequest", "true");

                return req;
            }
            async Task<string> RequestError(HttpResponseMessage response)
            {
                string str = string.Empty;
                string error = string.Empty;
                if (response.Content != null)
                {
                    str = await response.Content.ReadAsStringAsync();
                    error = string.Format("ERROR: " + response.StatusCode + " " + response.ReasonPhrase + "\r\n" + str);
                }
                return error;
            }
            async Task<HttpResponseMessage> Respond(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                    return response;
                else
                {
                    await RequestError(response);
                    return null;
                }
            }
            #endregion
        public async Task<HttpResponseMessage> GETRequest(string queryEndpoint)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, new Uri(RequestStub + queryEndpoint));
            req = AddHeaders(req);

            var response = await HttpClient.SendAsync(req);

            return await Respond(response);
        }
        public async Task<HttpResponseMessage> GETRequestNotifications(string queryEndpoint)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, new Uri(NotificationsRequestStub + queryEndpoint));
            req = AddHeaders(req);

            var response = await HttpClient.SendAsync(req);

            return await Respond(response);
        }
        public async Task<HttpResponseMessage> POSTRequest(string queryEndpoint, string jContent)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, new Uri(RequestStub + queryEndpoint));
            req = AddHeaders(req);

            req.Content = new StringContent(jContent, Encoding.UTF8, "application/json");

            var response = await HttpClient.SendAsync(req);

            return await Respond(response);
        }
        public async Task<HttpResponseMessage> DELETERequest(string queryEndpoint)
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, new Uri(RequestStub + queryEndpoint));
            req = AddHeaders(req);

            var response = await HttpClient.SendAsync(req);

            return await Respond(response);
        }

        #endregion

        #region ROOM
        public async Task<List<Room>> GetRoomsForCurrentUser()
        {
            List<Room> rooms = new List<Room>();

            HttpResponseMessage response = await GETRequest("/room");

            if (response != null)
                rooms = DeserializeObjects<Room>(XDocument.Parse(await response.Content.ReadAsStringAsync()));

            return rooms;
        }
        public async Task<Room> GetRoomForCurrentUser(Room Room)
        {
            Room room = new Room();

            HttpResponseMessage response = await GETRequest(String.Format("/room/{0}", Room.token));

            if (response != null)
                room = DeserializeObjects<Room>(XDocument.Parse(await response.Content.ReadAsStringAsync()))[0];

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

            if (response != null)
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
        #endregion

        #region CHAT
        public async Task<List<Chat>> GetChatsInRoom(Room Room)
        {
            List<Chat> chats = new List<Chat>();

            HttpResponseMessage response = await GETRequest(string.Format
                ("/chat/{0}?lastKnownMessageId=60&limit=100&lookIntoFuture=1", Room.token));

            if (response != null)
                chats = DeserializeObjects<Chat>(XDocument.Parse(await response.Content.ReadAsStringAsync()));

            return chats;
        }

        public async Task<Chat> SendChat(Room Room, Chat Chat)
        {
            Chat chat = new Chat();

            var jContent = JsonConvert.SerializeObject(Chat);

            HttpResponseMessage response = await POSTRequest(string.Format
                ("/chat/{0}", Room.token), jContent);

            if (response != null)
                chat = DeserializeObjects<Chat>(XDocument.Parse(await response.Content.ReadAsStringAsync()))[0];

            return chat;
        }

        //THIS DOES NOT APPEAR TO BE EVER CALLED, USING FIREFOX CONSOLE 29 AUGUST 2019
        //public async Task<Chat> MarkChatAsRead(Room Room)
        //{
        //    Chat chat = new Chat();

        //    HttpResponseMessage response = await POSTRequest(string.Format
        //        ("/chat/{0}/read", Room.token), "{ \"lastReadMessage\" = "+ Room.lastMessage.id + " }");

        //    if (response != null)
        //        return chat;
        //        //fare che setta anche localmente il messaggio come letto

        //    return chat;
        //}
        //   {"token":"upf7gnz6","message":"ciao","actorType":"","actorId":"","actorDisplayName":"","timestamp":0,"messageParameters":[]
        #endregion

        #region USER
        public async Task<List<User>> GetUsersInRoom(Room Room)
        {
            List<User> users = new List<User>();

            HttpResponseMessage response = await GETRequest(String.Format
                ("/room/{0}/participants", Room.token));

            if (response != null)
                users = DeserializeObjects<User>(XDocument.Parse(await response.Content.ReadAsStringAsync()));

            return users;
        }
        #endregion

        #region NOTIFIES
        public async Task<List<Notify>> GetNotifies()
        {
            List<Notify> notifies = new List<Notify>();

            HttpResponseMessage response = await GETRequestNotifications("/notifications");

            if (response != null)
                notifies = DeserializeObjects<Notify>(XDocument.Parse(await response.Content.ReadAsStringAsync()));

            return notifies;
        }
        #endregion
    }
}
