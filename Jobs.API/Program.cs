using Jobs.API.Extensions;
using Jobs.Application;
using Jobs.Infrastructure;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure("ddxx","dddd");
builder.Services.AddApplication();
builder.Services.AddDbContext<JobDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

	options.AddInterceptors(new SoftDeleteInterceptor());
});
// في ملف Program.cs
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // ميزة لدعم عرض الأخطاء بشكل قياسي

builder.Services.AddPermissionPolicies();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
// ... بعد builder.Build()
app.UseExceptionHandler(); // تفعيل الميدل وير الخاص بالتعامل مع الأخطاء

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
