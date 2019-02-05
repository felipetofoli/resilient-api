using Microsoft.AspNetCore.Mvc;
using Resilient.WebApi.Client;
using Resilient.WebApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Resilient.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostClient postClient;

        public PostsController(PostClient jsonPlaceHolderClient)
        {
            postClient = jsonPlaceHolderClient;
        }

        /// <summary>
        /// Gets all blog posts.
        /// </summary>
        /// <returns>All blog posts.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PostModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var repositories = await postClient.GetPosts();
            return Ok(repositories);
        }

        /// <summary>
        /// Gets the specified blog post.
        /// </summary>
        /// <param name="id">The id of the blog post desired.</param>
        /// <returns>A specified blog post.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PostModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int id)
        {
            var post = await postClient.GetPost(id);
            return Ok(post);
        }

        /// <summary>
        /// Creates a new blog post.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /posts
        ///     {
        ///        "userId": 1,
        ///        "title": "title name",
        ///        "body": "body text"
        ///     }
        ///
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>Status of success.</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] PostModel value)
        {
            await postClient.BlogPost(value);
            return Ok();
        }
    }
}