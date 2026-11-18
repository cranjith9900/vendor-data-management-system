import { AfterViewInit, Component } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { ReportService } from '../../services/report.service';

Chart.register(...registerables);

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html'
})
export class ReportsComponent implements AfterViewInit {
  error = '';

  constructor(private reportService: ReportService) {}

  ngAfterViewInit(): void {
    this.loadInventoryChart();
    this.loadTrendChart();
    this.loadVendorChart();
  }

  private loadInventoryChart(): void {
    this.reportService.getInventorySummary().subscribe({
      next: (data) => {
        new Chart('inventoryChart', {
          type: 'bar',
          data: {
            labels: data.map((d) => d.category),
            datasets: [
              {
                label: 'Inventory Value ($)',
                data: data.map((d) => d.totalValue),
                backgroundColor: '#0d6efd'
              }
            ]
          },
          options: { responsive: true, plugins: { title: { display: true, text: 'Inventory Value by Category' } } }
        });
      },
      error: () => (this.error = 'Unable to load report data. Is the API running?')
    });
  }

  private loadTrendChart(): void {
    this.reportService.getTransactionTrend(30).subscribe({
      next: (data) => {
        new Chart('trendChart', {
          type: 'line',
          data: {
            labels: data.map((d) => d.period),
            datasets: [
              { label: 'Inbound', data: data.map((d) => d.inboundQty), borderColor: '#198754', fill: false },
              { label: 'Outbound', data: data.map((d) => d.outboundQty), borderColor: '#dc3545', fill: false }
            ]
          },
          options: { responsive: true, plugins: { title: { display: true, text: 'Transaction Trend (Last 30 Days)' } } }
        });
      },
      error: () => (this.error = 'Unable to load report data. Is the API running?')
    });
  }

  private loadVendorChart(): void {
    this.reportService.getTopVendors(5).subscribe({
      next: (data) => {
        new Chart('vendorChart', {
          type: 'pie',
          data: {
            labels: data.map((d) => d.vendorName),
            datasets: [
              {
                label: 'Inventory Value',
                data: data.map((d) => d.inventoryValue),
                backgroundColor: ['#0d6efd', '#198754', '#ffc107', '#dc3545', '#6f42c1']
              }
            ]
          },
          options: { responsive: true, plugins: { title: { display: true, text: 'Inventory Value by Vendor' } } }
        });
      },
      error: () => (this.error = 'Unable to load report data. Is the API running?')
    });
  }
}
