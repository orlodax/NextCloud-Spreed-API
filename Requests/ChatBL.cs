using Microsoft.Json.Pointer;
using Newtonsoft.Json;
using NextCloudAPI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NextCloudAPI.Requests
{
    public class ChatBL
    {
        private RequestsBL RequestsBL;
        
        private JsonPointer dataPointer = new JsonPointer("/ocs/data");
        private JsonPointer elementPointer = new JsonPointer("/ocs/data/element");
        
        public ChatBL(RequestsBL requestsBL)
        {
            RequestsBL = requestsBL;
        }
        public async Task<List<Chat>> GetChatsInRoom(Room Room)
        {
            List<Chat> chats = new List<Chat>();

            HttpResponseMessage response = await RequestsBL.GETRequest(string.Format
                ("/chat/{0}?lastKnownMessageId=0&limit=100&lookIntoFuture=1", Room.token), Constants.BaseRequestStub);

            if (response != null)
                if (response.IsSuccessStatusCode)
                    chats = RequestsBL.DeserializeObjects<Chat>(XDocument.Parse(await response.Content.ReadAsStringAsync()), elementPointer);

            return chats;
        }

        public async Task<Chat> SendChat(Room Room, Chat Chat)
        {
            Chat chat = new Chat();

            var jContent = JsonConvert.SerializeObject(Chat);

            HttpResponseMessage response = await RequestsBL.POSTRequest(string.Format
                ("/chat/{0}", Room.token), jContent, Constants.BaseRequestStub);

            if (response != null)
                if (response.IsSuccessStatusCode)
                    chat = RequestsBL.DeserializeObjects<Chat>(XDocument.Parse(await response.Content.ReadAsStringAsync()), dataPointer)[0];

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
    }
}
