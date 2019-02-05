using Resilient.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resilient.WebApi.Client
{
    public class GithubClient
    {
        private HttpClient _client;

        public GithubClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<RepositoryModel>> GetRepositories()
        {
            var response = await _client.GetAsync("/users/felipetofoli/repos");
            return await response.Content.ReadAsAsync<IEnumerable<RepositoryModel>>();
        }
    }
}
