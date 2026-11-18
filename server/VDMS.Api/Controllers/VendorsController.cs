using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VDMS.Api.Data;
using VDMS.Api.Models;

namespace VDMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendorsController : ControllerBase
{
    private readonly VdmsDbContext _db;
    public VendorsController(VdmsDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vendor>>> GetAll() =>
        Ok(await _db.Vendors.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Vendor>> GetById(int id)
    {
        var vendor = await _db.Vendors.FindAsync(id);
        return vendor is null ? NotFound() : Ok(vendor);
    }

    [HttpPost]
    public async Task<ActionResult<Vendor>> Create(Vendor vendor)
    {
        _db.Vendors.Add(vendor);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = vendor.Id }, vendor);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Vendor vendor)
    {
        if (id != vendor.Id) return BadRequest();
        _db.Entry(vendor).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vendor = await _db.Vendors.FindAsync(id);
        if (vendor is null) return NotFound();
        _db.Vendors.Remove(vendor);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
