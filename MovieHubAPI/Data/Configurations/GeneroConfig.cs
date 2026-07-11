using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieHubAPI.Data.Configurations
{
    public class GeneroConfig : IEntityTypeConfiguration<GeneroModel>
    {
        public void Configure(EntityTypeBuilder<GeneroModel> builder)
        {
            builder.HasData(
                new GeneroModel { Id = 1, Nombre = "Acción" },
                new GeneroModel { Id = 2, Nombre = "Ciencia Ficción" },
                new GeneroModel { Id = 3, Nombre = "Drama" },
                new GeneroModel { Id = 4, Nombre = "Comedia" },
                new GeneroModel { Id = 5, Nombre = "Terror" },
                new GeneroModel { Id = 6, Nombre = "Aventura" },
                new GeneroModel { Id = 7, Nombre = "Fantasía" },
                new GeneroModel { Id = 8, Nombre = "Animación" },
                new GeneroModel { Id = 9, Nombre = "Crimen" },
                new GeneroModel { Id = 10, Nombre = "Misterio" },
                new GeneroModel { Id = 11, Nombre = "Romance" },
                new GeneroModel { Id = 12, Nombre = "Suspense" },
                new GeneroModel { Id = 13, Nombre = "Bélico" },
                new GeneroModel { Id = 14, Nombre = "Western" },
                new GeneroModel { Id = 15, Nombre = "Musical" },
                new GeneroModel { Id = 16, Nombre = "Documental" },
                new GeneroModel { Id = 17, Nombre = "Histórico" },
                new GeneroModel { Id = 18, Nombre = "Biográfico" },
                new GeneroModel { Id = 19, Nombre = "Deportes" },
                new GeneroModel { Id = 20, Nombre = "Familiar" },
                new GeneroModel { Id = 21, Nombre = "Cine Negro" },
                new GeneroModel { Id = 22, Nombre = "Catástrofes" },
                new GeneroModel { Id = 23, Nombre = "Independiente" },
                new GeneroModel { Id = 24, Nombre = "Experimental" },
                new GeneroModel { Id = 25, Nombre = "Adolescente" },
                new GeneroModel { Id = 26, Nombre = "Superhéroes" },
                new GeneroModel { Id = 27, Nombre = "Navideño" },
                new GeneroModel { Id = 28, Nombre = "Thriller Psicológico" },
                new GeneroModel { Id = 29, Nombre = "Realismo Mágico" },
                new GeneroModel { Id = 30, Nombre = "Cyberpunk" }
            );
        }
    }
}
