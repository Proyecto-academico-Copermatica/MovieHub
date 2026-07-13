import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';

import { Genero } from '../../models/genero.model';
import { ActiveView } from '../../shared/types';
import { trackByGeneroId } from '../../shared/utils/track-by';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule, MatIconModule, MatButtonModule,
    MatMenuModule, MatTooltipModule, MatDividerModule
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  readonly generos = input<Genero[]>([]);
  readonly activeView = input<ActiveView>('home');
  readonly peliculasExpanded = input(false);
  readonly selectedGenero = input<Genero | null>(null);

  readonly homeClick = output<void>();
  readonly selectGenero = output<Genero>();
  readonly togglePeliculas = output<void>();

  protected readonly trackByGeneroId = trackByGeneroId;
}
