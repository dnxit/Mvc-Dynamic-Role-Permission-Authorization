using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mvc.RoleAuthorization;
using Mvc.RoleAuthorization.Data;
using Mvc.RoleAuthorization.Handlers;
using Mvc.RoleAuthorization.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
	options.CheckConsentNeeded = context => true;
	options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
	options.SignIn.RequireConfirmedAccount = true;
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 6;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
})
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IDataAccessService, DataAccessService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

//when we use single policy it needs to register like this
//services.AddAuthorization(options =>
//{
//	options.AddPolicy("Authorization", policyCorrectUser =>
//	{
//		policyCorrectUser.Requirements.Add(new AuthorizationRequirement());
//	});
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	DbInitializer.Initialize(app);
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "areaRoute",
		pattern: "{area:exists}/{controller}/{action}",
		defaults: new { action = "Index" });

	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");

	endpoints.MapRazorPages();
});

app.Run();