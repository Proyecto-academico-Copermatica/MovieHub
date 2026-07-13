export interface Movie {
  id: number;
  titulo: string;
  descripcion: string;
  duracion: number;
  anioEstreno: number;
  director: string;
  imagen: string;
  puntuacionMedia: number;
  generos: string[];
}
 
export interface PaginatedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
 
export interface MovieRow {
  title: string;
  movies: Movie[];
}
 