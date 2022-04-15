using Microsoft.AspNetCore.Identity;
using Mvc.RoleAuthorization.Data;

namespace Mvc.RoleAuthorization
{
	public static class DbInitializer
	{
		public static void Initialize(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
				if (context != null)
				{
					context.Database.EnsureCreated();

					var _userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
					var _roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

					if (!context.Users.Any(usr => usr.UserName == "admin@test.com"))
					{
						var user = new IdentityUser()
						{
							UserName = "admin@test.com",
							Email = "admin@test.com",
							EmailConfirmed = true,
						};

						var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
					}

					if (!context.Users.Any(usr => usr.UserName == "manager@test.com"))
					{
						var user = new IdentityUser()
						{
							UserName = "manager@test.com",
							Email = "manager@test.com",
							EmailConfirmed = true,
						};

						var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
					}

					if (!context.Users.Any(usr => usr.UserName == "employee@test.com"))
					{
						var user = new IdentityUser()
						{
							UserName = "employee@test.com",
							Email = "employee@test.com",
							EmailConfirmed = true,
						};

						var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
					}

					if (!_roleManager.RoleExistsAsync("Admin").Result)
					{
						var role = _roleManager.CreateAsync(new IdentityRole { Name = "Admin" }).Result;
					}

					if (!_roleManager.RoleExistsAsync("Manager").Result)
					{
						var role = _roleManager.CreateAsync(new IdentityRole { Name = "Manager" }).Result;
					}

					if (!_roleManager.RoleExistsAsync("Employee").Result)
					{
						var role = _roleManager.CreateAsync(new IdentityRole { Name = "Employee" }).Result;
					}

					var adminUser = _userManager.FindByNameAsync("admin@test.com").Result;
					var adminRole = _userManager.AddToRolesAsync(adminUser, new string[] { "Admin" }).Result;

					var managerUser = _userManager.FindByNameAsync("manager@test.com").Result;
					var managerRole = _userManager.AddToRolesAsync(managerUser, new string[] { "Manager" }).Result;

					var employeeUser = _userManager.FindByNameAsync("employee@test.com").Result;
					var userRole = _userManager.AddToRolesAsync(employeeUser, new string[] { "Employee" }).Result;

					var permissions = GetPermissions();
					foreach (var item in permissions)
					{
						if (context?.NavigationMenu?.Any(n => n.Name == item.Name) == false)
						{
							context.NavigationMenu.Add(item);
						}
					}

					var _adminRole = _roleManager.Roles.Where(x => x.Name == "Admin").FirstOrDefault();
					var _managerRole = _roleManager.Roles.Where(x => x.Name == "Manager").FirstOrDefault();
					var _employeeRole = _roleManager.Roles.Where(x => x.Name == "Employee").FirstOrDefault();

					if (_adminRole != null)
					{
						if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0")))
						{
							context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0") });
						}

						if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c")))
						{
							context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c") });
						}

						if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c")))
						{
							context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c") });
						}

						if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("F704BDFD-D3EA-4A6F-9463-DA47ED3657AB")))
						{
							context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("F704BDFD-D3EA-4A6F-9463-DA47ED3657AB") });
						}

						if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5")))
						{
							context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5") });
						}

						if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734")))
						{
							context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734") });
						}

						if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("94C22F11-6DD2-4B9C-95F7-9DD4EA1002E6")))
						{
							context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("94C22F11-6DD2-4B9C-95F7-9DD4EA1002E6") });
						}
					}

					if (_managerRole != null)
					{
						if (!context.RoleMenuPermission.Any(x => x.RoleId == _managerRole.Id && x.NavigationMenuId == new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0")))
						{
							context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _managerRole.Id, NavigationMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0") });
						}

						if (!context.RoleMenuPermission.Any(x => x.RoleId == _managerRole.Id && x.NavigationMenuId == new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c")))
						{
							context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _managerRole.Id, NavigationMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c") });
						}
					}

					context.SaveChanges();
				}
			}
		}

		private static List<NavigationMenu> GetPermissions()
		{
			return new List<NavigationMenu>()
			{
				new NavigationMenu()
				{
					Id = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					Name = "Admin",
					ControllerName = "",
					ActionName = "",
					ParentMenuId = null,
					DisplayOrder=1,
					Visible = true,
				},
				new NavigationMenu()
				{
					Id = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
					Name = "Roles",
					ControllerName = "Admin",
					ActionName = "Roles",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=1,
					Visible = true,
				},
				new NavigationMenu()
				{
					Id = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c"),
					Name = "Users",
					ControllerName = "Admin",
					ActionName = "Users",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=3,
					Visible = true,
				},
				new NavigationMenu()
				{
					Id = new Guid("F704BDFD-D3EA-4A6F-9463-DA47ED3657AB"),
					Name = "External Google Link",
					ControllerName = "",
					ActionName = "",
					IsExternal = true,
					ExternalUrl = "https://www.google.com/",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=2,
					Visible = true,
				},
				new NavigationMenu()
				{
					Id = new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5"),
					Name = "Create Role",
					ControllerName = "Admin",
					ActionName = "CreateRole",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=3,
					Visible = true,
				},
				new NavigationMenu()
				{
					Id = new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734"),
					Name = "Edit User",
					ControllerName = "Admin",
					ActionName = "EditUser",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=3,
					Visible = false,
				},
				new NavigationMenu()
				{
					Id = new Guid("94C22F11-6DD2-4B9C-95F7-9DD4EA1002E6"),
					Name = "Edit Role Permission",
					ControllerName = "Admin",
					ActionName = "EditRolePermission",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=3,
					Visible = false,
				},
			};
		}
	}
}