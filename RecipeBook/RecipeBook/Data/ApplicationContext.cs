using RecipeBook.Models;
using RecipeBook.Areas.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Security.Claims;

namespace RecipeBook.Data
{
    public class ApplicationContext : IdentityDbContext<AppUser, AppRole, int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationContext(DbContextOptions<ApplicationContext> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Component> Component { get; set; }
        public DbSet<Household> Household { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Step> Step { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Unit> Unit { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<AppUser>().HasOne(u => u.Household).WithMany(u => u.AppUsers);

            base.OnModelCreating(modelBuilder);

        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSavingAsync();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSavingAsync();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSavingAsync()
        {
            var entries = ChangeTracker.Entries();
            var now = DateTime.UtcNow;
            var user = GetCurrentUsernameAsync();
            foreach (var entry in entries)
            {
                if (entry.Entity is AppTable trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedDate = now;
                            trackable.UpdatedBy = user;
                            break;

                        case EntityState.Added:
                            trackable.CreatedDate = now;
                            trackable.CreatedBy = user;
                            trackable.UpdatedDate = now;
                            trackable.UpdatedBy = user;
                            break;
                    }
                }
            }
        }

        private AppUser GetCurrentUsernameAsync()
        {
            AppUser user = null;

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                int userId;
                var success = int.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
                if (success)
                {
                    user = AppUser.Find(userId);
                }
            }
            return user;
        }

    }
}
