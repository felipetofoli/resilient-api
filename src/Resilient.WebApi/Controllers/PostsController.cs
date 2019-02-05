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
        private readonly PostClient postClient;

        public PostsController(PostClient jsonPlaceHolderClient)
        {
            this.postClient = jsonPlaceHolderClient;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var repositories = await postClient.GetPosts();
            return Ok(repositories);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post = await postClient.GetPost(id);
            return Ok(post);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostModel value)
        {
            await postClient.BlogPost(value);
            return Ok();
        }
    }
}
