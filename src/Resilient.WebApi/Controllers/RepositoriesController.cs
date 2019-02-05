using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Resilient.WebApi.Client;
using Resilient.WebApi.Models;

namespace Resilient.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoriesController : ControllerBase
    {
        private readonly GithubClient _githubClient;

        public RepositoriesController(GithubClient githubClient)
        {
            _githubClient = githubClient;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var repositories = await _githubClient.GetRepositories();
            return Ok(repositories);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
