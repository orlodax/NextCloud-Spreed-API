namespace NextCloudAPI
{
    public static class Constants
    {
        public const string BaseUrl = "www.yourspreedserver.url";
        public const string EndPoint = "/ocs/v2.php/apps/spreed/api/v1";

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
    }
}
