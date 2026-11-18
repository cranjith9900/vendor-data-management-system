import { Component, OnInit } from '@angular/core';
import { Part } from '../../models/models';
import { PartService } from '../../services/part.service';

@Component({
  selector: 'app-part-list',
  templateUrl: './part-list.component.html'
})
export class PartListComponent implements OnInit {
  parts: Part[] = [];
  search = '';
  loading = false;
  error = '';

  constructor(private partService: PartService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = '';
    this.partService.getAll(this.search || undefined).subscribe({
      next: (parts) => {
        this.parts = parts;
        this.loading = false;
      },
      error: () => {
        this.error = 'Unable to load parts. Is the API running?';
        this.loading = false;
      }
    });
  }

  deletePart(id: number): void {
    if (!confirm('Delete this part?')) return;
    this.partService.delete(id).subscribe(() => this.load());
  }
}
