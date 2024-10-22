using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManufacturingManagementSystem.Controllers;
using ManufacturingManagementSystem.Models;
using ManufacturingManagementSystem.Data;
using ManufacturingManagementSystem.DTOs;

namespace ManufacturingManagementSystem.Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly OrdersController _controller;
        private readonly ApplicationDbContext _context;

        public OrdersControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrdersTestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();
            _context.Orders.AddRange(new List<Order>
            {
                new Order { Id = 1, ProductName = "Product A", Quantity = 10, Status = OrderStatus.Pending },
                new Order { Id = 2, ProductName = "Product B", Quantity = 20, Status = OrderStatus.InProgress }
            });
            _context.SaveChanges();

            _controller = new OrdersController(_context);
        }

        [Fact]
        public async Task GetOrders_ReturnsAllOrders()
        {
            // Act
            var result = await _controller.GetOrders(new OrderQueryParameters());

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<Order>>>(result);
            var orders = Assert.IsAssignableFrom<IEnumerable<Order>>(okResult.Value);
            Assert.Equal(2, orders.Count());
        }

        [Fact]
        public async Task GetOrder_ReturnsOrderById()
        {
            // Act
            var result = await _controller.GetOrder(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Order>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var order = Assert.IsType<Order>(okResult.Value);
            Assert.Equal(1, order.Id);
        }

        [Fact]
        public async Task GetOrder_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetOrder(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateOrder_AddsOrder()
        {
            // Arrange
            var newOrderDto = new OrderCreateDto
            {
                ProductName = "Product C",
                Quantity = 15,
                Materials = new List<OrderMaterialDto>
                {
                    new OrderMaterialDto { MaterialId = 1, QuantityUsed = 5 }
                }
            };

            // Act
            var result = await _controller.CreateOrder(newOrderDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var order = Assert.IsType<Order>(createdAtActionResult.Value);
            Assert.Equal("Product C", order.ProductName);
            Assert.Equal(15, order.Quantity);
        }

        [Fact]
        public async Task UpdateOrderStatus_UpdatesStatus()
        {
            // Act
            var result = await _controller.UpdateOrderStatus(1, OrderStatus.Completed);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var updatedOrder = await _context.Orders.FindAsync(1);
            Assert.Equal(OrderStatus.Completed, updatedOrder.Status);
        }

        [Fact]
        public async Task UpdateOrderStatus_ReturnsNotFound()
        {
            // Act
            var result = await _controller.UpdateOrderStatus(999, OrderStatus.Completed);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_DeletesOrder()
        {
            // Act
            var result = await _controller.DeleteOrder(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.Orders.FindAsync(1));
        }

        [Fact]
        public async Task DeleteOrder_ReturnsNotFound()
        {
            // Act
            var result = await _controller.DeleteOrder(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
