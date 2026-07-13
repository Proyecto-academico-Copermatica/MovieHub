import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { Movie, MovieRow } from './models/movie.model';
import { MovieService } from './services/movie.service';

const MOVIES_PER_ROW = 20;

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  protected readonly title = signal('MovieHubAngular');

  protected readonly loading = signal(true);
  protected readonly error = signal<string | null>(null);
  protected readonly heroMovie = signal<Movie | null>(null);
  protected readonly rows = signal<MovieRow[]>([]);

  constructor(private movieService: MovieService) {}

  ngOnInit(): void {
    this.movieService.getAllMovies().subscribe({
      next: (movies) => {
        this.heroMovie.set(this.pickHeroMovie(movies));
        this.rows.set(this.buildRowsByGenre(movies));
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error cargando películas', err);
        this.error.set('No se ha podido conectar con la API de MovieHub. Comprueba que el backend esté arrancado.');
        this.loading.set(false);
      }
    });
  }

  /** La mejor valorada hace de portada del hero. */
  private pickHeroMovie(movies: Movie[]): Movie | null {
    if (movies.length === 0) return null;
    return [...movies].sort((a, b) => b.puntuacionMedia - a.puntuacionMedia)[0];
  }

  /** Agrupa las películas por género y arma las filas estilo Netflix. */
  private buildRowsByGenre(movies: Movie[]): MovieRow[] {
    const generosUnicos = Array.from(new Set(movies.flatMap((m) => m.generos)));

    return generosUnicos
      .map((genero) => ({
        title: genero,
        movies: movies
          .filter((m) => m.generos.includes(genero))
          .sort((a, b) => b.puntuacionMedia - a.puntuacionMedia)
          .slice(0, MOVIES_PER_ROW)
      }))
      .filter((row) => row.movies.length > 0);
  }
}