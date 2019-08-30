using Microsoft.Json.Pointer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NextCloudAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NextCloudAPI.Requests
{
    public class BaseBL
    {
        #region PROPERTIES

        private User User;
        private string RequestStub;
        private HttpClient HttpClient = new HttpClient();

        internal JsonPointer standardPointer = new JsonPointer("/ocs/data/element");
        internal JsonPointer responsePointer = new JsonPointer("/ocs");

        #endregion

        //CONSTRUCTOR: INJECTING USER THUS BUILDING BASE STRING FOR REQUESTS
        public BaseBL(User user)
        {
            User = user;

            RequestStub = String.Format("http://{0}:{1}@{2}{3}", User.userId, User.password, Constants.BaseUrl, Constants.EndPoint);
        }

        #region UNROLLERS
        internal List<T> DeserializeObjects<T>(XDocument xResponse, JsonPointer objPointer)
        {
            List<T> objects = new List<T>();

            string jResponse = JsonConvert.SerializeXNode(xResponse);
            JToken documentToken = JToken.Parse(jResponse);
            JToken genericToken = objPointer.Evaluate(documentToken);
            var jObjects = JsonConvert.DeserializeObject<List<T>>(genericToken.ToString());

            if (jObjects.Count > 0)
                objects = jObjects;

            return objects;
        }

        internal JObject DeserializeResponse(XDocument xResponse)
        {
            string jResponse = JsonConvert.SerializeXNode(xResponse);
            JToken documentToken = JToken.Parse(jResponse);
            JToken genericToken = responsePointer.Evaluate(documentToken);

            return (JObject)JsonConvert.DeserializeObject(genericToken.ToString());
        }
        #endregion

        #region BASE REQUESTS
            #region Common Methods for all requests
            HttpRequestMessage AddHeaders(HttpRequestMessage req)
            {
                req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", "cGllcm9wcm9jaWRhOlByb1BpZTIwMTk=");
                req.Headers.Connection.Add("keep-alive");
                req.Headers.Add("Accept-Encoding", "gzip,deflate");
                req.Headers.TryAddWithoutValidation("OCS-APIRequest", "true");
                return req;
            }
            async Task<string> RequestError(HttpResponseMessage response)
            {
                string str = string.Empty;
                string error = string.Empty;
                if (response.Content != null)
                {
                    str = await response.Content.ReadAsStringAsync();
                    error = string.Format("ERROR: " + response.StatusCode + " " + response.ReasonPhrase + "\r\n" + str);
                }
                return error;
            }
            async Task<HttpResponseMessage> Respond(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                    return response;
                else
                {
                    await RequestError(response);
                    return null;
                }
            }
            #endregion
        public async Task<HttpResponseMessage> GETRequest(string queryEndpoint)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, new Uri(RequestStub + queryEndpoint));
            req = AddHeaders(req);

            var response = await HttpClient.SendAsync(req);

            return await Respond(response);
        }
        public async Task<HttpResponseMessage> POSTRequest(string queryEndpoint, string jContent)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, new Uri(RequestStub + queryEndpoint));
            req = AddHeaders(req);

            req.Content = new StringContent(jContent, Encoding.UTF8, "application/json");

            var response = await HttpClient.SendAsync(req);

            return await Respond(response);
        }
        public async Task<HttpResponseMessage> DELETERequest(string queryEndpoint)
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, new Uri(RequestStub + queryEndpoint));
            req = AddHeaders(req);

            var response = await HttpClient.SendAsync(req);

            return await Respond(response);
        }
        #endregion
    }
}
