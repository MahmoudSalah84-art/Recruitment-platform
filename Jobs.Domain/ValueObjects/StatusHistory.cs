using Jobs.Domain.Common.YourProject.Domain.Common;
using Jobs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.ValueObjects
{
	public sealed class StatusHistory : ValueObject
	{
		public ApplicationStatus OldStatus { get; }
		public ApplicationStatus NewStatus { get; }
		public DateTime ChangedAt { get; }

		private StatusHistory(ApplicationStatus oldStatus,ApplicationStatus newStatus,DateTime changedAt)
		{
			OldStatus = oldStatus;
			NewStatus = newStatus;
			ChangedAt = changedAt;
		}

		// ===== Factory Method =====
		public static StatusHistory Create(ApplicationStatus oldStatus,ApplicationStatus newStatus)
		{
			if (oldStatus == newStatus)
				throw new InvalidOperationException("Old status and new status cannot be the same.");
			return new StatusHistory(oldStatus,newStatus,DateTime.UtcNow);
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return OldStatus;
			yield return NewStatus;
			yield return ChangedAt;
		}
	}
}
