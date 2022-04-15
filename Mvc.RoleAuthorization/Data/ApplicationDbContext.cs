using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mvc.RoleAuthorization.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<NavigationMenu> NavigationMenu { get; set; }
		public DbSet<RoleMenuPermission> RoleMenuPermission { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<RoleMenuPermission>().HasKey(c => new { c.RoleId, c.NavigationMenuId});

			foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
			{
				relationship.DeleteBehavior = DeleteBehavior.Restrict;
			}

			base.OnModelCreating(builder);
		}
	}
}