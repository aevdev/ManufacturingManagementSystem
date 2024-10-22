using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManufacturingManagementSystem.Data;
using ManufacturingManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using ManufacturingManagementSystem.DTOs;

namespace ManufacturingManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/orders
        // Прим. GET /api/orders?status=0&startDate=2024-01-01&endDate=2024-12-31
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders([FromQuery] OrderQueryParameters queryParameters)
        {
            var orders = _context.Orders.AsQueryable();

            if (queryParameters.Status.HasValue)
            {
                orders = orders.Where(o => o.Status == queryParameters.Status.Value);
            }

            if (queryParameters.StartDate.HasValue)
            {
                orders = orders.Where(o => o.CreatedAt >= TimeZoneInfo.ConvertTime(queryParameters.StartDate.Value, TimeZoneInfo.Utc));
            }

            if (queryParameters.EndDate.HasValue)
            {
                orders = orders.Where(o => o.CreatedAt <= TimeZoneInfo.ConvertTime(queryParameters.EndDate.Value, TimeZoneInfo.Utc));
            }
            return await orders.ToListAsync();
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderMaterials)
                .ThenInclude(om => om.Material)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = new Order
            {
                ProductName = orderDto.ProductName,
                Quantity = orderDto.Quantity,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                OrderMaterials = orderDto.Materials.Select(m => new OrderMaterial
                {
                    MaterialId = m.MaterialId,
                    QuantityUsed = m.QuantityUsed
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // PUT: api/orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            order.Status = status;

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
    public class OrderQueryParameters
    {
        public OrderStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
