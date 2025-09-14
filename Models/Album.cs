namespace ApiMusica.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = ""; // Inicializado para evitar warning
        public int Año { get; set; }

        // Relación con Artista
        public int ArtistaId { get; set; }
        public Artista? Artista { get; set; }  // Nullable, está bien así
    }
}
