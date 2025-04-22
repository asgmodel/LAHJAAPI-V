using LAHJAAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.Data.Configurations
{
    internal sealed class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder
            .HasOne(s => s.User)
            .WithOne(u => u.Subscription)
            .HasForeignKey<Subscription>(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);

            builder
           .HasMany(u => u.Requests)
           .WithOne(u => u.Subscription)
           .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
