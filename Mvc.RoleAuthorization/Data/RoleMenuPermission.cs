using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mvc.RoleAuthorization.Data
{
	[Table(name: "AspNetRoleMenuPermission")]
	public class RoleMenuPermission
	{
		public string RoleId { get; set; }

		public Guid NavigationMenuId { get; set; }

		public NavigationMenu NavigationMenu { get; set; }
	}
}