﻿using Microsoft.Json.Pointer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NextCloudAPI.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NextCloudAPI.Requests
{
    public class RequestsBL
    {
        #region PROPERTIES

        private User User;
        private string AuthorizationHeader;
        private HttpClient HttpClient = new HttpClient();

        #endregion

        #region CONSTRUCTOR
        public RequestsBL(User user)
        {
            User = user;

            AuthorizationHeader = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", User.userId, User.password)));
        }
        #endregion

        #region UNROLLERS
        internal List<T> DeserializeObjects<T>(XDocument xResponse, JsonPointer chosenPointer)
        {
            List<T> objects = new List<T>();

            string jResponse = JsonConvert.SerializeXNode(xResponse);               //transforms xml to json string
            JToken documentToken = JToken.Parse(jResponse);                         //loads it as jtoken (jobject)
            JToken elementToken = chosenPointer.Evaluate(documentToken);            //selects only the element node(s)

            if (elementToken != null)
            {
                if (elementToken is JArray)
                    objects = JsonConvert.DeserializeObject<List<T>>(elementToken.ToString());      //if multiple nodes (array)
                else if (elementToken is JObject)
                    objects.Add(JsonConvert.DeserializeObject<T>(elementToken.ToString()));         //if single nodes
            }

            return objects;
        }

        internal JObject DeserializeResponse(XDocument xResponse, JsonPointer chosenPointer)
        {
            string jResponse = JsonConvert.SerializeXNode(xResponse);
            JToken documentToken = JToken.Parse(jResponse);
            JToken genericToken = chosenPointer.Evaluate(jResponse);

            return (JObject)JsonConvert.DeserializeObject(genericToken.ToString());
        }
        #endregion

        #region BASE REQUESTS
          
        public async Task<HttpResponseMessage> GETRequest(string queryEndpoint, string chosenRequestStub)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, new Uri(chosenRequestStub + queryEndpoint));
            req = AddHeaders(req);

            var response = await HttpClient.SendAsync(req);

            return await Respond(response);
        }

        public async Task<HttpResponseMessage> POSTRequest(string queryEndpoint, string jContent, string chosenRequestStub)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, new Uri(chosenRequestStub + queryEndpoint));
            req = AddHeaders(req);

            req.Content = new StringContent(jContent, Encoding.UTF8, "application/json");

            var response = await HttpClient.SendAsync(req);

            return await Respond(response);
        }
        public async Task<HttpResponseMessage> DELETERequest(string queryEndpoint, string chosenRequestStub)
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, new Uri(chosenRequestStub + queryEndpoint));
            req = AddHeaders(req);

            var response = await HttpClient.SendAsync(req);

            return await Respond(response);
        }
        #endregion

        #region Common Methods for all requests
        HttpRequestMessage AddHeaders(HttpRequestMessage req)
        {
            req.Version = HttpVersion.Version11;
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            req.Headers.Authorization = new AuthenticationHeaderValue("Basic", AuthorizationHeader);
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
    }
}
