using Microsoft.EntityFrameworkCore;
using ApiMusica.Models;

namespace ApiMusica.Data
{
    public class MusicaContext : DbContext
    {
        public MusicaContext(DbContextOptions<MusicaContext> options) : base(options)
        {
        }

        public DbSet<Artista> Artistas { get; set; }
        public DbSet<Album> Albumes { get; set; }
    }
}
