import { Injectable, signal } from '@angular/core';
import { Movie, MovieRow } from '../../models/movie.model';
import { MOVIES_PER_ROW } from '../../shared/constants';

@Injectable({ providedIn: 'root' })
export class MovieStateService {
  readonly allMovies = signal<Movie[]>([]);
  readonly heroMovie = signal<Movie | null>(null);
  readonly rows = signal<MovieRow[]>([]);

  setMovies(movies: Movie[]): void {
    this.allMovies.set(movies);
    this.heroMovie.set(this.pickHeroMovie(movies));
    this.rows.set(this.buildRowsByGenre(movies));
  }

  pickHeroMovie(movies: Movie[]): Movie | null {
    if (movies.length === 0) return null;
    return [...movies].sort((a, b) => b.puntuacionMedia - a.puntuacionMedia)[0];
  }

  filterByGenre(nombre: string): MovieRow[] {
    return this.rows().filter((r) => r.title === nombre);
  }

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
