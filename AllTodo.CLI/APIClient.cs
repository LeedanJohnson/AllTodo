using AllTodo.Shared.Models.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AllTodo.CLI
{
    class APIClient
    {
        private HttpClient client;
        public APIClient(string server_url)
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(server_url);

            // Add an Accept header for JSON format.
            this.client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        ~APIClient()
        {
            this.client.Dispose();
        }

        public (HttpStatusCode status, string jsonstring) PostAsync(string path, object data)
        {
            HttpResponseMessage response = client.PostAsJsonAsync(path, data).Result;

            (HttpStatusCode status, string jsonstring) result;
            result.status = response.StatusCode;
            result.jsonstring = response.Content.ReadAsStringAsync().Result;

            return result;
        }

    }
}
