using Jobs.Domain.Common;

namespace Jobs.API.Extensions
{
	public static class AuthorizationExtensions
	{
		public static void AddPermissionPolicies( this IServiceCollection services)
		{
			services.AddAuthorizationBuilder()
					.AddPolicy(Permissions.Jobs.Create, policy => policy.RequireClaim("Permission",Permissions.Jobs.Create))
					.AddPolicy(Permissions.Jobs.Delete, policy => policy.RequireClaim("Permission",Permissions.Jobs.Delete));
		}
	}
}