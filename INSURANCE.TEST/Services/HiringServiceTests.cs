using AutoMapper;
using FluentAssertions;
using INSURANCES.APPLICATION.Services;
using INSURANCES.CORE.Dtos;
using INSURANCES.CORE.Entities;
using INSURANCES.CORE.Enum;
using INSURANCES.CORE.ModelView;
using INSURANCES.CORE.Ports.Respository;
using INSURANCES.CORE.Ports.Services;
using Moq;
using Xunit;

namespace INSURANCE.TEST.Services
{
    public class HiringServiceTests
    {
        private readonly Mock<IHiringRepository> _mockRepository;
        private readonly Mock<IProposalRespository> _mockProposalRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly HiringService _service;

        public HiringServiceTests()
        {
            _mockRepository = new Mock<IHiringRepository>();
            _mockProposalRepository = new Mock<IProposalRespository>();
            _mockMapper = new Mock<IMapper>();
            _service = new HiringService(_mockRepository.Object, _mockProposalRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldCreateHiring_WhenValidData()
        {
            // Arrange
            var dto = new PostHiringDto
            {
                Name = "Test Hiring",
                ProposalId = Guid.NewGuid(),
                IsApproved = false
            };

            var proposalEntity = new ProposalEntity
            {
                Id = dto.ProposalId,
                Name = "Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.APPROVED,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ProposalId = dto.ProposalId,
                HiringDate = DateTime.UtcNow,
                IsApproved = dto.IsApproved,
                CreatedDate = DateTime.UtcNow
            };

            var modelView = new HiringModelView
            {
                Id = entity.Id,
                Name = entity.Name,
                ProposalId = entity.ProposalId,
                HiringDate = entity.HiringDate,
                IsApproved = entity.IsApproved,
                CreatedDate = entity.CreatedDate
            };

            _mockProposalRepository.Setup(r => r.GetByIdProposalAsync(dto.ProposalId))
                .ReturnsAsync(proposalEntity);

            _mockRepository.Setup(r => r.PostCreateHiringAsync(It.IsAny<HiringEntity>()))
                .ReturnsAsync(entity);

            _mockMapper.Setup(m => m.Map<HiringModelView>(It.IsAny<HiringEntity>()))
                .Returns(modelView);

            // Act
            var result = await _service.PostCreateHiringAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(entity.Id);
            result.Name.Should().Be(dto.Name);
            result.ProposalId.Should().Be(dto.ProposalId);
            result.IsApproved.Should().Be(dto.IsApproved);

            _mockRepository.Verify(r => r.PostCreateHiringAsync(It.IsAny<HiringEntity>()), Times.Once);
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleNullDto()
        {
            // Arrange
            PostHiringDto dto = null!;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _service.PostCreateHiringAsync(dto));
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleEmptyName()
        {
            // Arrange
            var dto = new PostHiringDto
            {
                Name = string.Empty,
                ProposalId = Guid.NewGuid(),
                IsApproved = false
            };

            var proposalEntity = new ProposalEntity
            {
                Id = dto.ProposalId,
                Name = "Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.APPROVED,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ProposalId = dto.ProposalId,
                HiringDate = DateTime.UtcNow,
                IsApproved = dto.IsApproved,
                CreatedDate = DateTime.UtcNow
            };

            var modelView = new HiringModelView
            {
                Id = entity.Id,
                Name = entity.Name,
                ProposalId = entity.ProposalId,
                HiringDate = entity.HiringDate,
                IsApproved = entity.IsApproved,
                CreatedDate = entity.CreatedDate
            };

            _mockProposalRepository.Setup(r => r.GetByIdProposalAsync(dto.ProposalId))
                .ReturnsAsync(proposalEntity);

            _mockRepository.Setup(r => r.PostCreateHiringAsync(It.IsAny<HiringEntity>()))
                .ReturnsAsync(entity);

            _mockMapper.Setup(m => m.Map<HiringModelView>(It.IsAny<HiringEntity>()))
                .Returns(modelView);

            // Act
            var result = await _service.PostCreateHiringAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(string.Empty);
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleApprovedHiring()
        {
            // Arrange
            var dto = new PostHiringDto
            {
                Name = "Approved Hiring",
                ProposalId = Guid.NewGuid(),
                IsApproved = true
            };

            var proposalEntity = new ProposalEntity
            {
                Id = dto.ProposalId,
                Name = "Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.APPROVED,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ProposalId = dto.ProposalId,
                HiringDate = DateTime.UtcNow,
                IsApproved = dto.IsApproved,
                CreatedDate = DateTime.UtcNow
            };

            var modelView = new HiringModelView
            {
                Id = entity.Id,
                Name = entity.Name,
                ProposalId = entity.ProposalId,
                HiringDate = entity.HiringDate,
                IsApproved = entity.IsApproved,
                CreatedDate = entity.CreatedDate
            };

            _mockProposalRepository.Setup(r => r.GetByIdProposalAsync(dto.ProposalId))
                .ReturnsAsync(proposalEntity);

            _mockRepository.Setup(r => r.PostCreateHiringAsync(It.IsAny<HiringEntity>()))
                .ReturnsAsync(entity);

            _mockMapper.Setup(m => m.Map<HiringModelView>(It.IsAny<HiringEntity>()))
                .Returns(modelView);

            // Act
            var result = await _service.PostCreateHiringAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.IsApproved.Should().BeTrue();
        }

        [Fact]
        public async Task GetHiringByIdAsync_ShouldReturnHiring_WhenValidId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new HiringEntity
            {
                Id = id,
                Name = "Test Hiring",
                ProposalId = Guid.NewGuid(),
                HiringDate = DateTime.UtcNow,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow
            };

            var modelView = new HiringModelView
            {
                Id = entity.Id,
                Name = entity.Name,
                ProposalId = entity.ProposalId,
                HiringDate = entity.HiringDate,
                IsApproved = entity.IsApproved,
                CreatedDate = entity.CreatedDate
            };

            _mockRepository.Setup(r => r.GetHiringByIdAsync(id))
                .ReturnsAsync(entity);

            _mockMapper.Setup(m => m.Map<HiringModelView>(entity))
                .Returns(modelView);

            // Act
            var result = await _service.GetHiringByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Name.Should().Be("Test Hiring");
        }

        [Fact]
        public async Task GetHiringByIdAsync_ShouldThrowKeyNotFoundException_WhenHiringDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetHiringByIdAsync(id))
                .ReturnsAsync((HiringEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetHiringByIdAsync(id));
        }

        [Fact]
        public async Task GetHiringByIdAsync_ShouldHandleEmptyGuid()
        {
            // Arrange
            var id = Guid.Empty;

            _mockRepository.Setup(r => r.GetHiringByIdAsync(id))
                .ReturnsAsync((HiringEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetHiringByIdAsync(id));
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleRepositoryException()
        {
            // Arrange
            var dto = new PostHiringDto
            {
                Name = "Test Hiring",
                ProposalId = Guid.NewGuid(),
                IsApproved = false
            };

            var proposalEntity = new ProposalEntity
            {
                Id = dto.ProposalId,
                Name = "Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.APPROVED,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            _mockProposalRepository.Setup(r => r.GetByIdProposalAsync(dto.ProposalId))
                .ReturnsAsync(proposalEntity);

            _mockRepository.Setup(r => r.PostCreateHiringAsync(It.IsAny<HiringEntity>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.PostCreateHiringAsync(dto));
        }

        [Fact]
        public async Task GetHiringByIdAsync_ShouldHandleRepositoryException()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetHiringByIdAsync(id))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetHiringByIdAsync(id));
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleMultipleHirings()
        {
            // Arrange
            var dto = new PostHiringDto
            {
                Name = "Multiple Hiring Test",
                ProposalId = Guid.NewGuid(),
                IsApproved = false
            };

            var proposalEntity = new ProposalEntity
            {
                Id = dto.ProposalId,
                Name = "Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.APPROVED,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ProposalId = dto.ProposalId,
                HiringDate = DateTime.UtcNow,
                IsApproved = dto.IsApproved,
                CreatedDate = DateTime.UtcNow
            };

            var modelView = new HiringModelView
            {
                Id = entity.Id,
                Name = entity.Name,
                ProposalId = entity.ProposalId,
                HiringDate = entity.HiringDate,
                IsApproved = entity.IsApproved,
                CreatedDate = entity.CreatedDate
            };

            _mockProposalRepository.Setup(r => r.GetByIdProposalAsync(dto.ProposalId))
                .ReturnsAsync(proposalEntity);

            _mockRepository.Setup(r => r.PostCreateHiringAsync(It.IsAny<HiringEntity>()))
                .ReturnsAsync(entity);

            _mockMapper.Setup(m => m.Map<HiringModelView>(It.IsAny<HiringEntity>()))
                .Returns(modelView);

            // Act
            var result = await _service.PostCreateHiringAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Multiple Hiring Test");
        }

        [Fact]
        public async Task PostCreateHiringAsync_ShouldHandleHiringWithNullName()
        {
            // Arrange
            var dto = new PostHiringDto
            {
                Name = null!,
                ProposalId = Guid.NewGuid(),
                IsApproved = false
            };

            var proposalEntity = new ProposalEntity
            {
                Id = dto.ProposalId,
                Name = "Test Proposal",
                Proposal = 1000.00m,
                ProposalStatus = ProposalStatusEnum.APPROVED,
                IsDisabled = false,
                CreatedDate = DateTime.UtcNow
            };

            var entity = new HiringEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ProposalId = dto.ProposalId,
                HiringDate = DateTime.UtcNow,
                IsApproved = dto.IsApproved,
                CreatedDate = DateTime.UtcNow
            };

            var modelView = new HiringModelView
            {
                Id = entity.Id,
                Name = entity.Name,
                ProposalId = entity.ProposalId,
                HiringDate = entity.HiringDate,
                IsApproved = entity.IsApproved,
                CreatedDate = entity.CreatedDate
            };

            _mockProposalRepository.Setup(r => r.GetByIdProposalAsync(dto.ProposalId))
                .ReturnsAsync(proposalEntity);

            _mockRepository.Setup(r => r.PostCreateHiringAsync(It.IsAny<HiringEntity>()))
                .ReturnsAsync(entity);

            _mockMapper.Setup(m => m.Map<HiringModelView>(It.IsAny<HiringEntity>()))
                .Returns(modelView);

            // Act
            var result = await _service.PostCreateHiringAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().BeNull();
        }
    }
}
