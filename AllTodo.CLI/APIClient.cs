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
        }

        public (HttpStatusCode status, string jsonstring) Post(string path, object obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            string url = $"{this.server_url}/{path}";
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            byte[] buffer = encoding.GetBytes(data);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(buffer, 0, buffer.Length);
            dataStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string result = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
            {
                result = reader.ReadToEnd();
            }
            return (response.StatusCode, result);
        }

        public (HttpStatusCode status, string jsonstring) Get(string path, TokenCredentialsDTO credentials)
        {
            string url = $"{this.server_url}/{path}?idtoken={credentials.IDToken}&authtoken={credentials.AuthToken}";
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string result = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
            {
                result = reader.ReadToEnd();
            }
            return (response.StatusCode, result);
        }

        public (HttpStatusCode status, string jsonstring) Patch(string path, object obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            string url = $"{this.server_url}/{path}";
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PATCH";
            request.ContentType = "application/json; charset=utf-8";
            byte[] buffer = encoding.GetBytes(data);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(buffer, 0, buffer.Length);
            dataStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string result = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
            {
                result = reader.ReadToEnd();
            }
            return (response.StatusCode, result);
        }
    }
}
