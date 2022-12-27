using APICommonLibrary.Extensions;
using Identity.API.Consumers;
using Identity.API.Infrastructure.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDefaultServices<DataContext>(
	builder.Configuration,
	busConfig =>
	{
		busConfig.AddConsumer<CreateUserConsumer>();
	});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.DatabaseStartup();

app.Run();
