using System.Collections.Generic;

namespace ApiMusica.Models
{
    public class Artista
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Un artista puede tener muchos álbumes
        public List<Album> Albumes { get; set; }
    }
}
