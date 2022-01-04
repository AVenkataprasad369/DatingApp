using Microsoft.EntityFrameworkCore;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API.Data
{
    //// Commented after implementing the Identity
    // public class DataContext : DbContext
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
         IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
         IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
       public DataContext(DbContextOptions options) : base(options)
       {

       }

    //// Commented after implementing the Identity
    //    public DbSet<AppUser> Users { get; set; }
       public DbSet<UserLike> Likes { get; set; }
       public DbSet<Message> Messages { get; set; }
       public DbSet<Group> Groups { get; set; }
       public DbSet<Connection> Connections { get; set; }

       protected override void OnModelCreating(ModelBuilder builder)
       {
           base.OnModelCreating(builder);

           builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

           builder.Entity<UserLike>()
           .HasKey(k => new {k.SourceUserId, k.LikedUserId});

           //Important: If you are using SQL Server then you need to set the DeleteBehaviour here 
           //to DeleteBehaviour.NoAction Or you will get an error during migration
           builder.Entity<UserLike>()
           .HasOne(s => s.SourceUser)
           .WithMany(l => l.LikedUsers)
           .HasForeignKey(s => s.SourceUserId)
           .OnDelete(DeleteBehavior.Cascade); 

           //Important: If you are using SQL Server then you need to set the DeleteBehaviour here 
           //to DeleteBehaviour.NoAction Or you will get an error during migration
           builder.Entity<UserLike>()
           .HasOne(s => s.LikedUser)
           .WithMany(l => l.LikedByUsers)
           .HasForeignKey(s => s.LikedUserId)
           .OnDelete(DeleteBehavior.Cascade);

           builder.Entity<Message>()
           .HasOne(u => u.Recipient)
           .WithMany(m => m.MessagesReceived)
           .OnDelete(DeleteBehavior.Restrict);

           builder.Entity<Message>()
           .HasOne(u => u.Sender)
           .WithMany(m => m.MessagesSent)
           .OnDelete(DeleteBehavior.Restrict);

       }
    }
}