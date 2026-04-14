using Jobs.Domain.Common;

namespace Jobs.Application.Common.Helpers
{
	public static class PermissionsHelper
	{
		public static List<string?> GetAllPermissions()
		{
			return typeof(Permissions)
				.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
				.Select(f => f.GetValue(null)!.ToString())
				.ToList();
		}
	}
}
