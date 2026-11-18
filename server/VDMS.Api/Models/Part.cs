namespace VDMS.Api.Models;

public class Part
{
    public int Id { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int QuantityOnHand { get; set; }
    public string WarehouseLocation { get; set; } = string.Empty;

    public int VendorId { get; set; }
    public Vendor? Vendor { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
