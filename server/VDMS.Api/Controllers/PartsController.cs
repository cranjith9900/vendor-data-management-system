using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VDMS.Api.Data;
using VDMS.Api.DTOs;
using VDMS.Api.Models;

namespace VDMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartsController : ControllerBase
{
    private readonly VdmsDbContext _db;
    public PartsController(VdmsDbContext db) => _db = db;

    private static PartDto ToDto(Part p) => new(
        p.Id, p.PartNumber, p.Description, p.Category, p.UnitPrice,
        p.QuantityOnHand, p.WarehouseLocation, p.VendorId, p.Vendor?.Name);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartDto>>> GetAll([FromQuery] string? search, [FromQuery] string? category)
    {
        var query = _db.Parts.Include(p => p.Vendor).AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.PartNumber.Contains(search) || p.Description.Contains(search));

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category == category);

        var parts = await query.OrderBy(p => p.PartNumber).ToListAsync();
        return Ok(parts.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PartDto>> GetById(int id)
    {
        var part = await _db.Parts.Include(p => p.Vendor).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return part is null ? NotFound() : Ok(ToDto(part));
    }

    [HttpPost]
    public async Task<ActionResult<PartDto>> Create(PartUpsertDto dto)
    {
        var part = new Part
        {
            PartNumber = dto.PartNumber,
            Description = dto.Description,
            Category = dto.Category,
            UnitPrice = dto.UnitPrice,
            QuantityOnHand = dto.QuantityOnHand,
            WarehouseLocation = dto.WarehouseLocation,
            VendorId = dto.VendorId
        };

        _db.Parts.Add(part);
        await _db.SaveChangesAsync();
        await _db.Entry(part).Reference(p => p.Vendor).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = part.Id }, ToDto(part));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PartUpsertDto dto)
    {
        var part = await _db.Parts.FindAsync(id);
        if (part is null) return NotFound();

        part.PartNumber = dto.PartNumber;
        part.Description = dto.Description;
        part.Category = dto.Category;
        part.UnitPrice = dto.UnitPrice;
        part.QuantityOnHand = dto.QuantityOnHand;
        part.WarehouseLocation = dto.WarehouseLocation;
        part.VendorId = dto.VendorId;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var part = await _db.Parts.FindAsync(id);
        if (part is null) return NotFound();
        _db.Parts.Remove(part);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
