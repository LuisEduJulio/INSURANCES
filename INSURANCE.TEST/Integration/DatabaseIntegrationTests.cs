using FluentAssertions;
using INSURANCES.CORE.Entities;
using INSURANCES.CORE.Enum;
using INSURANCES.CORE.Ports.Respository;
using INSURANCES.DATA.Factory;
using INSURANCES.DATA.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace INSURANCE.TEST.Integration
{
    public class DatabaseIntegrationTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IHiringRepository _hiringRepository;
        private readonly IProposalRespository _proposalRepository;

        public DatabaseIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _hiringRepository = new HiringRepository(_context);
            _proposalRepository = new ProposalRespository(_context);
        }

        [Fact]
        public async Task HiringRepository_ShouldCreateAndRetrieveHiring()
        {
            // Arrange
            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = "Integration Test Hiring",
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var created = await _hiringRepository.PostCreateHiringAsync(entity);
            var retrieved = await _hiringRepository.GetHiringByIdAsync(created.Id);

            // Assert
            created.Should().NotBeNull();
            retrieved.Should().NotBeNull();
            retrieved!.Id.Should().Be(created.Id);
            retrieved.Name.Should().Be("Integration Test Hiring");
        }

        [Fact]
        public async Task HiringRepository_ShouldUpdateHiring()
        {
            // Arrange
            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = "Update Test Hiring",
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            var created = await _hiringRepository.PostCreateHiringAsync(entity);

            // Act
            created.IsApproved = true;
            created.UpdatedDate = DateTime.UtcNow;

            _context.HiringEntities.Update(created);
            await _context.SaveChangesAsync();

            var updated = await _hiringRepository.GetHiringByIdAsync(created.Id);

            // Assert
            updated.Should().NotBeNull();
            updated!.IsApproved.Should().BeTrue();
        }

        [Fact]
        public async Task HiringRepository_ShouldDeleteHiring()
        {
            // Arrange
            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = "Delete Test Hiring",
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            var created = await _hiringRepository.PostCreateHiringAsync(entity);

            // Act
            _context.HiringEntities.Remove(created);
            await _context.SaveChangesAsync();

            var deleted = await _hiringRepository.GetHiringByIdAsync(created.Id);

            // Assert
            deleted.Should().BeNull();
        }

        [Fact]
        public async Task ProposalRepository_ShouldCreateAndRetrieveProposal()
        {
            // Arrange
            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = "Integration Test Proposal",
                Proposal = 1500.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var created = await _proposalRepository.PostCreateProposalAsync(entity);
            var retrieved = await _proposalRepository.GetByIdProposalAsync(created.Id);

            // Assert
            created.Should().NotBeNull();
            retrieved.Should().NotBeNull();
            retrieved!.Id.Should().Be(created.Id);
            retrieved.Name.Should().Be("Integration Test Proposal");
        }

        [Fact]
        public async Task ProposalRepository_ShouldUpdateProposalStatus()
        {
            // Arrange
            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = "Status Update Test Proposal",
                Proposal = 2000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var created = await _proposalRepository.PostCreateProposalAsync(entity);

            // Act
            created.ProposalStatus = ProposalStatusEnum.APPROVED;
            var updated = await _proposalRepository.PutAlterStatusByIdProposalAsync(created);

            // Assert
            updated.Should().NotBeNull();
            updated.ProposalStatus.Should().Be(ProposalStatusEnum.APPROVED);
        }

        [Fact]
        public async Task ProposalRepository_ShouldGetProposalsByStatus()
        {
            // Arrange
            var entities = new List<ProposalEntity>
            {
                new ProposalEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Analysis Proposal 1",
                    Proposal = 1000.00m,
                    ProposalStatus = ProposalStatusEnum.ANALYSIS,
                    IsDisabled = false,
                    CreatedDate = DateTime.UtcNow
                },
                new ProposalEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Approved Proposal 1",
                    Proposal = 2000.00m,
                    ProposalStatus = ProposalStatusEnum.APPROVED,
                    IsDisabled = false,
                    CreatedDate = DateTime.UtcNow
                },
                new ProposalEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Analysis Proposal 2",
                    Proposal = 3000.00m,
                    ProposalStatus = ProposalStatusEnum.ANALYSIS,
                    IsDisabled = false,
                    CreatedDate = DateTime.UtcNow
                }
            };

            foreach (var entity in entities)
            {
                await _proposalRepository.PostCreateProposalAsync(entity);
            }

            var pagination = new PaginationEntity(1, 10);

            // Act
            var allProposals = await _proposalRepository.GetListProposalAsync(pagination);
            var analysisProposals = allProposals.Where(p => p.ProposalStatus == ProposalStatusEnum.ANALYSIS).ToList();

            // Assert
            allProposals.Should().HaveCount(3);
            analysisProposals.Should().HaveCount(2);
        }

        [Fact]
        public async Task Database_ShouldHandleConcurrentOperations()
        {
            // Arrange
            var proposal = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = "Concurrent Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var hiring = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = "Concurrent Test Hiring",
                ProposalId = proposal.Id,
                HiringDate = DateTime.UtcNow,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var createdProposal = await _proposalRepository.PostCreateProposalAsync(proposal);
            var createdHiring = await _hiringRepository.PostCreateHiringAsync(hiring);

            // Assert
            createdProposal.Should().NotBeNull();
            createdHiring.Should().NotBeNull();
        }

        [Fact]
        public async Task Database_ShouldMaintainDataIntegrity()
        {
            // Arrange
            var proposal = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = "Integrity Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var hiring = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = "Integrity Test Hiring",
                ProposalId = proposal.Id,
                HiringDate = DateTime.UtcNow,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            // Act
            var createdProposal = await _proposalRepository.PostCreateProposalAsync(proposal);
            var createdHiring = await _hiringRepository.PostCreateHiringAsync(hiring);

            // Assert
            createdProposal.Should().NotBeNull();
            createdHiring.Should().NotBeNull();
            createdHiring.ProposalId.Should().Be(createdProposal.Id);
        }

        [Fact]
        public async Task HiringRepository_ShouldHandleMultipleHirings()
        {
            // Arrange
            var entities = new List<HiringEntity>
            {
                new HiringEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Multiple Hiring 1",
                    ProposalId = Guid.NewGuid(),
                    HiringDate = DateTime.UtcNow,
                    IsApproved = false,
                    CreatedDate = DateTime.UtcNow
                },
                new HiringEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Multiple Hiring 2",
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
                var result = await _hiringRepository.PostCreateHiringAsync(entity);
                results.Add(result);
            }

            // Assert
            results.Should().HaveCount(2);
            results.Should().OnlyContain(r => r != null);
        }

        [Fact]
        public async Task HiringRepository_ShouldHandleHiringWithNullName()
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
            await Assert.ThrowsAsync<DbUpdateException>(() => _hiringRepository.PostCreateHiringAsync(entity));
        }

        [Fact]
        public async Task ProposalRepository_ShouldGetProposalsList()
        {
            // Arrange
            var entities = new List<ProposalEntity>
            {
                new ProposalEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "List Proposal 1",
                    Proposal = 1000.00m,
                    ProposalStatus = ProposalStatusEnum.ANALYSIS,
                    IsDisabled = false,
                    CreatedDate = DateTime.UtcNow
                },
                new ProposalEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "List Proposal 2",
                    Proposal = 2000.00m,
                    ProposalStatus = ProposalStatusEnum.APPROVED,
                    IsDisabled = false,
                    CreatedDate = DateTime.UtcNow
                }
            };

            foreach (var entity in entities)
            {
                await _proposalRepository.PostCreateProposalAsync(entity);
            }

            var pagination = new PaginationEntity(1, 5);

            // Act
            var result = await _proposalRepository.GetListProposalAsync(pagination);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Database_ShouldHandleProposalStatusChanges()
        {
            // Arrange
            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = "Status Change Test",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var created = await _proposalRepository.PostCreateProposalAsync(entity);

            // Act
            created.ProposalStatus = ProposalStatusEnum.REJECTED;
            var updated = await _proposalRepository.PutAlterStatusByIdProposalAsync(created);

            // Assert
            updated.Should().NotBeNull();
            updated.ProposalStatus.Should().Be(ProposalStatusEnum.REJECTED);
        }

        [Fact]
        public async Task Database_ShouldHandlePaginationCorrectly()
        {
            // Arrange
            var entities = new List<ProposalEntity>();
            for (int i = 1; i <= 25; i++)
            {
                entities.Add(new ProposalEntity
                {
                    Id = Guid.NewGuid(),
                    Name = $"Pagination Test Proposal {i}",
                    Proposal = 1000.00m * i,
                    ProposalStatus = ProposalStatusEnum.ANALYSIS,
                    IsDisabled = false,
                    CreatedDate = DateTime.UtcNow
                });
            }

            foreach (var entity in entities)
            {
                await _proposalRepository.PostCreateProposalAsync(entity);
            }

            // Act
            var page1 = await _proposalRepository.GetListProposalAsync(new PaginationEntity(1, 10));
            var page2 = await _proposalRepository.GetListProposalAsync(new PaginationEntity(2, 10));
            var page3 = await _proposalRepository.GetListProposalAsync(new PaginationEntity(3, 10));

            // Assert
            page1.Should().HaveCount(10);
            page2.Should().HaveCount(10);
            page3.Should().HaveCount(5);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
