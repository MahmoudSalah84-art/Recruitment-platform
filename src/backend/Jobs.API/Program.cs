using Jobs.API.Extensions;
using Jobs.API.Middlewares;
using Jobs.Application;
using Jobs.Infrastructure;
using Jobs.Infrastructure.Identity;
using Jobs.Infrastructure.Identity.Seeder;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


builder.Services.AddOpenApi(); 


builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


builder.Services.AddProblemDetails(); // ميزة لدعم عرض الأخطاء بشكل قياسي

builder.Services.AddPermissionPolicies();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		builder =>
		{
			builder//.AllowAnyOrigin() 
				   .AllowAnyMethod()
				   .AllowAnyHeader()
				   .AllowCredentials();
		}
	);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseExceptionHandler();


using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

	await IdentitySeeder.SeedAsync(roleManager);
}

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.MapOpenApi();
	//Add Swagger UI
	app.UseSwagger();
	app.UseSwaggerUI();
//}


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
