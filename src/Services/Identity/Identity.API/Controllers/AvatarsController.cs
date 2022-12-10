using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Identity.API.Contexts;
using Identity.API.Models;
using Identity.API.DTOs;
using AutoMapper;
using APICommonLibrary;

namespace Identity.API.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class AvatarsController : ControllerBase
    {
        private readonly DataContext _context;

        public AvatarsController(DataContext context)
        {
            _context = context;
        }


        // POST: api/Users/5/Avatar
        [HttpPost("{id}/Avatar")]
        public async Task<ActionResult<User>> PostAvatar(int id, IFormFile formFile)
        {
            var user = await _context.Users
                .Include(x => x.Avatar)
                .FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null) return NotFound();

            var (MIME, statusCode) = formFile.GetImageMIME();
            if (statusCode != null) return new StatusCodeResult((int)statusCode);

            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            user.Avatar = new Avatar(MIME, memoryStream.ToArray());
            user.LastModified = DateTime.Now;

            await _context.SaveChangesAsync();

            return Created("", null);
        }
    }
}
