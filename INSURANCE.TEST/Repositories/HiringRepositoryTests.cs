using FluentAssertions;
using INSURANCES.CORE.Entities;
using INSURANCES.CORE.Ports.Respository;
using INSURANCES.DATA.Factory;
using INSURANCES.DATA.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace INSURANCE.TEST.Repositories
{
    public class HiringRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IHiringRepository _repository;

        public HiringRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new HiringRepository(_context);
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldCreateHiring_WhenValidData()
        {
            // Arrange
            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Hiring",
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var result = await _repository.PostCreateHiringAsync(entity);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(entity.Id);
            result.Name.Should().Be(entity.Name);
            result.ProposalId.Should().Be(entity.ProposalId);
            result.IsApproved.Should().Be(entity.IsApproved);

            var savedEntity = await _context.HiringEntities.FindAsync(entity.Id);
            savedEntity.Should().NotBeNull();
            savedEntity!.Name.Should().Be(entity.Name);
        }

        [Fact]
        public async Task GetHiringByIdAsync_ShouldReturnHiring_WhenValidId()
        {
            // Arrange
            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Hiring",
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            _context.HiringEntities.Add(entity);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetHiringByIdAsync(entity.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(entity.Id);
            result.Name.Should().Be(entity.Name);
        }

        [Fact]
        public async Task GetHiringByIdAsync_ShouldReturnNull_WhenHiringDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _repository.GetHiringByIdAsync(nonExistentId);

            // Assert
            result.Should().BeNull();
        }

       

        [Fact]
        public async Task PostCreateHiringAsync_ShouldGenerateNewId_WhenIdIsEmpty()
        {
            // Arrange
            var entity = new HiringEntity
            {
                Id = Guid.Empty,
                Name = "Test Hiring",
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var result = await _repository.PostCreateHiringAsync(entity);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleMultipleHirings()
        {
            // Arrange
            var entities = new List<HiringEntity>
            {
                new HiringEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Hiring 1",
                    ProposalId = Guid.NewGuid(),
                    HiringDate = DateTime.UtcNow,
                    IsApproved = false,
                    CreatedDate = DateTime.UtcNow
                },
                new HiringEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Hiring 2",
                    ProposalId = Guid.NewGuid(),
                    HiringDate = DateTime.UtcNow,
                    IsApproved = true,
                    CreatedDate = DateTime.UtcNow
                }
            };

            // Act
            var results = new List<HiringEntity>();
            foreach (var entity in entities)
            {
                var result = await _repository.PostCreateHiringAsync(entity);
                results.Add(result);
            }

            // Assert
            results.Should().HaveCount(2);
            results.Should().OnlyContain(r => r != null);
            results[0].Name.Should().Be("Hiring 1");
            results[1].Name.Should().Be("Hiring 2");
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleHiringWithNullName()
        {
            // Arrange
            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = null!,
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _repository.PostCreateHiringAsync(entity));
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleHiringWithEmptyName()
        {
            // Arrange
            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var result = await _repository.PostCreateHiringAsync(entity);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(string.Empty);
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleHiringWithApprovedStatus()
        {
            // Arrange
            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = "Approved Hiring",
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow,
                IsApproved = true,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var result = await _repository.PostCreateHiringAsync(entity);

            // Assert
            result.Should().NotBeNull();
            result.IsApproved.Should().BeTrue();
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleHiringWithRejectedStatus()
        {
            // Arrange
            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = "Rejected Hiring",
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var result = await _repository.PostCreateHiringAsync(entity);

            // Assert
            result.Should().NotBeNull();
            result.IsApproved.Should().BeFalse();
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleHiringWithFutureDate()
        {
            // Arrange
            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = "Future Hiring",
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow.AddDays(30),
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var result = await _repository.PostCreateHiringAsync(entity);

            // Assert
            result.Should().NotBeNull();
            result.HiringDate.Should().Be(entity.HiringDate);
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleHiringWithPastDate()
        {
            // Arrange
            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = "Past Hiring",
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow.AddDays(-30),
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var result = await _repository.PostCreateHiringAsync(entity);

            // Assert
            result.Should().NotBeNull();
            result.HiringDate.Should().Be(entity.HiringDate);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
