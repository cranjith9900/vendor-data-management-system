import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Vendor } from '../../models/models';
import { PartService } from '../../services/part.service';
import { VendorService } from '../../services/vendor.service';

@Component({
  selector: 'app-part-form',
  templateUrl: './part-form.component.html'
})
export class PartFormComponent implements OnInit {
  id: number | null = null;
  vendors: Vendor[] = [];
  error = '';
  saving = false;

  model = {
    partNumber: '',
    description: '',
    category: '',
    unitPrice: 0,
    quantityOnHand: 0,
    warehouseLocation: '',
    vendorId: 0
  };

  constructor(
    private partService: PartService,
    private vendorService: VendorService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.vendorService.getAll().subscribe((vendors) => {
      this.vendors = vendors;
      if (!this.model.vendorId && vendors.length) {
        this.model.vendorId = vendors[0].id;
      }
    });

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = Number(idParam);
      this.partService.getById(this.id).subscribe((part) => {
        this.model = {
          partNumber: part.partNumber,
          description: part.description,
          category: part.category,
          unitPrice: part.unitPrice,
          quantityOnHand: part.quantityOnHand,
          warehouseLocation: part.warehouseLocation,
          vendorId: part.vendorId
        };
      });
    }
  }

  save(): void {
    this.saving = true;
    this.error = '';
    const request: Observable<unknown> = this.id
      ? this.partService.update(this.id, this.model)
      : this.partService.create(this.model);

    request.subscribe({
      next: () => this.router.navigate(['/parts']),
      error: (err: any) => {
        this.error = err?.error ?? 'Failed to save part.';
        this.saving = false;
      }
    });
  }
}
