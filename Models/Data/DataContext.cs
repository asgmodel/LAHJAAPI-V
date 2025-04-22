using AutoGenerator;
using AutoGenerator.Data;
using LAHJAAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LAHJAAPI.Data;

public class DataContext : AutoIdentityDataContext<ApplicationUser, IdentityRole, string>, ITAutoDbContext

{
    //public override DatabaseFacade? Database { get;}
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<AuthorizationSession> AuthorizationSessions { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<UserService> UserServices { get; set; }
    public DbSet<UserModelAi> UserModelAis { get; set; }

    public DbSet<ModelGateway> ModelGateways { get; set; }
    public DbSet<ModelAi> ModelAis { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceMethod> ServiceMethods { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<EventRequest> EventRequests { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<PlanServices> PlanServices { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<CategoryModel> Categories { get; set; }
    public DbSet<TypeModel> Types { get; set; }
    public DbSet<Dialect> Dialects { get; set; }
    public DbSet<Advertisement> Advertisements { get; set; }
    public DbSet<AdvertisementTab> AdvertisementTabs { get; set; }

    public DbSet<Setting> Settings { get; set; }
    public DbSet<Space> Spaces { get; set; }
    public DbSet<PlanFeature> PlanFeatures { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        modelBuilder.Entity<Request>().Navigation(e => e.Events).AutoInclude();

        //modelBuilder.Entity<AuthorizationSession>()
        // .Property(u => u.ServicesId)
        // .HasConversion(
        //     v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),  // تحويل القائمة إلى JSON عند الحفظ
        //     v => JsonSerializer.Deserialize<List<string>>(v, null as JsonSerializerOptions) // استرجاع القائمة من JSON عند القراءة
        // );
    }


}
