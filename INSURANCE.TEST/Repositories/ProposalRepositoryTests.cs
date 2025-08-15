using FluentAssertions;
using INSURANCES.CORE.Entities;
using INSURANCES.CORE.Enum;
using INSURANCES.CORE.Ports.Respository;
using INSURANCES.DATA.Factory;
using INSURANCES.DATA.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace INSURANCE.TEST.Repositories
{
    public class ProposalRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IProposalRespository _repository;

        public ProposalRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new ProposalRespository(_context);
        }

        [Fact]
        public async Task PostCreateProposalAsync_ShouldCreateProposal_WhenValidData()
        {
            // Arrange
            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var result = await _repository.PostCreateProposalAsync(entity);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(entity.Id);
            result.Name.Should().Be(entity.Name);
            result.Proposal.Should().Be(entity.Proposal);
            result.ProposalStatus.Should().Be(entity.ProposalStatus);

            var savedEntity = await _context.ProposalEntities.FindAsync(entity.Id);
            savedEntity.Should().NotBeNull();
            savedEntity!.Name.Should().Be(entity.Name);
        }

        [Fact]
        public async Task GetByIdProposalAsync_ShouldReturnProposal_WhenValidId()
        {
            // Arrange
            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            _context.ProposalEntities.Add(entity);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdProposalAsync(entity.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(entity.Id);
            result.Name.Should().Be(entity.Name);
        }

        [Fact]
        public async Task GetByIdProposalAsync_ShouldReturnNull_WhenProposalDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdProposalAsync(nonExistentId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetListProposalAsync_ShouldReturnProposals_WhenValidPagination()
        {
            // Arrange
            var entities = new List<ProposalEntity>
            {
                new ProposalEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Proposal 1",
                    Proposal = 1000.00m,
                    ProposalStatus = ProposalStatusEnum.ANALYSIS,
                    IsDisabled = false,
                    CreatedDate = DateTime.UtcNow
                },
                new ProposalEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Proposal 2",
                    Proposal = 2000.00m,
                    ProposalStatus = ProposalStatusEnum.APPROVED,
                    IsDisabled = false,
                    CreatedDate = DateTime.UtcNow
                }
            };

            _context.ProposalEntities.AddRange(entities);
            await _context.SaveChangesAsync();

            var pagination = new PaginationEntity(1, 10);

            // Act
            var result = await _repository.GetListProposalAsync(pagination);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task PutAlterStatusByIdProposalAsync_ShouldUpdateStatus_WhenValidData()
        {
            // Arrange
            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            _context.ProposalEntities.Add(entity);
            await _context.SaveChangesAsync();

            entity.ProposalStatus = ProposalStatusEnum.APPROVED;

            // Act
            var result = await _repository.PutAlterStatusByIdProposalAsync(entity);

            // Assert
            result.Should().NotBeNull();
            result.ProposalStatus.Should().Be(ProposalStatusEnum.APPROVED);

            var updatedEntity = await _context.ProposalEntities.FindAsync(entity.Id);
            updatedEntity.Should().NotBeNull();
            updatedEntity!.ProposalStatus.Should().Be(ProposalStatusEnum.APPROVED);
        }

        [Fact]
        public async Task PostCreateProposalAsync_ShouldGenerateNewId_WhenIdIsEmpty()
        {
            // Arrange
            var entity = new ProposalEntity
            {
                Id = Guid.Empty,
                Name = "Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var result = await _repository.PostCreateProposalAsync(entity);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task PostCreateProposalAsync_ShouldHandleMultipleProposals()
        {
            // Arrange
            var entities = new List<ProposalEntity>
            {
                new ProposalEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Proposal 1",
                    Proposal = 1000.00m,
                    ProposalStatus = ProposalStatusEnum.ANALYSIS,
                    IsDisabled = false,
                    CreatedDate = DateTime.UtcNow
                },
                new ProposalEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Proposal 2",
                    Proposal = 2000.00m,
                    ProposalStatus = ProposalStatusEnum.APPROVED,
                    IsDisabled = false,
                    CreatedDate = DateTime.UtcNow
                }
            };

            // Act
            var results = new List<ProposalEntity>();
            foreach (var entity in entities)
            {
                var result = await _repository.PostCreateProposalAsync(entity);
                results.Add(result);
            }

            // Assert
            results.Should().HaveCount(2);
            results.Should().OnlyContain(r => r != null);
            results[0].Name.Should().Be("Proposal 1");
            results[1].Name.Should().Be("Proposal 2");
        }

        [Fact]
        public async Task PostCreateProposalAsync_ShouldHandleProposalWithNullName()
        {
            // Arrange
            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = null!,
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _repository.PostCreateProposalAsync(entity));
        }

        [Fact]
        public async Task PostCreateProposalAsync_ShouldHandleProposalWithEmptyName()
        {
            // Arrange
            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var result = await _repository.PostCreateProposalAsync(entity);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(string.Empty);
        }

        [Fact]
        public async Task PutAlterStatusByIdProposalAsync_ShouldHandleNonExistentProposal()
        {
            // Arrange
            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = "Non-existent Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.APPROVED,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _repository.PutAlterStatusByIdProposalAsync(entity));
        }

        [Fact]
        public async Task PutAlterStatusByIdProposalAsync_ShouldHandleProposalWithNullName()
        {
            // Arrange
            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = null!,
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _repository.PostCreateProposalAsync(entity));
        }

        [Fact]
        public async Task GetListProposalAsync_ShouldHandleEmptyPagination()
        {
            // Arrange
            var pagination = new PaginationEntity(0, 0);

            // Act
            var result = await _repository.GetListProposalAsync(pagination);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetListProposalAsync_ShouldHandleNullPagination()
        {
            // Arrange
            PaginationEntity pagination = null!;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _repository.GetListProposalAsync(pagination));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
