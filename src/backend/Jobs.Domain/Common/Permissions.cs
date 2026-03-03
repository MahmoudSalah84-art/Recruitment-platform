
namespace Jobs.Domain.Common
{
	public static class Permissions
	{
		public static class Jobs
		{
			public const string Create = "Permissions.Jobs.Create";
			public const string Edit = "Permissions.Jobs.Edit";
			public const string Delete = "Permissions.Jobs.Delete";
			public const string View = "Permissions.Jobs.View";
		}

		public static class Applications
		{
			public const string Apply = "Permissions.Applications.Apply";
			public const string Review = "Permissions.Applications.Review";
		}
	}
}
