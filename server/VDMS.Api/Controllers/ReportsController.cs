using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VDMS.Api.Data;
using VDMS.Api.DTOs;
using VDMS.Api.Models;

namespace VDMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly VdmsDbContext _db;
    public ReportsController(VdmsDbContext db) => _db = db;

    [HttpGet("inventory-summary")]
    public async Task<ActionResult<IEnumerable<InventorySummaryDto>>> InventorySummary()
    {
        var summary = await _db.Parts
            .GroupBy(p => p.Category)
            .Select(g => new InventorySummaryDto(
                g.Key,
                g.Count(),
                g.Sum(p => p.QuantityOnHand),
                g.Sum(p => p.QuantityOnHand * p.UnitPrice)))
            .OrderByDescending(s => s.TotalValue)
            .ToListAsync();

        return Ok(summary);
    }

    [HttpGet("transaction-trend")]
    public async Task<ActionResult<IEnumerable<TransactionTrendDto>>> TransactionTrend([FromQuery] int days = 30)
    {
        var since = DateTime.UtcNow.Date.AddDays(-days);

        var raw = await _db.Transactions
            .Where(t => t.TransactionDate >= since)
            .ToListAsync();

        var trend = raw
            .GroupBy(t => t.TransactionDate.Date)
            .Select(g => new TransactionTrendDto(
                g.Key.ToString("yyyy-MM-dd"),
                g.Where(t => t.Type == TransactionType.Inbound).Sum(t => t.Quantity),
                g.Where(t => t.Type == TransactionType.Outbound).Sum(t => t.Quantity)))
            .OrderBy(t => t.Period)
            .ToList();

        return Ok(trend);
    }

    [HttpGet("top-vendors")]
    public async Task<ActionResult<IEnumerable<TopVendorDto>>> TopVendors([FromQuery] int take = 5)
    {
        var vendors = await _db.Parts
            .Include(p => p.Vendor)
            .GroupBy(p => p.Vendor!.Name)
            .Select(g => new TopVendorDto(
                g.Key,
                g.Count(),
                g.Sum(p => p.QuantityOnHand * p.UnitPrice)))
            .OrderByDescending(v => v.InventoryValue)
            .Take(take)
            .ToListAsync();

        return Ok(vendors);
    }
}
