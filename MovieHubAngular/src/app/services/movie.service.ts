import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, forkJoin, map, switchMap } from 'rxjs';

import { Movie, PaginatedResponse } from '../models/movie.model';

// El backend limita pageSize a 50 por petición, así que para traer
// el catálogo completo (250 títulos) hacemos varias llamadas y las unimos.
const API_BASE_URL = 'https://localhost:7154/api/Peliculas';
const MAX_PAGE_SIZE = 50;

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  constructor(private http: HttpClient) {}

  /** Trae una única página tal cual la devuelve la API. */
  getPage(page: number, pageSize: number = MAX_PAGE_SIZE): Observable<PaginatedResponse<Movie>> {
    return this.http.get<PaginatedResponse<Movie>>(API_BASE_URL, {
      params: { page, pageSize }
    });
  }

  /**
   * Trae TODO el catálogo paginando por debajo.
   * 1) Pide la primera página para saber cuántas páginas hay en total.
   * 2) Lanza en paralelo el resto de páginas (forkJoin).
   * 3) Concatena todos los "items" en un único array de Movie.
   */
  getAllMovies(pageSize: number = MAX_PAGE_SIZE): Observable<Movie[]> {
    return this.getPage(1, pageSize).pipe(
      switchMap((first) => {
        const remainingPages = Array.from(
          { length: first.totalPages - 1 },
          (_, i) => i + 2 // páginas 2, 3, 4...
        );

        if (remainingPages.length === 0) {
          return new Observable<Movie[]>((subscriber) => {
            subscriber.next(first.items);
            subscriber.complete();
          });
        }

        const restRequests = remainingPages.map((page) => this.getPage(page, pageSize));

        return forkJoin(restRequests).pipe(
          map((rest) => [
            ...first.items,
            ...rest.flatMap((response) => response.items)
          ])
        );
      })
    );
  }
}