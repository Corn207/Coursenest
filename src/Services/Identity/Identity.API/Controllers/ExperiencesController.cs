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
    public class ExperiencesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ExperiencesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // POST: api/Users/5/Experiences
        [HttpPost("{userId}/Experiences")]
        public async Task<ActionResult<Experience>> PostExperience(int userId, ExperiencePostRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null) return NotFound();

            user.Experiences.Add(_mapper.Map<Experience>(request));
            user.LastModified = DateTime.Now;

            await _context.SaveChangesAsync();

            return Created("", null);
        }


        // DELETE: api/Users/5/Experiences/5
        [HttpDelete("{userId}/Experiences/{id}")]
        public async Task<IActionResult> DeleteExperience(int userId, int experienceId)
        {
            var experience = await _context.Experience
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ExperienceId == experienceId);
            if (experience == null) return NotFound();

            _context.Experience.Remove(experience);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
