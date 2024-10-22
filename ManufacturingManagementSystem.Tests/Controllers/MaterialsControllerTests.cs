using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManufacturingManagementSystem.Controllers;
using ManufacturingManagementSystem.Models;
using ManufacturingManagementSystem.Data;
using ManufacturingManagementSystem.DTOs;

namespace ManufacturingManagementSystem.Tests.Controllers
{
    public class MaterialsControllerTests
    {
        private readonly MaterialsController _controller;
        private readonly ApplicationDbContext _context;

        public MaterialsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MaterialsTestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();
            _context.Materials.AddRange(new List<Material>
            {
                new Material { Id = 1, Name = "Material A", QuantityAvailable = 100, UnitOfMeasure = "kg" },
                new Material { Id = 2, Name = "Material B", QuantityAvailable = 200, UnitOfMeasure = "kg" }
            });
            _context.SaveChanges();

            _controller = new MaterialsController(_context);
        }

        [Fact]
        public async Task GetMaterials_ReturnsAllMaterials()
        {
            // Act
            var result = await _controller.GetMaterials();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<MaterialReadDto>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var materials = Assert.IsAssignableFrom<IEnumerable<MaterialReadDto>>(okResult.Value);
            Assert.Equal(2, materials.Count());
        }

        [Fact]
        public async Task GetMaterial_ReturnsMaterialById()
        {
            // Act
            var result = await _controller.GetMaterial(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<MaterialReadDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var material = Assert.IsType<MaterialReadDto>(okResult.Value);
            Assert.Equal(1, material.Id);
        }

        [Fact]
        public async Task GetMaterial_ReturnsNotFound()
        {
            var result = await _controller.GetMaterial(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateMaterial_AddsMaterial()
        {
            // Arrange
            var materialDto = new MaterialCreateDto
            {
                Name = "Material C",
                QuantityAvailable = 150,
                UnitOfMeasure = "kg"
            };

            // Act
            var result = await _controller.CreateMaterial(materialDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var material = Assert.IsType<Material>(createdAtActionResult.Value);
            Assert.Equal("Material C", material.Name);
            Assert.Equal(150, material.QuantityAvailable);
            Assert.Equal("kg", material.UnitOfMeasure);

            var materialInDb = await _context.Materials.FindAsync(material.Id);
            Assert.NotNull(materialInDb);
            Assert.Equal("Material C", materialInDb.Name);
        }

        [Fact]
        public async Task UpdateMaterialQuantity_UpdatesQuantity()
        {
            // Act
            var result = await _controller.UpdateMaterialQuantity(1, 120);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var updatedMaterial = await _context.Materials.FindAsync(1);
            Assert.Equal(120, updatedMaterial.QuantityAvailable);
        }

        [Fact]
        public async Task UpdateMaterialQuantity_ReturnsNotFound()
        {
            // Act
            var result = await _controller.UpdateMaterialQuantity(999, 50);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteMaterial_DeletesMaterial()
        {
            // Act
            var result = await _controller.DeleteMaterial(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.Materials.FindAsync(1));
        }

        [Fact]
        public async Task DeleteMaterial_ReturnsNotFound()
        {
            // Act
            var result = await _controller.DeleteMaterial(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
