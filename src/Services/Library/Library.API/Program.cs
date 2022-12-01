using APICommonLibrary;
using Library.API.Contexts;
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

<<<<<<< HEAD
=======
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();

    if (bool.TrueString == builder.Configuration["Migrate"])
    {
        db.Database.Migrate();
    }

    if (bool.TrueString == builder.Configuration["RecreateDatabase"])
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }

    if (bool.TrueString == builder.Configuration["Seeding"])
    {
        db.Seeding();
    }
}


>>>>>>> 8f2d456107893510f74a5d3eedbdad6da5b6fe3d
app.UseAuthorization();

app.MapControllers();

app.Run();
