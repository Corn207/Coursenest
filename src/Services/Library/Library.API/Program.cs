using Library.API;
using Library.API.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.HandleEnvironmentVariables(new[]
{
    ("Migrating", false),
    ("RecreateDatabase", false),
    ("Seeding", false)
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<LibraryContext>(options =>
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
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();

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


app.UseAuthorization();

app.MapControllers();

app.Run();
