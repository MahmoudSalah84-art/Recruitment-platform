using Jobs.Infrastructure.Data;
using Jobs.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;

namespace Jobs.Infrastructure.BackgroundJobs
{
	public class OutboxProcessor : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly ILogger<OutboxProcessor> _logger;
		public OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger)
		{
			_scopeFactory = scopeFactory;
			_logger = logger;
		}
		protected override async Task ExecuteAsync(CancellationToken ct)
		{
			_logger.LogInformation("Outbox Processor started...");

			while (!ct.IsCancellationRequested)
			{
				try
				{
					await ProcessOutboxMessages(ct);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error occurred while processing outbox messages.");
				}

				await Task.Delay(TimeSpan.FromSeconds(5), ct);
			}

		}

		private async Task ProcessOutboxMessages(CancellationToken stoppingToken)
		{
			using var scope = _scopeFactory.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<JobDbContext>();
			var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>(); // MediatR

			// get unprocessed messages in batches (e.g., 20 at a time)
			var messages = await dbContext.Set<OutboxMessage>()
				.Where(m => !m.Processed)
				.OrderBy(m => m.OccurredOn)
				.Take(20) // Batch size
				.ToListAsync(stoppingToken);

			if (!messages.Any()) return;

			foreach (var message in messages)
			{
				try
				{
					var eventType = GetEventTypeByName(message.Type);
					if (eventType == null) continue;

					var domainEvent = JsonSerializer.Deserialize(message.Content, eventType);

					if (domainEvent != null)
					{
						await publisher.Publish(domainEvent, stoppingToken);
					}

					message.MarkProcessed();
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, $"Failed to process message {message.Id}");
					message.SetError(ex.Message);
				}
			}

			await dbContext.SaveChangesAsync(stoppingToken);
		}



		private Type? GetEventTypeByName(string typeName)
		{
			// ابحث في الـ Assembly الخاص بالـ Domain عن كلاس بهذا الاسم
			return AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.FirstOrDefault(t => t.Name == typeName);
		}
	}





	//public class OutboxProcessor : BackgroundService
	//{
	//	private const int BatchSize = 20;
	//	private static readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(5);

	//	// Cache لتجنب scan الـ assemblies في كل message
	//	private static readonly Lazy<Dictionary<string, Type>> _eventTypeCache = new(() =>
	//		AppDomain.CurrentDomain.GetAssemblies()
	//			.SelectMany(a =>
	//			{
	//				try { return a.GetTypes(); }
	//				catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null)!; }
	//			})
	//			.Where(t => typeof(INotification).IsAssignableFrom(t) && !t.IsAbstract)
	//			.ToDictionary(t => t.Name, t => t, StringComparer.Ordinal)
	//	);

	//	private readonly IServiceScopeFactory _scopeFactory;
	//	private readonly ILogger<OutboxProcessor> _logger;

	//	public OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger)
	//	{
	//		_scopeFactory = scopeFactory;
	//		_logger = logger;
	//	}

	//	protected override async Task ExecuteAsync(CancellationToken ct)
	//	{
	//		_logger.LogInformation("Outbox Processor started.");

	//		while (!ct.IsCancellationRequested)
	//		{
	//			try
	//			{
	//				await ProcessOutboxMessagesAsync(ct);
	//			}
	//			catch (OperationCanceledException) when (ct.IsCancellationRequested)
	//			{
	//				break; // shutdown نظيف
	//			}
	//			catch (Exception ex)
	//			{
	//				_logger.LogError(ex, "Unhandled error in outbox processor loop.");
	//			}

	//			try
	//			{
	//				await Task.Delay(PollingInterval, ct);
	//			}
	//			catch (OperationCanceledException)
	//			{
	//				break;
	//			}
	//		}

	//		_logger.LogInformation("Outbox Processor stopped.");
	//	}

	//	private async Task ProcessOutboxMessagesAsync(CancellationToken ct)
	//	{
	//		using var scope = _scopeFactory.CreateScope();
	//		var dbContext = scope.ServiceProvider.GetRequiredService<JobDbContext>();
	//		var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

	//		var messages = await dbContext.Set<OutboxMessage>()
	//			.Where(m => !m.Processed )
	//			.OrderBy(m => m.OccurredOn)
	//			.Take(BatchSize)
	//			.ToListAsync(ct);

	//		if (messages.Count == 0) return;

	//		_logger.LogDebug("Processing {Count} outbox messages.", messages.Count);

	//		foreach (var message in messages)
	//		{
	//			await ProcessSingleMessageAsync(message, publisher, ct);
	//		}

	//		await dbContext.SaveChangesAsync(ct);
	//	}

	//	private async Task ProcessSingleMessageAsync( OutboxMessage message, IPublisher publisher, CancellationToken ct)
	//	{
	//		try
	//		{
	//			if (!_eventTypeCache.Value.TryGetValue(message.Type, out var eventType))
	//			{
	//				_logger.LogWarning("Unknown event type '{Type}' for message {Id}. Skipping.", message.Type, message.Id);
	//				message.MarkProcessed(); // منعاً للتكرار الأبدي
	//				return;
	//			}

	//			var domainEvent = JsonSerializer.Deserialize(message.Content, eventType) as INotification;

	//			if (domainEvent is null)
	//			{
	//				_logger.LogWarning("Failed to deserialize message {Id} as INotification.", message.Id);
	//				message.MarkProcessed();
	//				return;
	//			}

	//			await publisher.Publish(domainEvent, ct);

	//			message.MarkProcessed();

	//			_logger.LogDebug("Message {Id} ({Type}) published successfully.", message.Id, message.Type);
	//		}
	//		catch (Exception ex)
	//		{
	//			_logger.LogError(ex, "Failed to process outbox message {Id} .", message.Id);

	//			message.SetError(ex.Message);
	//		}
	//	}

	//	private static Type? GetEventTypeByName(string typeName) =>
	//		_eventTypeCache.Value.GetValueOrDefault(typeName);
	//}
}