using LAHJAAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.Data.Configurations
{
    internal sealed class PlanServicesConfiguration : IEntityTypeConfiguration<PlanServices>
    {
        public void Configure(EntityTypeBuilder<PlanServices> builder)
        {
            builder.HasKey(sc => new { sc.PlanId, sc.ServiceId });

            builder
                .HasOne(s => s.Plan)
                .WithMany(c => c.PlanServices)
                .HasForeignKey(c => c.PlanId);

            builder
                .HasOne(s => s.Service)
                .WithMany(c => c.PlanServices)
                .HasForeignKey(c => c.ServiceId);

            builder.Navigation(e => e.Service).AutoInclude();


        }
    }
}
