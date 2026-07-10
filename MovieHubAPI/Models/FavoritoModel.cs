public class FavoritoModel
{
    public long UsuarioId { get; set; }
    public UsuarioModel Usuario { get; set; } = null!;

    public int PeliculaId { get; set; }
    public PeliculaModel Pelicula { get; set; } = null!;
}