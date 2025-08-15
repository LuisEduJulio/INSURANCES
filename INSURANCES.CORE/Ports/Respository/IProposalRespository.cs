using INSURANCES.CORE.Entities;

namespace INSURANCES.CORE.Ports.Respository
{
    public interface IProposalRespository
    {
        Task<ProposalEntity?> GetByIdProposalAsync(Guid id);
        Task<ProposalEntity> PostCreateProposalAsync(ProposalEntity proposalEntity);
        Task<IList<ProposalEntity>> GetListProposalAsync(PaginationEntity paginationEntity);
        Task<ProposalEntity> PutAlterStatusByIdProposalAsync(ProposalEntity proposalEntity);
    }
}