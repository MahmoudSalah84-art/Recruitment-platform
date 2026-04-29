using Jobs.API.Extensions;
using Jobs.API.Middlewares;
using Jobs.Application;
using Jobs.Infrastructure;
using Jobs.Infrastructure.Identity;
using Jobs.Infrastructure.Identity.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;

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
builder.Services.AddSwaggerGen(doc =>
{
	var xmlFile = Path.Combine(AppContext.BaseDirectory,"ApiDocumentation.xml");
	doc.IncludeXmlComments(xmlFile);

	doc.SwaggerDoc("v1",
		new OpenApiInfo
		{
			Version = "v1",
			Title = "Jooobs API",
			Description = "API for managing job applications, user profiles, and related functionalities.",
			Contact = new OpenApiContact
			{
				Name = "mahmoud mohamed salah"
			}
		});

});


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
