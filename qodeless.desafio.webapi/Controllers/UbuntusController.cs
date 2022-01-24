using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qodeless.desafio.Infra.CrossCutting.identity.Data;
using qodeless.desafio.domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace qodeless.desafio.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UbuntusController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UbuntusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Ubuntus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubuntu>>> GetUbuntus()
        {
            return await _context.Ubuntus.ToListAsync();
        }

        // GET: api/Ubuntus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ubuntu>> GetUbuntu(Guid id)
        {
            var ubuntu = await _context.Ubuntus.FindAsync(id);

            if (ubuntu == null)
            {
                return NotFound();
            }

            return ubuntu;
        }

        // PUT: api/Ubuntus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUbuntu(Guid id, Ubuntu ubuntu)
        {
            if (id != ubuntu.Id)
            {
                return BadRequest();
            }

            _context.Entry(ubuntu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UbuntuExists(id))
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

        // POST: api/Ubuntus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ubuntu>> PostUbuntu(Ubuntu ubuntu)
        {
            _context.Ubuntus.Add(ubuntu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUbuntu", new { id = ubuntu.Id }, ubuntu);
        }

        // DELETE: api/Ubuntus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUbuntu(Guid id)
        {
            var ubuntu = await _context.Ubuntus.FindAsync(id);
            if (ubuntu == null)
            {
                return NotFound();
            }

            _context.Ubuntus.Remove(ubuntu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UbuntuExists(Guid id)
        {
            return _context.Ubuntus.Any(e => e.Id == id);
        }
    }
}
