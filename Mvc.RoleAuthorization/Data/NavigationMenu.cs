using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mvc.RoleAuthorization.Data
{
	[Table(name: "AspNetNavigationMenu")]
	public class NavigationMenu
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string? Name { get; set; }

		[ForeignKey(nameof(ParentNavigationMenu))]
		public Guid? ParentMenuId { get; set; }

		public virtual NavigationMenu? ParentNavigationMenu { get; set; }

		public string? Area { get; set; }

		public string? ControllerName { get; set; }

		public string? ActionName { get; set; }
		
		public bool IsExternal { get; set; }

		public string? ExternalUrl { get; set; }

		public int DisplayOrder { get; set; }

		[NotMapped]
		public bool Permitted { get; set; }

		public bool Visible { get; set; }
	}
}