using Jobs.Domain.Common;

namespace Jobs.Application.Common.Helpers
{
	public class RolesHelper
	{
		public static List<string?> GetAllRoles()
		{
			return typeof(Roles)
				.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
				.Select(f => f.GetValue(null)!.ToString())
				.ToList();
		}
	}
}