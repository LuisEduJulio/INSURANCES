using AutoMapper;
using INSURANCES.CORE.Dtos;
using INSURANCES.CORE.Entities;
using INSURANCES.CORE.Enum;
using INSURANCES.CORE.ModelView;
using INSURANCES.CORE.Ports.Respository;
using INSURANCES.CORE.Ports.Services;

namespace INSURANCES.APPLICATION.Services
{
    public class ProposalService(IProposalRespository proposalRespository, IMapper mapper)
        : IProposalService
    {
        private readonly IProposalRespository _proposalRespository = proposalRespository;
        private readonly IMapper _mapper = mapper;
        public async Task<ProposalModelView> GetProposalByIdAsync(Guid id)
        {
            var proposalEntity = await _proposalRespository.GetByIdProposalAsync(id);

            if (proposalEntity == null)
                throw new KeyNotFoundException($"Proposal with Id: {id} not found.");
            else
                return _mapper.Map<ProposalModelView>(proposalEntity);
        }
        public async Task<ProposalListModelView> GetProposalListAsync(GetProposalListDto proposalListDto)
        {
            var proposalListModelView = new ProposalListModelView();

            var proposalEntities = await _proposalRespository
                .GetListProposalAsync(new PaginationEntity(proposalListDto.Page, proposalListDto.Count));

            if (proposalEntities.Count == 0)
                return proposalListModelView;
            else
            {
                foreach (var proposalEntity in proposalEntities)
                    proposalListModelView
                        .Proposals
                        .Add(_mapper.Map<ProposalModelView>(proposalEntity));

                return proposalListModelView;
            }
        }
        public async Task<ProposalModelView> PostProposalByIdAsync(PostProposalDto postProposalDto)
        {
            var proposalEntity = await _proposalRespository.PostCreateProposalAsync(new ProposalEntity()
            {
                CreatedDate = DateTime.UtcNow,
                IsDisabled = false,
                Name = postProposalDto.Name,
                Proposal = postProposalDto.Proposal,
                ProposalStatus = ProposalStatusEnum.ANALYSIS
            });

            return _mapper.Map<ProposalModelView>(proposalEntity);
        }
        public async Task<ProposalModelView> PutProposalUpdateStatusByIdAsync(PutProposalUpdateStatusByIdDto putProposalUpdateStatusByIdDto)
        {
            var proposalEntity = await _proposalRespository.GetByIdProposalAsync(putProposalUpdateStatusByIdDto.Id);

            if (proposalEntity == null)
                throw new KeyNotFoundException($"Proposal with Id: {putProposalUpdateStatusByIdDto.Id} not found.");
            else
            {
                proposalEntity.ProposalStatus = putProposalUpdateStatusByIdDto.ProposalStatus;

                await _proposalRespository.PutAlterStatusByIdProposalAsync(proposalEntity);

                return _mapper.Map<ProposalModelView>(proposalEntity);
            }
        }
    }
}