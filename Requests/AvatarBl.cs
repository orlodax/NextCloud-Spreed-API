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
        public async Task<Bitmap> GetAvatar(string username, int pixelDesired)  //to load anyone custom sized avatar
        {
            Bitmap avatar = null;

            HttpResponseMessage response = await RequestsBL.GETRequest(string.Format("/{0}/{1}", username, pixelDesired), RequestsBL.Constants.AvatarRequestStub);

            if (response != null)
                if (response.IsSuccessStatusCode)
                    avatar = new Bitmap(await response.Content.ReadAsStreamAsync());

            return avatar;
        }
        public async Task<Bitmap> GetMyAvatar() //to load big avatar in details
        {
            Bitmap avatar = null;

            HttpResponseMessage response = await RequestsBL.GETRequest(string.Format("/{0}/{1}", RequestsBL.User.userId, 400), RequestsBL.Constants.AvatarRequestStub);

            if (response != null)
                if (response.IsSuccessStatusCode)
                    avatar = new Bitmap(await response.Content.ReadAsStreamAsync());

            return avatar;
        }
        public async Task<Bitmap> DeleteAvatar()
        {
            Bitmap avatar = null;

            HttpResponseMessage response = await RequestsBL.DELETERequest(string.Empty, RequestsBL.Constants.AvatarRequestStub);

            //after deleting, reloads the default image for this user
            if (response != null)
                if (response.IsSuccessStatusCode)
                    avatar = await GetAvatar(RequestsBL.User.userId, 400);

            return avatar;
        }

        //NON FUNZIONA, doppia richiesta POST/GET/POST nel php (?)

        //public async Task<Bitmap> UploadAvatar(MemoryStream stream)
        //{
        //    Bitmap avatar = null;
        //    var req = new HttpRequestMessage(HttpMethod.Post, new Uri(Constants.AvatarRequestStub + "/cropped"));

        //    //Content Formatting--------------------------------------------------------------------
        //    ContentDispositionHeaderValue cdstream = new ContentDispositionHeaderValue("form-data");
        //    cdstream.Name = "\"files[]\"";
        //    cdstream.FileName = "\"unnamed.jpg\"";
        //    ContentDispositionHeaderValue cdstring = new ContentDispositionHeaderValue("form-data");
        //    cdstring.Name = "\"requesttoken\"";

        //    HttpContent stringContent = new StringContent("paramstring");
        //    HttpContent streamContent = new StreamContent(stream);
        //    streamContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("image/jpeg");
        //    streamContent.Headers.ContentDisposition = cdstream;
        //    stringContent.Headers.ContentDisposition = cdstring;

        //    var formData = new MultipartFormDataContent();
        //    formData.Add(stringContent);
        //    formData.Add(streamContent);

            
   
        //    req.Content = formData;

        //    //-------------------------------------------------------------------------------------
        //    string boundary = "---------------------------" + DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        //    req.Content.Headers.Remove("Content-Type");
        //    req.Content.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);
        //    //req.Content.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);
               
        //    var response = await RequestsBL.HttpClient.SendAsync(req);

        //    if (response != null)
        //        if (response.IsSuccessStatusCode)
        //            avatar = await GetAvatar(RequestsBL.User.userId, 400);

        //    //MimePart mim = new MimePart();
        //    //MemoryStream[] files = new MemoryStream[] { stream };
        //    //NameValueCollection nameValueCollection = new NameValueCollection();




        //    //Upload(Constants.AvatarRequestStub, new NameValueCollection(), files);

        //    return avatar;
        //}

       //// public string Upload(string url, NameValueCollection requestParameters, MemoryStream file)
       // {

       //     var client = RequestsBL.HttpClient;
       //     var content = new MultipartFormDataContent();

       //     content.Add(new StreamContent(file));
       //     List<KeyValuePair<string, string>> b = new List<KeyValuePair<string, string>>();
       //     //b.Add(requestParameters);
       //     var addMe = new FormUrlEncodedContent(b);

       //     content.Add(addMe);
       //     var result = client.PostAsync(url, content);
       //     return result.Result.ToString();
       // }

        //public class MimePart
        //{
        //    NameValueCollection _headers = new NameValueCollection();
        //    byte[] _header;

        //    public NameValueCollection Headers
        //    {
        //        get { return _headers; }
        //    }

        //    public byte[] Header
        //    {
        //        get { return _header; }
        //    }

        //    public long GenerateHeaderFooterData(string boundary)
        //    {
        //        StringBuilder sb = new StringBuilder();

        //        sb.Append("--");
        //        sb.Append(boundary);
        //        sb.AppendLine();
        //        foreach (string key in _headers.AllKeys)
        //        {
        //            sb.Append(key);
        //            sb.Append(": ");
        //            sb.AppendLine(_headers[key]);
        //        }
        //        sb.AppendLine();

        //        _header = Encoding.UTF8.GetBytes(sb.ToString());

        //        return _header.Length + Data.Length + 2;
        //    }

        //    public Stream Data { get; set; }
        //}

        //public string Upload(string url, NameValueCollection requestParameters, params MemoryStream[] files)
        //{
        //    using (WebClient req = new WebClient())
        //    {
        //        List<MimePart> mimeParts = new List<MimePart>();

        //        try
        //        {
        //            foreach (string key in requestParameters.AllKeys)
        //            {
        //                MimePart part = new MimePart();

        //                part.Headers["Content-Disposition"] = "form-data; name=\"" + key + "\"";
        //                part.Data = new MemoryStream(Encoding.UTF8.GetBytes(requestParameters[key]));

        //                mimeParts.Add(part);
        //            }

        //            int nameIndex = 0;

        //            foreach (MemoryStream file in files)
        //            {
        //                MimePart part = new MimePart();
        //                string fieldName = "file" + nameIndex++;

        //                part.Headers["Content-Disposition"] = "form-data; name=\"" + fieldName + "\"; filename=\"" + fieldName + "\"";
        //                part.Headers["Content-Type"] = "application/octet-stream";

        //                part.Data = file;

        //                mimeParts.Add(part);
        //            }

        //            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
        //            req.Headers.Add(HttpRequestHeader.ContentType, "multipart/form-data; boundary=" + boundary);

        //            long contentLength = 0;

        //            byte[] _footer = Encoding.UTF8.GetBytes("--" + boundary + "--\r\n");

        //            foreach (MimePart part in mimeParts)
        //            {
        //                contentLength += part.GenerateHeaderFooterData(boundary);
        //            }

        //            //req.ContentLength = contentLength + _footer.Length;

        //            byte[] buffer = new byte[8192];
        //            byte[] afterFile = Encoding.UTF8.GetBytes("\r\n");
        //            int read;

        //            using (MemoryStream s = new MemoryStream())
        //            {
        //                foreach (MimePart part in mimeParts)
        //                {
        //                    s.Write(part.Header, 0, part.Header.Length);

        //                    while ((read = part.Data.Read(buffer, 0, buffer.Length)) > 0)
        //                        s.Write(buffer, 0, read);

        //                    part.Data.Dispose();

        //                    s.Write(afterFile, 0, afterFile.Length);
        //                }

        //                s.Write(_footer, 0, _footer.Length);
        //                byte[] responseBytes = req.UploadData(url, s.ToArray());
        //                string responseString = Encoding.UTF8.GetString(responseBytes);
        //                return responseString;
        //            }
        //        }
        //        catch
        //        {
        //            foreach (MimePart part in mimeParts)
        //                if (part.Data != null)
        //                    part.Data.Dispose();

        //            throw;
        //        }
        //    }
        //}

    }
}
