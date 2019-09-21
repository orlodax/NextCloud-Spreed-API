using System;

namespace NextCloudAPI.Models
{
    public class Notify
    {
        public int? notification_id { get; set; }
        public string app { get; set; } = string.Empty;
        public string user { get; set; } = string.Empty;
        public DateTime? datetime { get; set; }
        public string object_type { get; set; } = string.Empty;
        public string object_id { get; set; } = string.Empty;
        public string subject { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public string link { get; set; } = string.Empty;
        public string subjectRich { get; set; } = string.Empty;
        public SubjectRichParameters subjectRichParameters { get; set; } = new SubjectRichParameters();
        public object messageRich { get; set; } = new object();
        public object[] messageRichParameters { get; set; } = new object[] { };
        public string icon { get; set; } = string.Empty;
        public Action[] actions { get; set; } = new Action[] { };

        public class SubjectRichParameters
        {
            public class User
            {
                public string type { get; set; } = string.Empty;
                public string id { get; set; } = string.Empty;
                public string name { get; set; } = string.Empty;
            }
            public class Call
            {
                public string type { get; set; } = string.Empty;
                public string id { get; set; } = string.Empty;
                public string name { get; set; } = string.Empty;
            }
            public User user;
            public Call call;
        }
        public class Action
        {
            public string label { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
            public string type { get; set; } = string.Empty;
            public bool primary { get; set; }
        }

    }
}
