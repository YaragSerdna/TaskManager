using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data;
using TaskManager.Helpers;
using TaskManager.Models;

namespace TaskManager.Tests
{
    public class AuthServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthServiceTests()
        {
            // Configurar DbContext en memoria
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            // Configurar JwtHelper con valores simulados
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Jwt:Key", "SuperSecretKey12345!" },
                    { "Jwt:Issuer", "TestIssuer" },
                    { "Jwt:Audience", "TestAudience" }
                })
                .Build();

            _jwtHelper = new JwtHelper(configuration);
        }

        [Fact]
        public void HashPassword_ShouldReturnHashedString()
        {
            // Arrange
            var password = "MyPassword123";
            var hashedPassword = HashPassword(password);

            // Act
            var hashedPasswordAgain = HashPassword(password);

            // Assert
            Assert.Equal(hashedPassword, hashedPasswordAgain);
        }

        [Fact]
        public async Task Register_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = HashPassword("MyPassword123"),
                Role = "User"
            };

            // Act
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Assert
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
            Assert.NotNull(userInDb);
            Assert.Equal(user.Email, userInDb.Email);
        }

        [Fact]
        public async Task VerifyPassword_ShouldReturnTrueForValidPassword()
        {
            // Arrange
            var password = "MyPassword123";
            var hashedPassword = HashPassword(password);

            // Act
            var isValid = VerifyPassword(hashedPassword, password);

            // Assert
            Assert.True(isValid);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(passwordBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private bool VerifyPassword(string hash, string password)
        {
            var hashedInputPassword = HashPassword(password);
            return hashedInputPassword == hash;
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalseForInvalidPassword()
        {
            // Arrange
            var password = "MyPassword123";
            var hashedPassword = HashPassword(password);
            var wrongPassword = "WrongPassword456";

            // Act
            var isValid = VerifyPassword(hashedPassword, wrongPassword);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public async Task Register_ShouldNotAllowDuplicateEmail()
        {
            // Arrange
            var user1 = new User
            {
                Name = "User One",
                Email = "duplicate@example.com",
                Password = HashPassword("Password123"),
                Role = "User"
            };

            var user2 = new User
            {
                Name = "User Two",
                Email = "duplicate@example.com", // Mismo correo
                Password = HashPassword("Password456"),
                Role = "User"
            };

            _context.Users.Add(user1);
            await _context.SaveChangesAsync();

            // Act
            _context.Users.Add(user2);

            // Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNullForNonExistingUser()
        {
            // Arrange
            var nonExistentUserId = 999;

            // Act
            var user = await _context.Users.FindAsync(nonExistentUserId);

            // Assert
            Assert.Null(user);
        }


        [Fact]
        public async Task Login_ShouldFailForInvalidCredentials()
        {
            // Arrange
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = HashPassword("Password123"),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var wrongPassword = "WrongPassword";

            // Act
            var isValid = VerifyPassword(user.Password, wrongPassword);

            // Assert
            Assert.False(isValid);
        }

    }
}
