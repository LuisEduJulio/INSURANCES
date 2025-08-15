using AutoMapper;
using FluentAssertions;
using INSURANCES.APPLICATION.Services;
using INSURANCES.CORE.Dtos;
using INSURANCES.CORE.Entities;
using INSURANCES.CORE.Enum;
using INSURANCES.CORE.ModelView;
using INSURANCES.CORE.Ports.Respository;
using Moq;
using Xunit;

namespace INSURANCE.TEST.Services
{
    public class ProposalServiceTests
    {
        private readonly Mock<IProposalRespository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProposalService _service;

        public ProposalServiceTests()
        {
            _mockRepository = new Mock<IProposalRespository>();
            _mockMapper = new Mock<IMapper>();
            _service = new ProposalService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task PostProposalByIdAsync_ShouldCreateProposal_WhenValidData()
        {
            // Arrange
            var dto = new PostProposalDto
            {
                Name = "Test Proposal",
                Proposal = 1000.00m
            };

            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Proposal = dto.Proposal,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var modelView = new ProposalModelView
            {
                Id = entity.Id,
                Name = entity.Name,
                Proposal = entity.Proposal,
                ProposalStatus = entity.ProposalStatus,
                IsDisabled = entity.IsDisabled,
                CreatedDate = entity.CreatedDate
            };

            _mockRepository.Setup(r => r.PostCreateProposalAsync(It.IsAny<ProposalEntity>()))
                .ReturnsAsync(entity);

            _mockMapper.Setup(m => m.Map<ProposalModelView>(entity))
                .Returns(modelView);

            // Act
            var result = await _service.PostProposalByIdAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(entity.Id);
            result.Name.Should().Be(dto.Name);
            result.Proposal.Should().Be(dto.Proposal);
            result.ProposalStatus.Should().Be(ProposalStatusEnum.ANALYSIS);

            _mockRepository.Verify(r => r.PostCreateProposalAsync(It.IsAny<ProposalEntity>()), Times.Once);
        }

        [Fact]
        public async Task PostProposalByIdAsync_ShouldHandleNullDto()
        {
            // Arrange
            PostProposalDto dto = null!;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _service.PostProposalByIdAsync(dto));
        }

        [Fact]
        public async Task PostProposalByIdAsync_ShouldHandleEmptyName()
        {
            // Arrange
            var dto = new PostProposalDto
            {
                Name = string.Empty,
                Proposal = 500.00m
            };

            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Proposal = dto.Proposal,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var modelView = new ProposalModelView
            {
                Id = entity.Id,
                Name = entity.Name,
                Proposal = entity.Proposal,
                ProposalStatus = entity.ProposalStatus,
                IsDisabled = entity.IsDisabled,
                CreatedDate = entity.CreatedDate
            };

            _mockRepository.Setup(r => r.PostCreateProposalAsync(It.IsAny<ProposalEntity>()))
                .ReturnsAsync(entity);

            _mockMapper.Setup(m => m.Map<ProposalModelView>(entity))
                .Returns(modelView);

            // Act
            var result = await _service.PostProposalByIdAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(string.Empty);
        }

        [Fact]
        public async Task PostProposalByIdAsync_ShouldHandleZeroProposal()
        {
            // Arrange
            var dto = new PostProposalDto
            {
                Name = "Zero Proposal",
                Proposal = 0.00m
            };

            var entity = new ProposalEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Proposal = dto.Proposal,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var modelView = new ProposalModelView
            {
                Id = entity.Id,
                Name = entity.Name,
                Proposal = entity.Proposal,
                ProposalStatus = entity.ProposalStatus,
                IsDisabled = entity.IsDisabled,
                CreatedDate = entity.CreatedDate
            };

            _mockRepository.Setup(r => r.PostCreateProposalAsync(It.IsAny<ProposalEntity>()))
                .ReturnsAsync(entity);

            _mockMapper.Setup(m => m.Map<ProposalModelView>(entity))
                .Returns(modelView);

            // Act
            var result = await _service.PostProposalByIdAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Proposal.Should().Be(0.00m);
        }

        [Fact]
        public async Task GetProposalByIdAsync_ShouldReturnProposal_WhenValidId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new ProposalEntity
            {
                Id = id,
                Name = "Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var modelView = new ProposalModelView
            {
                Id = entity.Id,
                Name = entity.Name,
                Proposal = entity.Proposal,
                ProposalStatus = entity.ProposalStatus,
                IsDisabled = entity.IsDisabled,
                CreatedDate = entity.CreatedDate
            };

            _mockRepository.Setup(r => r.GetByIdProposalAsync(id))
                .ReturnsAsync(entity);

            _mockMapper.Setup(m => m.Map<ProposalModelView>(entity))
                .Returns(modelView);

            // Act
            var result = await _service.GetProposalByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Name.Should().Be("Test Proposal");
        }

        [Fact]
        public async Task GetProposalByIdAsync_ShouldThrowKeyNotFoundException_WhenProposalDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetByIdProposalAsync(id))
                .ReturnsAsync((ProposalEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetProposalByIdAsync(id));
        }

        [Fact]
        public async Task GetProposalByIdAsync_ShouldHandleEmptyGuid()
        {
            // Arrange
            var id = Guid.Empty;

            _mockRepository.Setup(r => r.GetByIdProposalAsync(id))
                .ReturnsAsync((ProposalEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetProposalByIdAsync(id));
        }        

        [Fact]
        public async Task PutProposalUpdateStatusByIdAsync_ShouldHandleNullDto()
        {
            // Arrange
            PutProposalUpdateStatusByIdDto dto = null!;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _service.PutProposalUpdateStatusByIdAsync(dto));
        }

       

        [Fact]
        public async Task GetProposalListAsync_ShouldReturnProposals_WhenValidData()
        {
            // Arrange
            var dto = new GetProposalListDto
            {
                Page = 1,
                Count = 10
            };

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

            var modelViews = entities.Select(entity => new ProposalModelView
            {
                Id = entity.Id,
                Name = entity.Name,
                Proposal = entity.Proposal,
                ProposalStatus = entity.ProposalStatus,
                IsDisabled = entity.IsDisabled,
                CreatedDate = entity.CreatedDate
            }).ToList();

            var expectedResult = new ProposalListModelView
            {
                Proposals = modelViews
            };

            _mockRepository.Setup(r => r.GetListProposalAsync(It.IsAny<PaginationEntity>()))
                .ReturnsAsync(entities);

            _mockMapper.Setup(m => m.Map<ProposalModelView>(It.IsAny<ProposalEntity>()))
                .Returns((ProposalEntity entity) => modelViews.First(mv => mv.Id == entity.Id));

            // Act
            var result = await _service.GetProposalListAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Proposals.Should().HaveCount(2);
            result.Proposals.Should().OnlyContain(p => p != null);
        }

        [Fact]
        public async Task GetProposalListAsync_ShouldHandleEmptyList()
        {
            // Arrange
            var dto = new GetProposalListDto
            {
                Page = 1,
                Count = 10
            };

            var entities = new List<ProposalEntity>();

            var expectedResult = new ProposalListModelView
            {
                Proposals = new List<ProposalModelView>()
            };

            _mockRepository.Setup(r => r.GetListProposalAsync(It.IsAny<PaginationEntity>()))
                .ReturnsAsync(entities);

            // Act
            var result = await _service.GetProposalListAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Proposals.Should().BeEmpty();
        }

        [Fact]
        public async Task PostProposalByIdAsync_ShouldHandleRepositoryException()
        {
            // Arrange
            var dto = new PostProposalDto
            {
                Name = "Test Proposal",
                Proposal = 1000.00m
            };

            _mockRepository.Setup(r => r.PostCreateProposalAsync(It.IsAny<ProposalEntity>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.PostProposalByIdAsync(dto));
        }

        [Fact]
        public async Task GetProposalByIdAsync_ShouldHandleRepositoryException()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetByIdProposalAsync(id))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetProposalByIdAsync(id));
        }

        [Fact]
        public async Task PutProposalUpdateStatusByIdAsync_ShouldHandleRepositoryException()
        {
            // Arrange
            var dto = new PutProposalUpdateStatusByIdDto
            {
                Id = Guid.NewGuid(),
                ProposalStatus = ProposalStatusEnum.APPROVED
            };

            var entity = new ProposalEntity
            {
                Id = dto.Id,
                Name = "Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.ANALYSIS,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            _mockRepository.Setup(r => r.GetByIdProposalAsync(dto.Id))
                .ReturnsAsync(entity);

            _mockRepository.Setup(r => r.PutAlterStatusByIdProposalAsync(It.IsAny<ProposalEntity>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.PutProposalUpdateStatusByIdAsync(dto));
        }

        [Fact]
        public async Task GetProposalListAsync_ShouldHandleRepositoryException()
        {
            // Arrange
            var dto = new GetProposalListDto
            {
                Page = 1,
                Count = 10
            };

            _mockRepository.Setup(r => r.GetListProposalAsync(It.IsAny<PaginationEntity>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetProposalListAsync(dto));
        }
    }
}
