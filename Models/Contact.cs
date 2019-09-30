using System.Drawing;

namespace NextCloudAPI.Models
{
    public class Contact
    {
        public string id { get; set; }
        public string label { get; set; }
        public string source { get; set; }

        #region UI
        public Bitmap Avatar { get; set; }
        #endregion 
    }
}
