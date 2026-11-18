import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Part, TransactionType } from '../../models/models';
import { PartService } from '../../services/part.service';
import { TransactionService } from '../../services/transaction.service';

@Component({
  selector: 'app-transaction-form',
  templateUrl: './transaction-form.component.html'
})
export class TransactionFormComponent implements OnInit {
  parts: Part[] = [];
  error = '';
  saving = false;

  model = {
    partId: 0,
    type: 'Inbound' as TransactionType,
    quantity: 1,
    reference: '',
    notes: ''
  };

  constructor(
    private partService: PartService,
    private transactionService: TransactionService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.partService.getAll().subscribe((parts) => {
      this.parts = parts;
      if (parts.length) this.model.partId = parts[0].id;
    });
  }

  save(): void {
    this.saving = true;
    this.error = '';
    this.transactionService.create(this.model).subscribe({
      next: () => this.router.navigate(['/transactions']),
      error: (err: any) => {
        this.error = err?.error ?? 'Failed to record transaction.';
        this.saving = false;
      }
    });
  }
}
