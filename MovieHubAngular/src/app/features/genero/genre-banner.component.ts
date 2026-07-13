import { Component, input } from '@angular/core';
import { Genero } from '../../models/genero.model';

@Component({
  selector: 'app-genre-banner',
  standalone: true,
  imports: [],
  templateUrl: './genre-banner.component.html',
  styleUrl: './genre-banner.component.scss'
})
export class GenreBannerComponent {
  readonly genero = input.required<Genero>();
}
