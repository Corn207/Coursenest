using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Identity.API.Contexts;
using Identity.API.Models;
using Azure.Core;
using Identity.API.DTOs;
using AutoMapper;

namespace Identity.API.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TopicsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // POST: api/Users/5/Topics/Interested
        [HttpPost("{userId}/Topics/Interested")]
        public async Task<ActionResult<Experience>> PostTopicInterested(int userId, int topicId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null) return NotFound();

            // TODO Add interested topic -> Check other microservice has topic
            user.InterestedTopics.Add(new InterestedTopic() { TopicId = topicId });
            user.LastModified = DateTime.Now;

            await _context.SaveChangesAsync();

            return Created("", null);
        }


        // DELETE: api/Users/5/Topics/Interested/5
        [HttpDelete("{userId}/Topics/Interested/{id}")]
        public async Task<IActionResult> DeleteTopicInterested(int userId, int topicId)
        {
            var topic = await _context.InterestedTopics
                .FirstOrDefaultAsync(x => x.UserId == userId && x.TopicId == topicId);
            if (topic == null) return NotFound();

            _context.InterestedTopics.Remove(topic);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/Users/5/Topics/Followed
        [HttpPost("{userId}/Topics/Followed")]
        public async Task<ActionResult<Experience>> PostTopicFollowed(int userId, int topicId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null) return NotFound();

            // TODO Add followed topic -> Check other microservice has topic
            user.FollowedTopics.Add(new FollowedTopic() { TopicId = topicId });
            user.LastModified = DateTime.Now;

            await _context.SaveChangesAsync();

            return Created("", null);
        }


        // DELETE: api/Users/5/Topics/Followed/5
        [HttpDelete("{userId}/Topics/Followed/{id}")]
        public async Task<IActionResult> DeleteTopicFollowed(int userId, int topicId)
        {
            var topic = await _context.FollowedTopics
                .FirstOrDefaultAsync(x => x.UserId == userId && x.TopicId == topicId);
            if (topic == null) return NotFound();

            _context.FollowedTopics.Remove(topic);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
