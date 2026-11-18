namespace VDMS.Api.DTOs;

public record PartDto(
    int Id,
    string PartNumber,
    string Description,
    string Category,
    decimal UnitPrice,
    int QuantityOnHand,
    string WarehouseLocation,
    int VendorId,
    string? VendorName
);

public record PartUpsertDto(
    string PartNumber,
    string Description,
    string Category,
    decimal UnitPrice,
    int QuantityOnHand,
    string WarehouseLocation,
    int VendorId
);
