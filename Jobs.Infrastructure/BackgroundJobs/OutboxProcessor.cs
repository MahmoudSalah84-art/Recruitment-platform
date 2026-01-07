using Jobs.Infrastructure.Data;
using Jobs.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Outbox Processor started...");

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await ProcessOutboxMessages(stoppingToken);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error occurred while processing outbox messages.");
				}

				await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
			}
			
		}


		
		private async Task ProcessOutboxMessages(CancellationToken stoppingToken)
		{
			// إنشاء Scope يدوي لأن الـ DbContext والـ Mediator غالباً Scoped
			using var scope = _scopeFactory.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<JobDbContext>();
			var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>(); // MediatR

			// 1. جلب الرسائل غير المعالجة
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
					// 2. تحويل الـ JSON إلى كائن (Object)
					// ملحوظة: نحتاج لمعرفة الـ Type الأصلي من الاسم المخزن
					var eventType = GetEventTypeByName(message.Type);
					if (eventType == null) continue;

					var domainEvent = JsonSerializer.Deserialize(message.Content, eventType);

					if (domainEvent != null)
					{
						// 3. النشر داخلياً لمستخدمي MediatR Notification Handlers
						await publisher.Publish(domainEvent, stoppingToken);
					}

					// 4. تحديث حالة الرسالة بنجاح
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



		// ميثود مساعدة لتحويل اسم الكلاس المخزن (String) إلى Type حقيقي
		private Type? GetEventTypeByName(string typeName)
		{
			// ابحث في الـ Assembly الخاص بالـ Domain عن كلاس بهذا الاسم
			return AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.FirstOrDefault(t => t.Name == typeName);
		}
	}
	
}
