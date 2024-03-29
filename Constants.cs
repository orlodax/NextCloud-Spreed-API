﻿namespace NextCloudAPI
{
    public class Constants
    {
        internal string BaseUrl = "WWW.InputYourNextCloud.URL";
        internal string BaseEndPoint = "/ocs/v2.php/apps/spreed/api/v1";
        internal string NotificationsEndPoint = "/ocs/v2.php/apps/notifications/api/v2";
        internal string ContactsEndPoint = "/ocs/v2.php/core/autocomplete";
        internal string AvatarEndPoint = "/index.php/avatar";
        internal string UsersEndPoint = "/ocs/v2.php/cloud/users";

        internal string BaseRequestStub { get; }
        internal string ContactsRequestStub { get; }
        internal string AvatarRequestStub { get; }
        internal string UsersRequestStub { get; }

        public Constants(string url)
        {
            BaseUrl = url;

            BaseRequestStub = "https://" + BaseUrl + BaseEndPoint;
            ContactsRequestStub = "https://" + BaseUrl + ContactsEndPoint;
            AvatarRequestStub = "https://" + BaseUrl + AvatarEndPoint;
            UsersRequestStub = "https://" + BaseUrl + UsersEndPoint;
        }

        public enum ConversationTypes
        {
            OneToOne = 1,
            Group = 2,
            Public = 3,
            Changelog = 4
        };

        public enum ParticipantTypes
        {
            Owner = 1,
            Moderator = 2,
            User = 3,
            Guest = 4,
            UserFollowingPublicLink = 5,
            GuestWithModeratorPermissions = 6
        };

        public static string[] ActorTypes = new string[]
        {
            "guests",
            "users"
        };

        public static string[] MessageTypes = new string[]
        {
            "command",
            "comment",
            "system"
        };

        /*
            System messages

            conversation_created - {actor} created the conversation
            conversation_renamed - {actor} renamed the conversation from "foo" to "bar"
            call_started - {actor} started a call
            call_joined - {actor} joined the call
            call_left - {actor} left the call
            call_ended - Call with {user1}, {user2}, {user3}, {user4} and {user5} (Duration 30:23)
            guests_allowed - {actor} allowed guests in the conversation
            guests_disallowed - {actor} disallowed guests in the conversation
            password_set - {actor} set a password for the conversation
            password_removed - {actor} removed the password for the conversation
            user_added - {actor} added {user} to the conversation
            user_removed - {actor} removed {user} from the conversation
            moderator_promoted - {actor} promoted {user} to moderator
            moderator_demoted - {actor} demoted {user} from moderator
            guest_moderator_promoted - {actor} promoted {user} to moderator
            guest_moderator_demoted - {actor} demoted {user} from moderator
            read_only_off - {actor} unlocked the conversation
            read_only - {actor} locked the conversation

         */
    }
}
