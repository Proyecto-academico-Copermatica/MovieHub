import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Genero } from '../models/genero.model';

const API_BASE_URL = 'https://localhost:7154/api';

@Injectable({
  providedIn: 'root'
})
export class GeneroService {
  private readonly http = inject(HttpClient);

  getAll(): Observable<Genero[]> {
    return this.http.get<Genero[]>(`${API_BASE_URL}/Generos`);
  }
}
