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
    public class ChatBL : BaseBL
    {
        public ChatBL(User user) : base(user)
        {

        }

        public async Task<List<Chat>> GetChatsInRoom(Room Room)
        {
            List<Chat> chats = new List<Chat>();

            HttpResponseMessage response = await GETRequest(string.Format
                ("/chat/{0}?lastKnownMessageId=60&limit=100&lookIntoFuture=1", Room.token));
            
            if (response != null)
                chats = DeserializeObjects<Chat>(XDocument.Parse(await response.Content.ReadAsStringAsync()), standardPointer);
              
            return chats;
        }

        public async Task<Chat> SendChat(Room Room, Chat Chat)
        {
            Chat chat = new Chat();

            var jContent = JsonConvert.SerializeObject(Chat);

            HttpResponseMessage response = await POSTRequest(string.Format
                ("/chat/{0}", Room.token), jContent);

            if (response != null)
                chat = DeserializeObjects<Chat>(XDocument.Parse(await response.Content.ReadAsStringAsync()), standardPointer)[0];

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

