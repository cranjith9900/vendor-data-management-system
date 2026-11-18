using VDMS.Api.Models;

namespace VDMS.Api.DTOs;

public record TransactionDto(
    int Id,
    int PartId,
    string? PartNumber,
    TransactionType Type,
    int Quantity,
    DateTime TransactionDate,
    string Reference,
    string Notes
);

public record TransactionCreateDto(
    int PartId,
    TransactionType Type,
    int Quantity,
    string Reference,
    string Notes
);
