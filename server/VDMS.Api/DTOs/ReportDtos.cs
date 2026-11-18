namespace VDMS.Api.DTOs;

public record InventorySummaryDto(string Category, int PartCount, int TotalQuantity, decimal TotalValue);

public record TransactionTrendDto(string Period, int InboundQty, int OutboundQty);

public record TopVendorDto(string VendorName, int PartCount, decimal InventoryValue);
