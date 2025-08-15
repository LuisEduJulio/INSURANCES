using AutoMapper;
using INSURANCES.CORE.Dtos;
using INSURANCES.CORE.Entities;
using INSURANCES.CORE.Enum;
using INSURANCES.CORE.ModelView;
using INSURANCES.CORE.Ports.Respository;
using INSURANCES.CORE.Ports.Services;

namespace INSURANCES.APPLICATION.Services
{
    public class HiringService(IHiringRepository hiringRepository,
        IProposalRespository proposalRespository,
        IMapper mapper)
        : IHiringService
    {
        private readonly IHiringRepository _hiringRepository = hiringRepository;
        private readonly IProposalRespository _proposalRespository = proposalRespository;
        private readonly IMapper _mapper = mapper;
        public async Task<HiringModelView> GetHiringByIdAsync(Guid id)
        {
            var hiringEntity = await _hiringRepository.GetHiringByIdAsync(id);

            if (hiringEntity == null)
                throw new KeyNotFoundException($"Hiring with Id: {id} not found.");
            else
                return _mapper.Map<HiringModelView>(hiringEntity);
        }
        public async Task<HiringModelView> PostCreateHiringAsync(PostHiringDto postHiringDto)
        {
            var proposalEntity = await _proposalRespository.GetByIdProposalAsync(postHiringDto.ProposalId);

            if (proposalEntity == null)
                throw new KeyNotFoundException(
                    $"Proposal with Id: {postHiringDto.ProposalId} not found."
                );

            if (proposalEntity.ProposalStatus != ProposalStatusEnum.APPROVED)
                throw new InvalidOperationException(
                    $"Proposal with Id: {postHiringDto.ProposalId} is not already approved."
                );

            if (proposalEntity.Hiring != null)
                throw new InvalidOperationException(
                    $"Proposal with Id: {postHiringDto.ProposalId} is being used by another contract."
                );

            var hiringEntity = new HiringEntity
            {
                CreatedDate = DateTime.UtcNow,
                ProposalId = postHiringDto.ProposalId,
                HiringDate = DateTime.UtcNow,
                IsApproved = postHiringDto.IsApproved,
                Name = postHiringDto.Name
            };

            await _hiringRepository.PostCreateHiringAsync(hiringEntity);

            return _mapper.Map<HiringModelView>(hiringEntity);
        }
    }
}