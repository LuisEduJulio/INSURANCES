using INSURANCES.CORE.Enum;

namespace INSURANCES.CORE.Dtos
{
    public class PutProposalUpdateStatusByIdDto
    {
        public Guid Id { get; set; }
        public ProposalStatusEnum ProposalStatus { get; set; }
    }
}