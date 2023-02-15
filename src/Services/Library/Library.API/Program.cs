using CommonLibrary.API.Extensions;
using Library.API.Infrastructure.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDefaultServices<DataContext>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
