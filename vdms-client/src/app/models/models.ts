export interface Vendor {
  id: number;
  name: string;
  contactEmail: string;
  phone: string;
  address: string;
}

export interface Part {
  id: number;
  partNumber: string;
  description: string;
  category: string;
  unitPrice: number;
  quantityOnHand: number;
  warehouseLocation: string;
  vendorId: number;
  vendorName?: string;
}

export type TransactionType = 'Inbound' | 'Outbound' | 'Adjustment';

export interface Transaction {
  id: number;
  partId: number;
  partNumber?: string;
  type: TransactionType;
  quantity: number;
  transactionDate: string;
  reference: string;
  notes: string;
}

export interface InventorySummary {
  category: string;
  partCount: number;
  totalQuantity: number;
  totalValue: number;
}

export interface TransactionTrend {
  period: string;
  inboundQty: number;
  outboundQty: number;
}

export interface TopVendor {
  vendorName: string;
  partCount: number;
  inventoryValue: number;
}
