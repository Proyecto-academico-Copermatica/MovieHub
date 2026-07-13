import { Pipe, PipeTransform } from '@angular/core';

const DEFAULT_MAX = 200;

@Pipe({
  name: 'truncate',
  standalone: true
})
export class TruncatePipe implements PipeTransform {
  transform(value: string | null, max: number = DEFAULT_MAX): string {
    if (!value) return '';
    const cleaned = value.replace(/\s+/g, ' ').trim();
    return cleaned.length > max ? cleaned.slice(0, max - 3) + '...' : cleaned;
  }
}
