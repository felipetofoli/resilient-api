using Newtonsoft.Json;
using Resilient.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Resilient.WebApi.Client
{
    public class JsonPlaceHolderClient
    {
        private HttpClient _client;

        public JsonPlaceHolderClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<PostModel>> GetRepositories()
        {
            var response = await _client.GetAsync("http://jsonplaceholder.typicode.com/posts");
            return await response.Content.ReadAsAsync<IEnumerable<PostModel>>();
        }

        public async Task<PostModel> GetPost(int id)
        {
            var response = await _client.GetAsync("$http://jsonplaceholder.typicode.com/posts/{id}");
            return await response.Content.ReadAsAsync<PostModel>();
        }

        public async Task BlogPost(PostModel value)
        {
            var content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

            await _client.PostAsync("http://jsonplaceholder.typicode.com/posts", content);
        }
    }
}
