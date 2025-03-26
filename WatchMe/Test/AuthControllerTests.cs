using Microsoft.AspNetCore.Mvc;
using Moq;
using WatchMe.Controllers;
using WatchMe.Data;
using WatchMe.Models;
using WatchMe.Dtos;
using WatchMe.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.Collections.Generic;

namespace WatchMe.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly Mock<EmailService> _mockEmailService;
        private readonly Mock<ResetPasswordService> _mockResetPasswordService;
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockContext = new Mock<AppDbContext>();
            _mockEmailService = new Mock<EmailService>();
            _mockResetPasswordService = new Mock<ResetPasswordService>();
            _mockLogger = new Mock<ILogger<AuthController>>();

            _controller = new AuthController(
                _mockContext.Object,
                _mockEmailService.Object,
                _mockResetPasswordService.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "Password123" };
            var hashedPassword = HashPassword("Password123");

            var users = new List<User> {
                new User { Email = "test@example.com", Password = hashedPassword, Nickname = "testuser" }
            };

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IAsyncEnumerable<User>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new AsyncEnumerator<User>(users.GetEnumerator()));

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "wrong@example.com", Password = "wrongpassword" };

            var users = new List<User> {
                new User { Email = "test@example.com", Password = HashPassword("Password123"), Nickname = "testuser" }
            };

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IAsyncEnumerable<User>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new AsyncEnumerator<User>(users.GetEnumerator()));

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hashedBytes = sha256.ComputeHash(bytes);
                return System.Convert.ToBase64String(hashedBytes);
            }
        }
    }

    public class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public AsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current => _inner.Current;

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_inner.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask();
        }
    }
}