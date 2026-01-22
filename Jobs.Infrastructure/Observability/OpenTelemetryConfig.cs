// Ensure the following NuGet packages are installed in your project:
// - OpenTelemetry
// - OpenTelemetry.Extensions.Hosting
// - OpenTelemetry.Instrumentation.AspNetCore
// - OpenTelemetry.Instrumentation.EntityFrameworkCore
// - OpenTelemetry.Instrumentation.Runtime
// - OpenTelemetry.Api

//using Microsoft.Extensions.DependencyInjection;
//using OpenTelemetry.Resources;
//using OpenTelemetry.Trace;
//using OpenTelemetry.Metrics;

//namespace Jobs.Infrastructure.Observability
//{
//	public static class OpenTelemetryConfig
//	{
//		public static IServiceCollection AddObservability(this IServiceCollection services)
//		{
//			services.AddOpenTelemetryTracing(tracing =>
//			{
//				tracing
//					.SetResourceBuilder(ResourceBuilder.CreateDefault())
//					.AddAspNetCoreInstrumentation()
//					.AddEntityFrameworkCoreInstrumentation();
//			});

//			services.AddOpenTelemetry()
//				.WithMetrics(metrics =>
//				{
//					metrics
//						.SetResourceBuilder(ResourceBuilder.CreateDefault())
//						.AddAspNetCoreInstrumentation()
//						.AddRuntimeInstrumentation();
//				});

//			return services;
//		}
//	}
//}
