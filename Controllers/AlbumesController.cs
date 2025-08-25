using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMusica.Data;
using ApiMusica.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMusica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumesController : ControllerBase
    {
        private readonly MusicaContext _context;

        public AlbumesController(MusicaContext context)
        {
            _context = context;
        }

        // GET: api/Albumes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbumes()
        {
            return await _context.Albumes.Include(a => a.Artista).ToListAsync();
        }

        // GET: api/Albumes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Album>> GetAlbum(int id)
        {
            var album = await _context.Albumes.Include(a => a.Artista)
                                              .FirstOrDefaultAsync(a => a.Id == id);

            if (album == null)
            {
                return NotFound();
            }

            return album;
        }

        // POST: api/Albumes
        [HttpPost]
        public async Task<ActionResult<Album>> PostAlbum(Album album)
        {
            _context.Albumes.Add(album);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlbum), new { id = album.Id }, album);
        }

        // PUT: api/Albumes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlbum(int id, Album album)
        {
            if (id != album.Id)
            {
                return BadRequest();
            }

            _context.Entry(album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
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

        // DELETE: api/Albumes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await _context.Albumes.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            _context.Albumes.Remove(album);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlbumExists(int id)
        {
            return _context.Albumes.Any(e => e.Id == id);
        }
    }
}
