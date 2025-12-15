using Jobs.Domain.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Jobs.Domain.Common
{
	public abstract class AggregateRoot : BaseEntity 
	{

		// ===== Domain Events =====
		private readonly List<DomainEvent> _events = new();
		public IReadOnlyCollection<DomainEvent> Events => _events.AsReadOnly();


        protected void AddEvent(DomainEvent @event) => _events.Add(@event);
		public void ClearEvents() => _events.Clear();


		



		// in future

		// 5. (اختياري) إدارة التزامن Optimistic Concurrency
		// [ConcurrencyCheck]
		// public byte[] RowVersion { get; set; }
		//[Timestamp]
		//public byte[] RowVersion { get; set; }
	}
}