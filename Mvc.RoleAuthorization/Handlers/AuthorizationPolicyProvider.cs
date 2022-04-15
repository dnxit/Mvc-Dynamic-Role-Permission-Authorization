using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Mvc.RoleAuthorization.Handlers
{
	public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
	{
		private readonly AuthorizationOptions _options;

		public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
		{
			_options = options.Value;
		}

		public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
		{
			return await base.GetPolicyAsync(policyName)
				   ?? new AuthorizationPolicyBuilder()
					   .AddRequirements(new AuthorizationRequirement(policyName))
					   .Build();
		}
	}
}