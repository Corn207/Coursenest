<<<<<<< HEAD
using APICommonLibrary;
=======
>>>>>>> 8f2d456107893510f74a5d3eedbdad6da5b6fe3d
using Identity.API.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings"], builder =>
    {
        //builder.EnableRetryOnFailure(1, TimeSpan.FromSeconds(3), null);
    });
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

    app.Services.OverwriteDatabase<DataContext>();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
