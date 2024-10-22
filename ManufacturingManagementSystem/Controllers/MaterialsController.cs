using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManufacturingManagementSystem.Data;
using ManufacturingManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using ManufacturingManagementSystem.DTOs;

namespace ManufacturingManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MaterialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/materials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialReadDto>>> GetMaterials()
        {
            var materials = await _context.Materials.ToListAsync();

            var materialDtos = materials.Select(material => new MaterialReadDto
            {
                Id = material.Id,
                Name = material.Name,
                QuantityAvailable = material.QuantityAvailable,
                UnitOfMeasure = material.UnitOfMeasure
            }).ToList();

            return Ok(materialDtos);
        }

        // GET: api/materials/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialReadDto>> GetMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
                return NotFound();

            var materialDto = new MaterialReadDto
            {
                Id = material.Id,
                Name = material.Name,
                QuantityAvailable = material.QuantityAvailable,
                UnitOfMeasure = material.UnitOfMeasure
            };

            return Ok(materialDto);
        }

        // POST: api/materials
        [HttpPost]
        public async Task<ActionResult<Material>> CreateMaterial([FromBody] MaterialCreateDto materialDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var material = new Material
            {
                Name = materialDto.Name,
                QuantityAvailable = materialDto.QuantityAvailable,
                UnitOfMeasure = materialDto.UnitOfMeasure
            };

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaterial), new { id = material.Id }, material);
        }

        // PUT: api/materials/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterialQuantity(int id, [FromBody] int quantity)
        {
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
                return NotFound();

            material.QuantityAvailable = quantity;

            _context.Entry(material).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/materials/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
                return NotFound();

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
