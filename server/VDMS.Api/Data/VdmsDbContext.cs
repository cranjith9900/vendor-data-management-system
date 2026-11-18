using Microsoft.EntityFrameworkCore;
using VDMS.Api.Models;

namespace VDMS.Api.Data;

public class VdmsDbContext : DbContext
{
    public VdmsDbContext(DbContextOptions<VdmsDbContext> options) : base(options) { }

    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Part>()
            .HasIndex(p => p.PartNumber)
            .IsUnique();

        modelBuilder.Entity<Part>()
            .HasOne(p => p.Vendor)
            .WithMany(v => v.Parts)
            .HasForeignKey(p => p.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Part)
            .WithMany(p => p.Transactions)
            .HasForeignKey(t => t.PartId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Vendor>().HasData(
            new Vendor { Id = 1, Name = "Acme Supply Co.", ContactEmail = "sales@acmesupply.com", Phone = "555-0101", Address = "100 Industrial Way, Springfield" },
            new Vendor { Id = 2, Name = "Northbridge Parts", ContactEmail = "orders@northbridgeparts.com", Phone = "555-0199", Address = "22 Harbor Rd, Portview" }
        );

        modelBuilder.Entity<Part>().HasData(
            new Part { Id = 1, PartNumber = "PN-1001", Description = "Hex Bolt M8x40", Category = "Fasteners", UnitPrice = 0.35m, QuantityOnHand = 5000, WarehouseLocation = "A1-03", VendorId = 1 },
            new Part { Id = 2, PartNumber = "PN-1002", Description = "Ball Bearing 608ZZ", Category = "Bearings", UnitPrice = 1.20m, QuantityOnHand = 1200, WarehouseLocation = "B2-11", VendorId = 1 },
            new Part { Id = 3, PartNumber = "PN-2050", Description = "Hydraulic Hose 1/2in", Category = "Hydraulics", UnitPrice = 14.75m, QuantityOnHand = 300, WarehouseLocation = "C4-02", VendorId = 2 }
        );
    }
}
