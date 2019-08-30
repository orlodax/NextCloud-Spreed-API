namespace NextCloudAPI.Models
{
    public class User                                           // SEE https://nextcloud-talk.readthedocs.io/en/latest/participant/
    {
        public string userId { get; set; } = string.Empty;      // Is empty for guests
        public string password { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty; // Can be empty for guests
        public string sessionID { get; set; } = string.Empty;   // '0' if not connected, otherwise a 512 character long string
        public byte participantType { get; set; }               // Permissions level of the current user - SEE Constants.cs
        public ulong lastPing { get; set; }                     // Timestamp of the last ping of the current user (should be used for sorting)
        public byte inCall { get; set; }                        // Is user in call

        /* RESPONSE SAMPLE------------------------------------ 
        <?xml version="1.0"?>
            <ocs>
                ...
                <data>
                    <element>
                        <inCall>0</inCall>
                        <lastPing>0</lastPing>
                        <sessionId>0</sessionId>
                        <participantType>3</participantType>
                        <userId>pieroprocida</userId>
                        <displayName>Piero Procida</displayName>
                    </element>
                </data>
            </ocs>

         ---------------------------------------------------*/
    }
}
