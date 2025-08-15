using INSURANCES.CORE.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace INSURANCES.DATA.Configurations
{
    public class HiringConfiguration : IEntityTypeConfiguration<HiringEntity>
    {
        public void Configure(EntityTypeBuilder<HiringEntity> builder)
        {
            builder.ToTable("Hiring");

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

            builder.Property(p => p.ProposalId)
                .IsRequired();

            builder.Property(p => p.HiringDate)
                .IsRequired();

            builder.Property(p => p.IsApproved)
                .IsRequired();
        }
    }
}