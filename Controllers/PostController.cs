using Model;
using System.Collections.Generic;
using System.Linq;
using Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Reddit.Dtos;

namespace Controller
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private RedditContext _context;
        public PostController(RedditContext context)
        {
            _context = context;
        }

        //*GetByID GET: post/2
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Post>> GetByID([FromRoute] string id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post is null)
                return NotFound();
            return Ok(post);
        }

        //*GetAll GET: post
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Post>>> GetAll()
        {
            var posts = await _context.Posts.ToListAsync();

            if (posts is null)
                return NotFound();
            return Ok(posts);
        }

        //*Insert POST: post
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Post>> Post([FromBody] CreatePostDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = new Post(model.CommunityName, model.Author);
            _context.Posts.Add(post);

            await _context.SaveChangesAsync();
            return Ok(post);
        }

        //*UpdateById  PUT: post/5
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Post>> Put([FromRoute] string id, [FromBody] UpdatePostDto update)
        {
            var post = await _context.Posts.FindAsync(id);

            post.CommunityName = update.CommunityName;
            post.Author = update.Author;

            _context.Update(post);

            _context.SaveChanges();

            return Ok(post);
        }


        //* DeleteByID  DELETE: v1/author/3
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Post>> Delete([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var post = await _context.Posts.FindAsync(id);
            // _context.Posts.Remove(author);

            if (post.IsDeleted == true)
                return BadRequest();

            post.IsDeleted = true;

            _context.Update(post);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}