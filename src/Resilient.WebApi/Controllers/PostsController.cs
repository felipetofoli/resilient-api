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
    public class PostsController : ControllerBase
    {
        private readonly JsonPlaceHolderClient jsonPlaceHolderClient;

        public PostsController(JsonPlaceHolderClient jsonPlaceHolderClient)
        {
            this.jsonPlaceHolderClient = jsonPlaceHolderClient;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var repositories = await jsonPlaceHolderClient.GetRepositories();
            return Ok(repositories);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post = await jsonPlaceHolderClient.GetPost(id);
            return Ok(post);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostModel value)
        {
            await jsonPlaceHolderClient.BlogPost(value);

            return Ok();
        }
    }
}
