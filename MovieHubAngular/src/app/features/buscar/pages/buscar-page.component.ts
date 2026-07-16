import { Component, inject, input, signal, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { toObservable } from '@angular/core/rxjs-interop';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { Subscription, switchMap, map } from 'rxjs';

import { MovieService } from '../../../core/services/movie.service';
import { Movie } from '../../../models/movie.model';

@Component({
  selector: 'app-buscar-page',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatCardModule],
  templateUrl: './buscar-page.component.html',
  styleUrl: './buscar-page.component.scss',
})
export class BuscarPageComponent implements OnDestroy {
  private readonly movieService = inject(MovieService);
  private readonly router = inject(Router);

  readonly q = input.required<string>();
  readonly movies = signal<Movie[]>([]);
  readonly total = signal(0);
  readonly loading = signal(true);

  private readonly sub: Subscription;

  constructor() {
    const q$ = toObservable(this.q);
    this.sub = q$.pipe(
      switchMap((term) => {
        this.loading.set(true);
        return this.movieService.buscar({ q: term, pageSize: 50 }).pipe(
          map((res) => ({ items: res.items, total: res.totalCount }))
        );
      })
    ).subscribe((res) => {
      this.movies.set(res.items);
      this.total.set(res.total);
      this.loading.set(false);
    });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  goToPelicula(id: number): void {
    this.router.navigate(['/pelicula', id]);
  }
}
