using Jobs.Application.Common.Helpers;

namespace Jobs.API.Extensions
{
	public static class AuthorizationExtensions
	{
		public static void AddPermissionPolicies(this IServiceCollection services)
		{
			services.AddAuthorization(options =>
			{
				foreach (var permission in PermissionsHelper.GetAllPermissions())
				{
					options.AddPolicy(permission!, policy => policy.RequireClaim("Permission", permission!));
				}
			});
		}
	}
}