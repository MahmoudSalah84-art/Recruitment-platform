
namespace Jobs.Domain.Common
{
	public static class Permissions
	{
		// Users
		public const string Users_View = "Users.View";
		public const string Users_Create = "Users.Create";
		public const string Users_Update = "Users.Update";
		public const string Users_Delete = "Users.Delete";
		// Companies
		public const string Companies_View = "Companies.View";
		public const string Companies_Create = "Companies.Create";
		public const string Companies_Update = "Companies.Update";
		public const string Companies_Delete = "Companies.Delete";

		// Jobs
		public const string Jobs_View = "Jobs.View";
		public const string Jobs_Create = "Jobs.Create";
		public const string Jobs_Update = "Jobs.Update";
		public const string Jobs_Delete = "Jobs.Delete";

		// Applications
		public const string Applications_View = "Applications.View";
		public const string Applications_Apply = "Applications.Apply";
		public const string Applications_UpdateStatus = "Applications.UpdateStatus";
		public const string Applications_Delete = "Applications.Delete";

		// CVs
		public const string CVs_View = "CVs.View";
		public const string CVs_Upload = "CVs.Upload";
		public const string CVs_Update = "CVs.Update";
		public const string CVs_Delete = "CVs.Delete";

		// Reviews
		public const string Reviews_View = "Reviews.View";
		public const string Reviews_Create = "Reviews.Create";
		public const string Reviews_Delete = "Reviews.Delete";

		// Roles & Permissions
		public const string Roles_View = "Roles.View";
		public const string Roles_Create = "Roles.Create";
		public const string Roles_Update = "Roles.Update";
		public const string Roles_Delete = "Roles.Delete";

		public const string Permissions_Manage = "Permissions.Manage";
	}
}
