using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAT250_REST.Data;
using DAT250_REST.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DAT250_REST.Messaging;

namespace DAT250_REST.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly RabbitMqClient<Vote> _rabbitMqClient;

        public VotesController(AppDBContext context)
        {
            _context = context;
            _rabbitMqClient = new RabbitMqClient<Vote>();
        }

        // GET: api/Votes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vote>>> GetVotes()
        {
            return await _context.Votes.ToListAsync();
        }

        // GET: api/Votes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vote>> GetVote(int id)
        {
            var vote = await _context.Votes.FindAsync(id);

            if (vote == null)
            {
                return NotFound();
            }

            return vote;
        }

        // PUT: api/Votes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVote(int id, Vote vote)
        {
            if (id != vote.Id)
            {
                return BadRequest();
            }

            _context.Entry(vote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Votes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vote>> PostVote(VoteDto voteDto)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var creator = await _context.Users.FindAsync(userId);

            Vote vote = new()
            {
                Option = await _context.VoteOptions.FindAsync(voteDto.OptionId) ?? throw new InvalidOperationException("No voteoptions found for: " + voteDto.OptionId),
                Poll = await _context.Polls.FindAsync(voteDto.PollId) ?? throw new InvalidOperationException("No poll found with pollid: " + voteDto.PollId),
                PublishedAt = voteDto.PublishedAt,
                User = creator
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();
            await _rabbitMqClient.PublishMessageAsync(vote, "pollapp-analytics-queue");
            return CreatedAtAction("GetVote", new { id = vote.Id }, vote);
        }

        // DELETE: api/Votes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(int id)
        {
            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VoteExists(int id)
        {
            return _context.Votes.Any(e => e.Id == id);
        }

        public class VoteDto
        {
            public required String PollId { get; set; }
            public required int OptionId { get; set; }
            public DateTime PublishedAt { get; set; }
        }
    }
}
