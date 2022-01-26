using Microsoft.AspNetCore.Mvc;
using MyNamespace;
using Reddit.Model;

namespace Reddit.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private RedditContext _context;

        public CommentController(RedditContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Comment>> GetByID([FromRoute] string id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment is null)
                return NotFound();
            return Ok(comment);
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Comment>>> GetAll()
        {
            var comments = await _context.Comments.ToListAsync();

            if (comments is null)
                return NotFound();
            return Ok(comments);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Comment>> Post([FromBody] Comment model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Comments.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Comment>> Put([FromRoute] string id, [FromBody] UpdateCommentDto update)
        {
            var comment = await _context.Comments.FindAsync(id);

            comment.UserId = update.UserId;
            comment.Type = update.Type;

            _context.Update(comment);
            _context.SaveChanges();

            return Ok(comment);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Comment>> Delete([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var comment = await _context.Comments.FindAsync(id);

            if (comment.IsDeleted == true)
                return BadRequest();

            comment.IsDeleted = true;

            _context.Update(comment);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }

}