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




}

