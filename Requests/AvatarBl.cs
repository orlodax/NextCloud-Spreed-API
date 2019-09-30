using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NextCloudAPI.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace NextCloudAPI.Requests
{
    public class AvatarBL
    {
        private RequestsBL RequestsBL;

        public AvatarBL(RequestsBL requestsBL)
        {
            RequestsBL = requestsBL;
        }
        public async Task<Bitmap> GetAvatar(string username, int pixelDesired)
        {
            Bitmap avatar = null;

            HttpResponseMessage response = await RequestsBL.GETRequest(string.Format("/{0}/{1}", username, pixelDesired), Constants.AvatarRequestStub);

            if (response != null)
                avatar = new Bitmap(await response.Content.ReadAsStreamAsync());

            return avatar;
        }
    }
}
