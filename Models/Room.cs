using System.Collections.Generic;

namespace NextCloudAPI.Models
{
    public class Room                                           // SEE https://nextcloud-talk.readthedocs.io/en/latest/conversation/
    {
        public uint id { get; set; }
        public string token { get; set; } = string.Empty;       // token identifier of the conversation which is used for further interaction
        public uint type { get; set; }
        public string name { get; set; } = string.Empty;        // name of the conversation (can also be empty)
        public string displayName { get; set; } = string.Empty; // =name if non empty, otherwise it falls back to a list of participants
        public byte participantType { get; set; }               // Permissions level of the current user - SEE Constants.cs            
        public uint participantFlags { get; set; }              // Flags of the current user
        public byte readOnly { get; set; }                      // Either 0 or 1
        public ushort count { get; set; }                       // Number of active users
        public ushort numGuests { get; set; }                   // Number of active guests
        public ulong lastPing { get; set; }                     // Timestamp of the last ping of the current user (should be used for sorting)
        public string sessionId { get; set; }                   // '0' if not connected, otherwise a 512 character long string
        public bool? hasPassword { get; set; }                  // Flag if the conversation has a password
        public bool? hasCall { get; set; }                      // Flag if the conversation has an active call
        public ulong lastActivity { get; set; }                 // Timestamp of the last activity in the conversation, in seconds and UTC time zone
        public bool? isFavorite { get; set; }                   // Flag if the conversation is favorited by the user
        public byte notificationLevel { get; set; }             // The notification level for the user (one of Participant::NOTIFY_* (1-3))
        public uint unreadMessages { get; set; }                // Number of unread chat messages in the conversation (only available with chat-v2 capability)
        public bool? unreadMention { get; set; }                // Flag if the user was mentioned since their last visit
        public uint lastReadMessage { get; set; }               // ID of the last read message in a room
        public Chat lastMessage { get; set; } = new Chat();     // Last message in a conversation if available, otherwise empty
        public string objectType { get; set; } = string.Empty;  // The type of object that the conversation is associated with; "share:password" if the conversation is used to request a password for a share, otherwise empty
        public string objectId { get; set; } = string.Empty;    // Share token if "objectType" is "share:password", otherwise empty
        
        public object participants { get; set; }                // fake object to hold serialization spam

        //to hold participant: NOTE --- api won't respond consistently so deserializing does not work (requires to much extra work to map different jsonattributes)
        //please use GetUsersInRoom(Room Room) in UserBL.cs to populate this
        public List<User> Participants { get; set; } = new List<User>();


        /* ESEMPIO DI RISPOSTA (oggetto Room)--------- 
        <element>
            <id>3</id>
            <token>vassc49e</token>
            <type>1</type>
            <name>admin</name>
            <displayName>admin</displayName>
            <objectType></objectType>
            <objectId></objectId>
            <participantType>1</participantType>
            <participantInCall></participantInCall>
            <participantFlags>0</participantFlags>
            <readOnly>0</readOnly>
            <count>0</count>
            <hasPassword></hasPassword>
            <hasCall></hasCall>
            <lastActivity>1566465303</lastActivity>
            <unreadMessages>0</unreadMessages>
            <unreadMention></unreadMention>
            <isFavorite></isFavorite>
            <notificationLevel>1</notificationLevel>
            <lastPing>1566465520</lastPing>
            <sessionId>0</sessionId>
            <participants>
                <pieroprocida>
                    <name>Piero Procida</name>
                    <type>1</type>
                    <call>0</call>
                    <sessionId>0</sessionId>
                </pieroprocida>
                <admin>
                    <name>admin</name>
                    <type>1</type>
                    <call>0</call>
                    <sessionId>0</sessionId>
                </admin>
            </participants>
            <numGuests>0</numGuests>
            <guestList></guestList>
            <lastMessage>
                <id>61</id>
                <actorType>users</actorType>
                <actorId>pieroprocida</actorId>
                <actorDisplayName>Piero Procida</actorDisplayName>
                <timestamp>1566465303</timestamp>
                <message>ciao</message>
                <messageParameters/>
                <systemMessage></systemMessage>
            </lastMessage>
        </element>
            ------------------------------------------*/
    }
}
