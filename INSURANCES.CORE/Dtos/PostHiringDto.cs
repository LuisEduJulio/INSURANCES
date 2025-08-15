namespace INSURANCES.CORE.Dtos
{
    public class PostHiringDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProposalId { get; set; }
        public bool IsApproved { get; set; }
    }
}