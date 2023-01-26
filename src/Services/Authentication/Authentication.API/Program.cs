using APICommonLibrary.Extensions;
using Authentication.API.Consumers;
using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Options;

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

app.UseAuthorization();

app.MapControllers();

app.Run();
