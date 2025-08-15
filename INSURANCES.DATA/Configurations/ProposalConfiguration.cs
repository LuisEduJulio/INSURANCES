using INSURANCES.CORE.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace INSURANCES.DATA.Configurations
{
    public class ProposalConfiguration : IEntityTypeConfiguration<ProposalEntity>
    {
        public void Configure(EntityTypeBuilder<ProposalEntity> builder)
        {
            builder.ToTable("Proposal");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(p => p.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.Property(p => p.UpdatedDate)
                .IsRequired(false);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Proposal)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.IsDisabled)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.ProposalStatus)
                .HasConversion<int>()
                .IsRequired();

            builder.HasOne(p => p.Hiring)
                .WithOne()
                .HasForeignKey<HiringEntity>(h => h.ProposalId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}