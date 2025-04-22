using LAHJAAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.Data.Configurations
{
    internal sealed class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder
                .HasMany(s => s.ServiceMethods)
                .WithOne(c => c.Service)
                .HasForeignKey(c => c.ServiceId);

            builder
           .HasMany(u => u.Requests)
           .WithOne(u => u.Service)
           .IsRequired();
        }
    }
}
