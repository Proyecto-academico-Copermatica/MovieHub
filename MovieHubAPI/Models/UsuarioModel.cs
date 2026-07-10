using Microsoft.AspNetCore.Identity;

public class UsuarioModel : IdentityUser<long>
{
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Propiedades de navegación
    public ICollection<ValoracionModel> Valoraciones { get; set; } = new List<ValoracionModel>();
    public ICollection<FavoritoModel> Favoritos { get; set; } = new List<FavoritoModel>();
}