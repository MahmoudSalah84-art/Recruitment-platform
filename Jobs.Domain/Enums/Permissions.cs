
namespace Jobs.Domain.Enums
{
    public class Permissions
    {
		public static class Users
		{
			public const string View = "Permissions.Users.View";
			public const string Create = "Permissions.Users.Create";
			public const string Edit = "Permissions.Users.Edit";
			public const string Delete = "Permissions.Users.Delete";
		}

		public static class Roles
		{
			public const string View = "Permissions.Roles.View";
			public const string Create = "Permissions.Roles.Create";
			public const string Edit = "Permissions.Roles.Edit";
			public const string Delete = "Permissions.Roles.Delete";
		}

		public static IEnumerable<string> GetAll() =>
			typeof(Permissions).GetNestedTypes()
				.SelectMany(t => t.GetFields()
				.Where(f => f.IsLiteral && !f.IsInitOnly)
				.Select(f => f.GetValue(null)!.ToString()!));
	}
}
