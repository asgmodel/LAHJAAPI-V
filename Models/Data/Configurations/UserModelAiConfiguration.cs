using LAHJAAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.Data.Configurations
{
    internal sealed class UserModelAiConfiguration : IEntityTypeConfiguration<UserModelAi>
    {
        public void Configure(EntityTypeBuilder<UserModelAi> builder)
        {
            builder.HasKey(sc => new { sc.UserId, sc.ModelAiId });

            builder
                .HasOne(s => s.User)
                .WithMany(c => c.UserModelAis)
                .HasForeignKey(c => c.UserId);

            builder
                .HasOne(s => s.ModelAi)
                .WithMany(c => c.UserModelAis)
                .HasForeignKey(c => c.ModelAiId);

            builder.Navigation(e => e.ModelAi).AutoInclude();
        }
    }
}
