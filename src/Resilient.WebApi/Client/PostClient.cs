using Newtonsoft.Json;
using Resilient.WebApi.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Resilient.WebApi.Client
{
    public class PostClient
    {
        private readonly HttpClient _client;

        public PostClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<PostModel>> GetPosts()
        {
            var response = await _client.GetAsync("/posts");
            return await response.Content.ReadAsAsync<IEnumerable<PostModel>>();
        }

        public async Task<PostModel> GetPost(int id)
        {
            var response = await _client.GetAsync($"/posts/{id}");
            return await response.Content.ReadAsAsync<PostModel>();
        }

        public async Task BlogPost(PostModel value)
        {
            var content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            await _client.PostAsync("/posts", content);
        }
    }
}