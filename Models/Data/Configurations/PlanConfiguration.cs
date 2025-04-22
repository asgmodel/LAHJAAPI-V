using LAHJAAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.Data.Configurations
{
    internal sealed class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder
            .HasMany(u => u.Subscriptions)
            .WithOne(u => u.Plan)
            .HasForeignKey(s => s.PlanId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(e => e.PlanServices).AutoInclude();

        }
    }
}
