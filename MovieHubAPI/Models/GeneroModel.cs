using System.ComponentModel.DataAnnotations;

namespace MovieHubAPI.Models
{
    public class GeneroModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        // Propiedad de navegación
        public ICollection<PeliculaGeneroModel> PeliculaGeneros { get; set; } = new List<PeliculaGeneroModel>();
    }
}