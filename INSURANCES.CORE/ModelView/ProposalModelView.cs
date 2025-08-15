using INSURANCES.CORE.Enum;

namespace INSURANCES.CORE.ModelView
{
    public class ProposalModelView
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Proposal { get; set; }
        public Guid? HiringId { get; set; }
        public DateTime? HiringDate { get; set; }
        public bool IsDisabled { get; set; }
        public ProposalStatusEnum ProposalStatus { get; set; }
    }
}