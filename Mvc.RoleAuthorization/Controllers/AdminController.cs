using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mvc.RoleAuthorization.Models;
using Mvc.RoleAuthorization.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.RoleAuthorization.Controllers
{
	[Authorize("Authorization")]
	public class AdminController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IDataAccessService _dataAccessService;
		private readonly ILogger<AdminController> _logger;

		public AdminController(
				UserManager<IdentityUser> userManager,
				RoleManager<IdentityRole> roleManager,
				IDataAccessService dataAccessService,
				ILogger<AdminController> logger)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_dataAccessService = dataAccessService;
			_logger = logger;
		}

		public async Task<IActionResult> Roles()
		{
			var roleViewModel = new List<RoleViewModel>();

			try
			{
				var roles = await _roleManager.Roles.ToListAsync();
				foreach (var item in roles)
				{
					roleViewModel.Add(new RoleViewModel()
					{
						Id = item.Id,
						Name = item.Name,
					});
				}
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, ex.GetBaseException().Message);
			}

			return View(roleViewModel);
		}

		public IActionResult CreateRole()
		{
			return View(new RoleViewModel());
		}

		[HttpPost]
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

		public async Task<IActionResult> Users()
		{
			var userViewModel = new List<UserViewModel>();

			try
			{
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
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, ex.GetBaseException().Message);
			}

			return View(userViewModel);
		}

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

		[HttpPost]
		public async Task<IActionResult> EditUser(UserViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(viewModel.Id);
				var userRoles = await _userManager.GetRolesAsync(user);

				await _userManager.RemoveFromRolesAsync(user, userRoles);
				await _userManager.AddToRolesAsync(user, viewModel.Roles.Where(x => x.Selected).Select(x => x.Name));

				return RedirectToAction(nameof(Users));
			}

			return View(viewModel);
		}

		public async Task<IActionResult> EditRolePermission(string id)
		{
			var permissions = new List<NavigationMenuViewModel>();
			if (!string.IsNullOrWhiteSpace(id))
			{
				permissions = await _dataAccessService.GetPermissionsByRoleIdAsync(id);
			}

			return View(permissions);
		}

		[HttpPost]
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