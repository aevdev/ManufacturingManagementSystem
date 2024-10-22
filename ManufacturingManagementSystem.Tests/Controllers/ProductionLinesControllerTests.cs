using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManufacturingManagementSystem.Controllers;
using ManufacturingManagementSystem.Models;
using ManufacturingManagementSystem.Data;

namespace ManufacturingManagementSystem.Tests.Controllers
{
    public class ProductionLinesControllerTests
    {
        private readonly ProductionLinesController _controller;
        private readonly ApplicationDbContext _context;

        public ProductionLinesControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductionLinesTestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();
            _context.ProductionLines.AddRange(new List<ProductionLine>
            {
                new ProductionLine { Id = 1, Name = "Line A", Status = ProductionLineStatus.Active },
                new ProductionLine { Id = 2, Name = "Line B", Status = ProductionLineStatus.Inactive }
            });
            _context.SaveChanges();

            _controller = new ProductionLinesController(_context);
        }

        [Fact]
        public async Task GetProductionLines_ReturnsAllLines()
        {
            // Act
            var result = await _controller.GetProductionLines();

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<ProductionLine>>>(result);
            var lines = Assert.IsAssignableFrom<IEnumerable<ProductionLine>>(okResult.Value);
            Assert.Equal(2, lines.Count());
        }

        [Fact]
        public async Task GetProductionLine_ReturnsLineById()
        {
            // Act
            var result = await _controller.GetProductionLine(1);

            // Assert
            var okResult = Assert.IsType<ActionResult<ProductionLine>>(result);
            var line = Assert.IsType<ProductionLine>(okResult.Value);
            Assert.Equal(1, line.Id);
        }

        [Fact]
        public async Task GetProductionLine_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetProductionLine(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateProductionLine_AddsLine()
        {
            // Arrange
            var newLine = new ProductionLine
            {
                Name = "Line C",
                Status = ProductionLineStatus.Active
            };

            // Act
            var result = await _controller.CreateProductionLine(newLine);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var line = Assert.IsType<ProductionLine>(createdAtActionResult.Value);
            Assert.Equal("Line C", line.Name);
        }

        [Fact]
        public async Task UpdateProductionLineStatus_UpdatesStatus()
        {
            // Act
            var result = await _controller.UpdateProductionLineStatus(1, ProductionLineStatus.Inactive);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var updatedLine = await _context.ProductionLines.FindAsync(1);
            Assert.Equal(ProductionLineStatus.Inactive, updatedLine.Status);
        }

        [Fact]
        public async Task UpdateProductionLineStatus_ReturnsNotFound()
        {
            // Act
            var result = await _controller.UpdateProductionLineStatus(999, ProductionLineStatus.Active);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProductionLine_DeletesLine()
        {
            // Act
            var result = await _controller.DeleteProductionLine(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.ProductionLines.FindAsync(1));
        }

        [Fact]
        public async Task DeleteProductionLine_ReturnsNotFound()
        {
            // Act
            var result = await _controller.DeleteProductionLine(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
