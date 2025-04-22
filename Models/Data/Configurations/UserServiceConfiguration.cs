using LAHJAAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.Data.Configurations
{
    internal sealed class UserServiceConfiguration : IEntityTypeConfiguration<UserService>
    {
        public void Configure(EntityTypeBuilder<UserService> builder)
        {
            builder.HasKey(sc => new { sc.UserId, sc.ServiceId });

            builder
                .HasOne(s => s.User)
                .WithMany(c => c.UserServices)
                .HasForeignKey(c => c.UserId);

            builder
                .HasOne(s => s.Service)
                .WithMany(c => c.UserServices)
                .HasForeignKey(c => c.ServiceId);

            builder.Navigation(e => e.Service).AutoInclude();
        }
    }
}
