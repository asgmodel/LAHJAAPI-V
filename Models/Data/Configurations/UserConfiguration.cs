using LAHJAAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.Data.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .HasMany(u => u.Requests)
                .WithOne(u => u.User)
                .IsRequired();
            // Each User can have many UserClaims
            //builder
            //    .HasMany(e => e.Claims)
            //    .WithOne(e => e.User)
            //    .HasForeignKey(uc => uc.UserId)
            //    .IsRequired();

            //// Each User can have many UserLogins
            //builder
            //    .HasMany(e => e.Logins)
            //    .WithOne(e => e.User)
            //    .HasForeignKey(ul => ul.UserId)
            //    .IsRequired();

            //// Each User can have many UserTokens
            //builder
            //    .HasMany(e => e.Tokens)
            //    .WithOne(e => e.User)
            //    .HasForeignKey(ut => ut.UserId)
            //    .IsRequired();

            //// Each User can have many entries in the UserRole join table
            //builder
            //    .HasMany(e => e.UserRoles)
            //    .WithOne(e => e.User)
            //    .HasForeignKey(ur => ur.UserId)
            //    .IsRequired();
        }
    }
}
