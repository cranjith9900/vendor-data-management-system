using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VDMS.Api.Data;
using VDMS.Api.DTOs;
using VDMS.Api.Models;

namespace VDMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly VdmsDbContext _db;
    public TransactionsController(VdmsDbContext db) => _db = db;

    private static TransactionDto ToDto(Transaction t) => new(
        t.Id, t.PartId, t.Part?.PartNumber, t.Type, t.Quantity, t.TransactionDate, t.Reference, t.Notes);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll([FromQuery] int? partId, [FromQuery] int take = 100)
    {
        var query = _db.Transactions.Include(t => t.Part).AsNoTracking().AsQueryable();

        if (partId.HasValue)
            query = query.Where(t => t.PartId == partId);

        var transactions = await query
            .OrderByDescending(t => t.TransactionDate)
            .Take(take)
            .ToListAsync();

        return Ok(transactions.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TransactionDto>> GetById(int id)
    {
        var t = await _db.Transactions.Include(x => x.Part).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return t is null ? NotFound() : Ok(ToDto(t));
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDto>> Create(TransactionCreateDto dto)
    {
        var part = await _db.Parts.FindAsync(dto.PartId);
        if (part is null) return BadRequest("Part not found.");

        if (dto.Quantity <= 0) return BadRequest("Quantity must be greater than zero.");

        if (dto.Type == TransactionType.Outbound && dto.Quantity > part.QuantityOnHand)
            return BadRequest("Insufficient quantity on hand for this outbound transaction.");

        var transaction = new Transaction
        {
            PartId = dto.PartId,
            Type = dto.Type,
            Quantity = dto.Quantity,
            Reference = dto.Reference,
            Notes = dto.Notes,
            TransactionDate = DateTime.UtcNow
        };

        part.QuantityOnHand += dto.Type switch
        {
            TransactionType.Inbound => dto.Quantity,
            TransactionType.Outbound => -dto.Quantity,
            TransactionType.Adjustment => dto.Quantity,
            _ => 0
        };

        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();
        await _db.Entry(transaction).Reference(t => t.Part).LoadAsync();

        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, ToDto(transaction));
    }
}
