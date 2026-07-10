using System.ComponentModel.DataAnnotations;

public class ValoracionModel
{
    [Key]
    public int Id { get; set; }

    public long UsuarioId { get; set; }
    public UsuarioModel Usuario { get; set; } = null!;

    public int PeliculaId { get; set; }
    public PeliculaModel Pelicula { get; set; } = null!;

    [Range(1, 5)]
    public int Puntuacion { get; set; }

    public DateTime Fecha { get; set; } = DateTime.UtcNow;
}