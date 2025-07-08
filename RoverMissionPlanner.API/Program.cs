using FluentValidation;

using Microsoft.EntityFrameworkCore;
using RoverMissionPlanner.API.Middlewares;
using RoverMissionPlanner.API.Validators;
using RoverMissionPlanner.Application.Interfaces;
using RoverMissionPlanner.Infrastructure.Persistence;
using RoverMissionPlanner.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// agregamos services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<RoverTaskValidator>();

// agregamos DbContext InMemory
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseInMemoryDatabase("RoverDb"));
//habiitamos  cors para que se conecte angular
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngularApp",
		policy => policy
			.WithOrigins("http://localhost:4200", "https://localhost:4200")
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials()
			);
});


// agregamos la Inyeccion de dependencias
builder.Services.AddScoped<IRoverTaskService, RoverTaskService>();

var app = builder.Build();

// middleware
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.MapControllers();
app.Run();
