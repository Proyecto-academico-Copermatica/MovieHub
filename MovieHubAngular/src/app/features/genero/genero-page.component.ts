import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

import { Genero } from '../../models/genero.model';
import { MovieRow } from '../../models/movie.model';
import { trackByRowTitle } from '../../shared/utils/track-by';
import { GenreBannerComponent } from './genre-banner.component';
import { MovieRowComponent } from '../home/movie-row.component';

@Component({
  selector: 'app-genero-page',
  standalone: true,
  imports: [CommonModule, GenreBannerComponent, MovieRowComponent],
  templateUrl: './genero-page.component.html'
})
export class GeneroPageComponent {
  readonly genero = input.required<Genero>();
  readonly rows = input.required<MovieRow[]>();

  protected readonly trackByRowTitle = trackByRowTitle;
}
