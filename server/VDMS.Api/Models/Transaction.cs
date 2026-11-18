namespace VDMS.Api.Models;

public enum TransactionType
{
    Inbound,
    Outbound,
    Adjustment
}

public class Transaction
{
    public int Id { get; set; }
    public int PartId { get; set; }
    public Part? Part { get; set; }

    public TransactionType Type { get; set; }
    public int Quantity { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string Reference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
