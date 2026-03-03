using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Jobs.Infrastructure.Data;
using Jobs.Infrastructure.Repositories.Repo;
using Jobs.Domain.Entities;
using Jobs.Domain.ValueObjects;
using Jobs.Domain.Enums;

namespace Jobs.Infrastructure.Tests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private DbContextOptions<JobDbContext> _options;
        private JobDbContext _context;
        private UserRepository _repository;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<JobDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique db per test
                .Options;

            _context = new JobDbContext(_options);
            _repository = new UserRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetByEmailAsync_ExistingEmail_ReturnsUser()
        {
            // Arrange
            var emailStr = "test@example.com";
            var email = Email.Create(emailStr);
            var user = new User("John Doe", email, UserRole.Applicant, e => false);

            _context.Users.Add(user); // Assuming the DbSet is named Users
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync(emailStr);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email.Value, Is.EqualTo(emailStr));
        }

        [Test]
        public async Task GetByEmailAsync_NonExistingEmail_ReturnsNull()
        {
            // Arrange
            var emailStr = "nonexisting@example.com";

            // Act
            var result = await _repository.GetByEmailAsync(emailStr);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByEmailAsync_EmptyEmail_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _repository.GetByEmailAsync(""));
        }

        [Test]
        public async Task EmailExistsAsync_ExistingEmail_ReturnsTrue()
        {
            // Arrange
            var emailStr = "exists@example.com";
            var email = Email.Create(emailStr);
            var user = new User("Jane Doe", email, UserRole.Applicant, e => false);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.EmailExistsAsync(emailStr);

            // Assert
            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task EmailExistsAsync_NonExistingEmail_ReturnsFalse()
        {
            // Act
            var exists = await _repository.EmailExistsAsync("nobody@example.com");

            // Assert
            Assert.That(exists, Is.False);
        }
    }
}
