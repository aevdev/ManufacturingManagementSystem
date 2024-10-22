using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManufacturingManagementSystem.Data;
using ManufacturingManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace ManufacturingManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionLinesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductionLinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/productionlines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionLine>>> GetProductionLines()
        {
            return await _context.ProductionLines.ToListAsync();
        }

        // GET: api/productionlines/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionLine>> GetProductionLine(int id)
        {
            var productionLine = await _context.ProductionLines.FindAsync(id);

            if (productionLine == null)
                return NotFound();

            return productionLine;
        }

        // POST: api/productionlines
        [HttpPost]
        public async Task<ActionResult<ProductionLine>> CreateProductionLine([FromBody] ProductionLine productionLine)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.ProductionLines.Add(productionLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductionLine), new { id = productionLine.Id }, productionLine);
        }

        // PUT: api/productionlines/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductionLineStatus(int id, [FromBody] ProductionLineStatus status)
        {
            var productionLine = await _context.ProductionLines.FindAsync(id);

            if (productionLine == null)
                return NotFound();

            productionLine.Status = status;

            _context.Entry(productionLine).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/productionlines/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductionLine(int id)
        {
            var productionLine = await _context.ProductionLines.FindAsync(id);

            if (productionLine == null)
                return NotFound();

            _context.ProductionLines.Remove(productionLine);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
