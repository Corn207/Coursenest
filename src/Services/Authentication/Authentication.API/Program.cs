using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Options;
using CommonLibrary.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDefaultServices<DataContext>(builder.Configuration);
builder.Services.AddRequiredOptions<JwtOptions>(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.DatabaseSetup();

app.Run();
