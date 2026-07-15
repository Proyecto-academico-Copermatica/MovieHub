namespace MovieHubAPI.Models
{
    public class PeliculaGeneroModel
    {
        public int PeliculaId { get; set; }
        public PeliculaModel Pelicula { get; set; } = null!;

        public int GeneroId { get; set; }
        public GeneroModel Genero { get; set; } = null!;
    }
}