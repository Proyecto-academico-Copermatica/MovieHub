import { Component, input, output, viewChild, afterNextRender, signal, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';

import { Movie, MovieRow } from '../../../models/movie.model';
import { trackByMovieId } from '../../../shared/utils/track-by';
import { MatIconModule } from '@angular/material/icon';
import { MovieCardComponent } from './movie-card.component';

@Component({
  selector: 'app-movie-row',
  standalone: true,
  imports: [CommonModule, MatIconModule, MovieCardComponent],
  templateUrl: './movie-row.component.html',
  styleUrl: './movie-row.component.scss'
})
export class MovieRowComponent {
  readonly row = input.required<MovieRow>();
  readonly movieClick = output<Movie>();

  protected readonly trackByMovieId = trackByMovieId;
  protected readonly canScrollLeft = signal(false);
  protected readonly canScrollRight = signal(false);

  private readonly trackRef = viewChild<ElementRef<HTMLElement>>('track');

  constructor() {
    afterNextRender(() => {
      const track = this.trackRef()?.nativeElement;
      if (!track) return;

      const update = (): void => {
        const { scrollLeft, scrollWidth, clientWidth } = track;
        this.canScrollLeft.set(scrollLeft > 4);
        this.canScrollRight.set(scrollLeft + clientWidth < scrollWidth - 4);
      };

      track.addEventListener('scroll', update);

      const observer = new ResizeObserver(update);
      observer.observe(track);

      track.addEventListener('wheel', (e: WheelEvent) => {
        if (e.deltaY === 0) return;

        const canScrollHorizontally = e.deltaY > 0
          ? track.scrollLeft + track.clientWidth < track.scrollWidth - 4
          : track.scrollLeft > 4;

        if (canScrollHorizontally) {
          e.preventDefault();
          track.scrollLeft += e.deltaY;
          update();
        }
      }, { passive: false });

      update();
    });
  }

  protected scrollLeft(): void {
    const el = this.trackRef()?.nativeElement;
    if (!el) return;
    const amount = Math.min(el.clientWidth - 200, 600);
    el.scrollBy({ left: -amount, behavior: 'smooth' });
  }

  protected scrollRight(): void {
    const el = this.trackRef()?.nativeElement;
    if (!el) return;
    const amount = Math.min(el.clientWidth - 200, 600);
    el.scrollBy({ left: amount, behavior: 'smooth' });
  }
}
