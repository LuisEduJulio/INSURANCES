namespace INSURANCES.CORE.Entities
{
    public class HiringEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProposalId { get; set; }
        public DateTime HiringDate { get; set; }
        public bool IsApproved { get; set; }
    }
}