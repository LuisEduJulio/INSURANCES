namespace INSURANCES.CORE.ModelView
{
    public class HiringModelView
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProposalId { get; set; }
        public DateTime HiringDate { get; set; }
        public bool IsApproved { get; set; }
    }
}