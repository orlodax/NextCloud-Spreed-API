using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NextCloudAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NextCloudAPI.Requests
{
    public class ContactBL
    {
        private RequestsBL RequestsBL;

        public ContactBL(RequestsBL requestsBL)
        {
            RequestsBL = requestsBL;
        }
        public async Task<List<Contact>> SearchContacts(string searchText)
        {
            List<Contact> contacts = new List<Contact>();

            HttpResponseMessage response = await RequestsBL.GETRequest
                (String.Format("/get?format=json&search={0}&itemType=call&itemId=new&shareTypes%5B%5D=0&shareTypes%5B%5D=1", searchText), RequestsBL.Constants.ContactsRequestStub);

            if (response != null)
            {
                JObject jResponse = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());     //sto coso era già un json e non un xml
                var contactsContainer = jResponse["ocs"]["data"].AsJEnumerable();                                           //non si sa perché il pointer non funziona, seleziono path a mano

                if (contactsContainer != null)                                                                              //gli oggetti che trova finalmente li serializza
                    if (contactsContainer is JArray)
                        contacts = JsonConvert.DeserializeObject<List<Contact>>(contactsContainer.ToString());      //if multiple nodes (array)
                    else if (contactsContainer is JObject)
                        contacts.Add(JsonConvert.DeserializeObject<Contact>(contactsContainer.ToString()));         //if single node
            }
            return contacts;
        }
    }
}
