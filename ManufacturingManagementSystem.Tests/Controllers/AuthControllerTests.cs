using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManufacturingManagementSystem.Controllers;
using ManufacturingManagementSystem.Models;
using ManufacturingManagementSystem.Data;
using Microsoft.Extensions.Configuration;
using ManufacturingManagementSystem.DTOs;

namespace ManufacturingManagementSystem.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthTestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();

            // Настраиваем IConfiguration для тестов
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", "TestSecretKeyForJwtTokenTestSecretKeyForJwtTokenTestSecretKeyForJwtToken"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _controller = new AuthController(_context, _configuration);
        }

        [Fact]
        public async Task Register_CreatesNewUser()
        {
            // Arrange
            var userDto = new UserDto
            {
                Username = "testuser",
                Password = "password123"
            };

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Регистрация успешна", okResult.Value);

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
            Assert.NotNull(userInDb);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserExists()
        {
            // Arrange
            var existingUser = new User
            {
                Username = "existinguser",
                PasswordHash = "hash"
            };
            _context.Users.Add(existingUser);
            _context.SaveChanges();

            var userDto = new UserDto
            {
                Username = "existinguser",
                Password = "password123"
            };

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Пользователь уже существует", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsToken_WhenCredentialsAreValid()
        {
            // Arrange
            var password = "password123";
            var user = new User
            {
                Username = "testuser",
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password)
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            var userDto = new UserDto
            {
                Username = "testuser",
                Password = password
            };

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenResponse = okResult.Value; //Явно обозначаем что мы ищем в поле value.
            Assert.NotNull(tokenResponse);
            Assert.IsType<string>(tokenResponse.ToString());
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var userDto = new UserDto
            {
                Username = "nonexistentuser",
                Password = "wrongpassword"
            };

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Неверный логин или пароль", unauthorizedResult.Value);
        }
    }
}
