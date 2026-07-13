import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';

import { Movie } from '../../models/movie.model';
import { RatingPercentPipe } from '../../shared/pipes/rating-percent.pipe';

@Component({
  selector: 'app-movie-detail-page',
  standalone: true,
  imports: [
    CommonModule, MatButtonModule, MatIconModule,
    MatChipsModule, MatTooltipModule, RatingPercentPipe
  ],
  templateUrl: './movie-detail-page.component.html',
  styleUrl: './movie-detail-page.component.scss'
})
export class MovieDetailPageComponent {
  readonly movie = input.required<Movie>();
  readonly back = output<void>();
}
