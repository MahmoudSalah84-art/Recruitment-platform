//using Microsoft.Extensions.Hosting;
//using Serilog;


//namespace Jobs.Infrastructure.Observability
//{
//	public static class SerilogConfig
//	{
//		public static void Configure(HostBuilderContext context, LoggerConfiguration configuration)
//		{
//			configuration
//				.ReadFrom.Configuration(context.Configuration) // قراءة الإعدادات من appsettings.json
//				.Enrich.FromLogContext()
//				.Enrich.WithMachineName() // مفيد لو عندك أكثر من سيرفر (Microservices)
//				.Enrich.WithThreadId()    // مفيد لتتبع العمليات المتوازية
//				.WriteTo.Console()        // للكتابة في الشاشة أثناء التطوير

//				// التعديل 2: استخدام Async للكتابة في الملفات لعدم تعطيل التطبيق
//				.WriteTo.Async(a => a.File(
//					path: "logs/jobsite-.log",
//					rollingInterval: RollingInterval.Day,
//					// التعديل 3: تنسيق الرسالة لتكون مقروءة أكثر
//					outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
//					retainedFileCountLimit: 30 // الاحتفاظ بملفات آخر 30 يوم فقط لتوفير المساحة
//				));
//		}
//	}
//}
