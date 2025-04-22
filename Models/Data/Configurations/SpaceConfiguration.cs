using LAHJAAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.Data.Configurations
{
    internal sealed class SpaceConfiguration : IEntityTypeConfiguration<Space>
    {
        public void Configure(EntityTypeBuilder<Space> builder)
        {
            builder
           .HasMany(u => u.Requests)
           .WithOne(u => u.Space)
           .IsRequired(false);
        }
    }
}
