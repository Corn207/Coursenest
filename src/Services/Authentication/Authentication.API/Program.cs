using APICommonLibrary;
using APICommonLibrary.Options;
using Authentication.API.Contexts;
using Authentication.API.Options;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DbContext, DataContext>();
builder.Services.AddControllers();

builder.Services.AddOptions<ConnectionOptions>()
	.Bind(builder.Configuration.GetSection(ConnectionOptions.SectionName))
	.ValidateDataAnnotations();
builder.Services.AddOptions<DatabaseOptions>()
	.Bind(builder.Configuration.GetSection(DatabaseOptions.SectionName))
	.ValidateDataAnnotations();
builder.Services.AddOptions<JwtOptions>()
	.Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
	.ValidateDataAnnotations();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddMassTransit(x =>
{
	x.UsingConfiguredRabbitMq(builder.Configuration);
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

app.Services.Startup();

app.Run();
