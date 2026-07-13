import { CommonModule } from '@angular/common';
import { Component, OnInit, signal, ChangeDetectionStrategy, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

import { TruncatePipe } from './shared/pipes/truncate.pipe';
import { RatingPercentPipe } from './shared/pipes/rating-percent.pipe';

import { Movie, MovieRow } from './models/movie.model';
import { Genero } from './models/genero.model';
import { ActiveView } from './shared/types';
import { MovieService } from './services/movie.service';
import { GeneroService } from './services/genero.service';
import { NavbarComponent } from './core/layout/navbar.component';
import {
  trackByMovieId,
  trackByRowTitle
} from './shared/utils/track-by';
import { MOVIES_PER_ROW } from './shared/constants';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet, CommonModule, NavbarComponent,
    MatCardModule, MatChipsModule, MatDividerModule,
    MatIconModule, MatTooltipModule,
    TruncatePipe, RatingPercentPipe
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class App implements OnInit {
  private readonly generoService = inject(GeneroService);
  private readonly movieService = inject(MovieService);

  protected readonly loading = signal(true);
  protected readonly error = signal<string | null>(null);
  protected readonly heroMovie = signal<Movie | null>(null);
  protected readonly rows = signal<MovieRow[]>([]);
  protected readonly generos = signal<Genero[]>([]);
  protected readonly selectedGenero = signal<Genero | null>(null);
  protected readonly activeView = signal<ActiveView>('home');
  protected readonly peliculasExpanded = signal(false);

  protected readonly trackByMovieId = trackByMovieId;
  protected readonly trackByRowTitle = trackByRowTitle;

  private allMovies: Movie[] = [];

  ngOnInit(): void {
    this.loadGeneros();
    this.loadMovies();
  }

  private loadGeneros(): void {
    this.generoService.getAll().subscribe({
      next: (generos) => this.generos.set(generos),
      error: () => console.warn('No se pudieron cargar los géneros')
    });
  }

  private loadMovies(): void {
    this.movieService.getCatalogMovies().subscribe({
      next: (movies) => {
        this.allMovies = movies;
        this.heroMovie.set(this.pickHeroMovie(movies));
        this.rows.set(this.buildRowsByGenre(movies));
        this.loading.set(false);
      },
      error: () => {
        this.error.set('No se ha podido conectar con la API de MovieHub. Comprueba que el backend esté arrancado.');
        this.loading.set(false);
      }
    });
  }

  selectGenero(genero: Genero): void {
    this.selectedGenero.set(genero);
    this.activeView.set('genero');
    this.peliculasExpanded.set(false);
    const generoRows = this.buildRowsByGenre(this.allMovies).filter(
      (r) => r.title === genero.nombre
    );
    this.rows.set(generoRows);
  }

  goHome(): void {
    this.selectedGenero.set(null);
    this.activeView.set('home');
    this.peliculasExpanded.set(false);
    this.heroMovie.set(this.pickHeroMovie(this.allMovies));
    this.rows.set(this.buildRowsByGenre(this.allMovies));
  }

  togglePeliculas(): void {
    this.peliculasExpanded.update((v) => !v);
  }

  private pickHeroMovie(movies: Movie[]): Movie | null {
    if (movies.length === 0) return null;
    return [...movies].sort((a, b) => b.puntuacionMedia - a.puntuacionMedia)[0];
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
