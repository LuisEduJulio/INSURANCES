using INSURANCES.CORE.Enum;

namespace INSURANCES.CORE.Entities
{
    public class ProposalEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Proposal { get; set; }
        public HiringEntity? Hiring { get; set; }
        public bool IsDisabled { get; set; }
        public ProposalStatusEnum ProposalStatus { get; set; }
    }
}