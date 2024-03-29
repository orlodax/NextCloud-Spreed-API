﻿using System;
using System.Drawing;

namespace NextCloudAPI.Models
{
    public class Chat                                                           // SEE https://nextcloud-talk.readthedocs.io/en/latest/chat/
    {
        private ulong? _timestamp;

        public uint id { get; set; }
        public string token { get; set; } = string.Empty;                       // token identifier of the conversation which is used for further interaction
        public string actorType { get; set; } = string.Empty;                   // Constants.ActorTypes: "guests" or "users"
        public string actorId { get; set; } = string.Empty;                     // User id of the message author -> User.UserId
        public string actorDisplayName { get; set; } = string.Empty;            // Display name of the message author -> User.displayName
        public ulong? timestamp { get => _timestamp;                            // Timestamp in seconds and UTC time zone
            set { _timestamp = value; SetOrario(); } }                                    
        public object message { get; set; }                                     // Message string with placeholders (see Rich Object String https://github.com/nextcloud/server/issues/1706)
        public string messageType { get; set; } = string.Empty;                 // Constants.ActorTypes: "command" or "comment" or "system"
        public Parameters messageParameters { get; set; } = new Parameters();   // Message parameters for message (see Rich Object String) (see Rich Object String https://github.com/nextcloud/server/issues/1706)
        public string systemMessage { get; set; }                               // Empty for normal chat message or the type of the system message (untranslated) (Constants, commented list)

        public class Parameters
        {
            public Actor actor;
            public class Actor
            {
                public string type { get; set; }
                public string id { get; set; }
                public string name { get; set; }
            }
        }


        /// UI MEMBERS ///
        public string Orario { get; set; }

        public Color ColoreSfondo { get; set; }
        public string MessageAlignment { get; set; }
            
        private void SetOrario()
        {
            if (timestamp.HasValue)
                if (timestamp > 0)
                {
                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime date = start.AddSeconds(Convert.ToInt64(timestamp)).ToLocalTime();
                    Orario = date.ToShortDateString() + " " + date.ToShortTimeString();
                }
        }

        /* ESEMPIO DI RISPOSTA (oggetto Message in Chat)--------- 
        <element>
            <id>59</id>
            <token>upf7gnz6</token>
            <actorType>users</actorType>
            <actorId>pieroprocida</actorId>
            <actorDisplayName>Piero Procida</actorDisplayName>
            <timestamp>1565702323</timestamp>
            <message>Hai abbandonato la chiamata</message>
            <messageParameters>
                <actor>
                    <type>user</type>
                    <id>pieroprocida</id>
                    <name>Piero Procida</name>
                </actor>
            </messageParameters>
            <systemMessage>call_left</systemMessage>
        </element>
         -------------------------------------------------------*/
    }
}
