//using Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Models.Data.Configurations
//{
//    internal sealed class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
//    {
//        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
//        {
//            // Each Role can have many entries in the UserRole join table
//            builder.HasMany(e => e.UserRoles)
//                .WithOne(e => e.Role)
//                .HasForeignKey(ur => ur.RoleId)
//                .IsRequired();

//            // Each Role can have many associated RoleClaims
//            builder.HasMany(e => e.RoleClaims)
//                .WithOne(e => e.Role)
//                .HasForeignKey(rc => rc.RoleId)
//            .IsRequired();

//            builder.Navigation(e => e.RoleClaims).AutoInclude();

//        }
//    }
//}
