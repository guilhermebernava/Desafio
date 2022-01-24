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
using qodeless.desafio.services.Interfaces;
using qodeless.desafio.webapi.ViewModel;

namespace qodeless.desafio.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UbuntusController : ApiController
    {
        private readonly IUbuntuServices _ubuntuServices;

        public UbuntusController(ApplicationDbContext db, IUbuntuServices ubuntuServices) : base(db)
        {
            _ubuntuServices = ubuntuServices;
        }

        // GET: api/Ubuntus
        [HttpGet]
        public async Task<IActionResult> GetUbuntus()
        {
            var result = await Task.Run(() => _ubuntuServices.GetAll());
            return Ok(result);
        }

        // GET: api/Ubuntus/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUbuntu(Guid id)
        {
            var result = await Task.Run(() => _ubuntuServices.GetById(id));

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
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

            try
            {
                _ubuntuServices.Update(ubuntu);
                await Db.SaveChangesAsync();
                return Ok();
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
            _ubuntuServices.Add(ubuntu);
            await Db.SaveChangesAsync();

            return CreatedAtAction("GetUbuntu", new { id = ubuntu.Id }, ubuntu);
        }

        // DELETE: api/Ubuntus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUbuntu(Guid id)
        {
            var ubuntu = await Task.Run(() => _ubuntuServices.GetById(id));
            if (ubuntu == null)
            {
                return NotFound();
            }

            _ubuntuServices.Remove(id);
            await Db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("UbuntusTrojan")]
        public async Task<IActionResult> GetUbuntusTrojan()
        {
            var ubuntus = await Task.Run(() => _ubuntuServices.GetAll());

            var validacaoList = new List<UbuntuValidationResponse>();

            foreach (Ubuntu item in ubuntus)
            {
                 var hash = _ubuntuServices.sha256_hash(item.Name + item.Telephone + item.IndicatorArea.ToString() + item.Date.ToString("yyyy-MM-dd") + "ubuntu");
                 
                 validacaoList.Add(new UbuntuValidationResponse { Nome = item.Name, isValid = hash == item.Key });
                
            }

            return Ok(validacaoList);
        }

        private bool UbuntuExists(Guid id)
        {
            return Db.Ubuntus.Any(e => e.Id == id);
        }
    }
}
