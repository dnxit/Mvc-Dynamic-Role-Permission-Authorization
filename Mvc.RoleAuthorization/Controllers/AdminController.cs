using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mvc.RoleAuthorization.Models;
using Mvc.RoleAuthorization.Services;

namespace Mvc.RoleAuthorization.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		private readonly ILogger<AdminController> _logger;
		private readonly IDataAccessService _dataAccessService;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AdminController(
				UserManager<IdentityUser> userManager,
				RoleManager<IdentityRole> roleManager,
				IDataAccessService dataAccessService,
				ILogger<AdminController> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
			_roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
			_dataAccessService = dataAccessService ?? throw new ArgumentNullException(nameof(dataAccessService));
		}

		[Authorize("Authorization")]
		public async Task<IActionResult> Roles()
		{
			var roleViewModel = new List<RoleViewModel>();
			var roles = await _roleManager.Roles.ToListAsync();
			foreach (var item in roles)
			{
				roleViewModel.Add(new RoleViewModel()
				{
					Id = item.Id,
					Name = item.Name,
				});
			}

			return View(roleViewModel);
		}

		[Authorize("Roles")]
		public IActionResult CreateRole()
		{
			return View(new RoleViewModel());
		}

		[HttpPost]
		[Authorize("Roles")]
		public async Task<IActionResult> CreateRole(RoleViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var result = await _roleManager.CreateAsync(new IdentityRole() { Name = viewModel.Name });
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Roles));
				}
				else
				{
					ModelState.AddModelError("Name", string.Join(",", result.Errors));
				}
			}

			return View(viewModel);
		}

		[Authorize("Authorization")]
		public async Task<IActionResult> Users()
		{
			var userViewModel = new List<UserViewModel>();
			var users = await _userManager.Users.ToListAsync();
			foreach (var item in users)
			{
				userViewModel.Add(new UserViewModel()
				{
					Id = item.Id,
					Email = item.Email,
					UserName = item.UserName,
				});
			}

			return View(userViewModel);
		}

		[Authorize("Users")]
		public async Task<IActionResult> EditUser(string id)
		{
			var viewModel = new UserViewModel();
			if (!string.IsNullOrWhiteSpace(id))
			{
				var user = await _userManager.FindByIdAsync(id);
				var userRoles = await _userManager.GetRolesAsync(user);

				viewModel.Email = user?.Email;
				viewModel.UserName = user?.UserName;

				var allRoles = await _roleManager.Roles.ToListAsync();
				viewModel.Roles = allRoles.Select(x => new RoleViewModel()
				{
					Id = x.Id,
					Name = x.Name,
					Selected = userRoles.Contains(x.Name)
				}).ToArray();

			}

			return View(viewModel);
		}

		[HttpPost, Authorize("Users")]
		public async Task<IActionResult> EditUser(UserViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(viewModel.Id);
				var userRoles = await _userManager.GetRolesAsync(user);

				await _userManager.RemoveFromRolesAsync(user, userRoles);
				await _userManager.AddToRolesAsync(user, viewModel.Roles?.Where(x => x.Selected).Select(x => x.Name));

				return RedirectToAction(nameof(Users));
			}

			return View(viewModel);
		}

		[Authorize("Authorization")]
		public async Task<IActionResult> EditRolePermission(string id)
		{
			var permissions = new List<NavigationMenuViewModel>();
			if (!string.IsNullOrWhiteSpace(id))
			{
				permissions = await _dataAccessService.GetPermissionsByRoleIdAsync(id);
			}

			return View(permissions);
		}

		[HttpPost, Authorize("Authorization")]
		public async Task<IActionResult> EditRolePermission(string id, List<NavigationMenuViewModel> viewModel)
		{
			if (ModelState.IsValid)
			{
				var permissionIds = viewModel.Where(x => x.Permitted).Select(x => x.Id);

				await _dataAccessService.SetPermissionsByRoleIdAsync(id, permissionIds);
				return RedirectToAction(nameof(Roles));
			}

			return View(viewModel);
		}
	}
}