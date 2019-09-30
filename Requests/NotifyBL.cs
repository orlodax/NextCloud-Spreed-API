using System;
using System.Collections.Generic;
using System.Text;

namespace NextCloudAPI.Requests
{
    public class NotifyBL
    {
        private RequestsBL RequestsBL;

        public NotifyBL(RequestsBL requestsBL)
        {
            RequestsBL = requestsBL;
        }

        //#region NOTIFIES
        //public async Task<List<Notify>> GetNotifies()
        //{
        //    List<Notify> notifies = new List<Notify>();

        //    HttpResponseMessage response = await GETRequest("/notifications", NotificationsRequestStub);

        //    if (response != null)
        //        notifies = DeserializeObjects<Notify>(XDocument.Parse(await response.Content.ReadAsStringAsync()), elementPointer);

        //    return notifies;
        //}
        //#endregion
    }
}
