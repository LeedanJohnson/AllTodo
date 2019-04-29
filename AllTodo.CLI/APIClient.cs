using AllTodo.Shared.Models;
using AllTodo.Shared.Models.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AllTodo.CLI
{
    class APIClient
    {
        private string server_url;
        public APIClient(string server_url)
        {
            this.server_url = server_url;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public (bool success, string message) Login(string username, string password)
        {
            string url = $"{this.server_url}/api/login";
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["username"] = username;
            request.Headers["password"] = password;

            HttpWebResponse response;
            try { response = (HttpWebResponse)request.GetResponse(); }
            catch (WebException e) { response = (HttpWebResponse)e.Response; }

            string result = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
            {
                result = reader.ReadToEnd();
            }

            if (response.StatusCode != HttpStatusCode.OK)
                return (false, "Login Failed");

            TokenCredentialsDTO credentials = JsonConvert.DeserializeObject<TokenCredentialsDTO>(result);

            Environment.SetEnvironmentVariable("ALLTODO_IDTOKEN", credentials.IDToken, EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable("ALLTODO_AUTHTOKEN", credentials.UserAuthToken, EnvironmentVariableTarget.User);

            return (true, "Login Successful");
        }

        public (HttpStatusCode response_code, string jsonstring) Post(string path, object obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            string url = $"{this.server_url}/{path}";
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Headers["idtoken"] = Environment.GetEnvironmentVariable("ALLTODO_IDTOKEN", EnvironmentVariableTarget.User);
            request.Headers["authtoken"] = Environment.GetEnvironmentVariable("ALLTODO_AUTHTOKEN", EnvironmentVariableTarget.User);
            request.ContentType = "application/json; charset=utf-8";
            byte[] buffer = encoding.GetBytes(data);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(buffer, 0, buffer.Length);
            dataStream.Close();

            HttpWebResponse response;
            try { response = (HttpWebResponse)request.GetResponse(); }
            catch (WebException e) { response = (HttpWebResponse)e.Response; }

            string result = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
            {
                result = reader.ReadToEnd();
            }
            return (response.StatusCode, result);
        }

        public (HttpStatusCode response_code, string jsonstring) Get(string path)
        {
            string url = $"{this.server_url}/{path}";
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["idtoken"] = Environment.GetEnvironmentVariable("ALLTODO_IDTOKEN", EnvironmentVariableTarget.User);
            request.Headers["authtoken"] = Environment.GetEnvironmentVariable("ALLTODO_AUTHTOKEN", EnvironmentVariableTarget.User);

            HttpWebResponse response;
            try { response = (HttpWebResponse)request.GetResponse(); }
            catch (WebException e) { response = (HttpWebResponse)e.Response; }

            string result = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
            {
                result = reader.ReadToEnd();
            }
            return (response.StatusCode, result);
        }

        public (HttpStatusCode response_code, string jsonstring) Patch(string path, object obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            string url = $"{this.server_url}/{path}";
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PATCH";
            request.Headers["idtoken"] = Environment.GetEnvironmentVariable("ALLTODO_IDTOKEN", EnvironmentVariableTarget.User);
            request.Headers["authtoken"] = Environment.GetEnvironmentVariable("ALLTODO_AUTHTOKEN", EnvironmentVariableTarget.User);
            request.ContentType = "application/json; charset=utf-8";
            byte[] buffer = encoding.GetBytes(data);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(buffer, 0, buffer.Length);
            dataStream.Close();

            HttpWebResponse response;
            try { response = (HttpWebResponse)request.GetResponse(); }
            catch (WebException e) { response = (HttpWebResponse)e.Response; }

            string result = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
            {
                result = reader.ReadToEnd();
            }
            return (response.StatusCode, result);
        }
    }
}
