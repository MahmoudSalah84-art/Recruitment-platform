using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.BackgroundJobs
{
	/// <summary>
	/// Background worker for CV ↔ Job AI matching
	/// </summary>
	public class JobMatchingWorker : BackgroundService
	{
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				// TODO: run AI matching logic
				await Task.Delay(10000, stoppingToken);
			}
		}
	}
}
