import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface Favorito {
  peliculaId: number;
  titulo: string;
  imagen: string | null;
  puntuacionMedia: number;
}

@Injectable({ providedIn: 'root' })
export class FavoritoService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrlBase}/Favoritos`;

  getAll(): Observable<Favorito[]> {
    return this.http.get<Favorito[]>(this.baseUrl);
  }

  add(peliculaId: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${peliculaId}`, null);
  }

  remove(peliculaId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${peliculaId}`);
  }
}
