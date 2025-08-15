using INSURANCES.CORE.Dtos;
using INSURANCES.CORE.ModelView;

namespace INSURANCES.CORE.Ports.Services
{
    public interface IProposalService
    {
        Task<ProposalListModelView> GetProposalListAsync(GetProposalListDto proposalListDto);
        Task<ProposalModelView> GetProposalByIdAsync(Guid id);
        Task<ProposalModelView> PostProposalByIdAsync(PostProposalDto postProposalDto);
        Task<ProposalModelView> PutProposalUpdateStatusByIdAsync(PutProposalUpdateStatusByIdDto putProposalUpdateStatusByIdDto);
    }
}