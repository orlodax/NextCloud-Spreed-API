using Microsoft.Json.Pointer;
using Newtonsoft.Json.Linq;
using NextCloudAPI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NextCloudAPI.Requests
{
    public class UserBL
    {
        private RequestsBL RequestsBL;

        private JsonPointer dataPointer = new JsonPointer("/ocs/data");

        public UserBL(RequestsBL requestsBL)
        {
            RequestsBL = requestsBL;
        }
        public async Task<UserInfo> GetUserInfo(string userId)
        {
            HttpResponseMessage response = await RequestsBL.GETRequest(string.Format("/{0}", userId), Constants.UsersRequestStub);

           UserInfo ui = new UserInfo();

            if (response != null)
                if (response.IsSuccessStatusCode)
                    ui = RequestsBL.DeserializeObjects<UserInfo>(XDocument.Parse(await response.Content.ReadAsStringAsync()), dataPointer)[0];
     
            return ui;
        }
    }
}
