using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MovieHubAPI.Models;

namespace MovieHubAPI.Models
{
    public class PeliculaModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [MaxLength(500)]
        public string? Director { get; set; }

        public int Anio { get; set; }

        public int Duracion { get; set; } // minutos

        [MaxLength(500)]
        public string? PosterUrl { get; set; }

        [Column(TypeName = "decimal(3,2)")]
        public double PuntuacionMedia { get; set; } = 0;

        // Propiedades de navegación
        public ICollection<PeliculaGeneroModel> PeliculaGeneros { get; set; } = new List<PeliculaGeneroModel>();
        public ICollection<ValoracionModel> Valoraciones { get; set; } = new List<ValoracionModel>();
        public ICollection<FavoritoModel> Favoritos { get; set; } = new List<FavoritoModel>();
    }
}