using APICommonLibrary;
using Authentication.API.Contexts;
using Authentication.API.Models;
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

    app.Services.OverwriteDatabase<DataContext>(context =>
    {
        var cred = new Credential("testUser", "testPass")
        {
            UserId = 1
        };
        cred.Roles.Add(new Role() { Expiry = DateTime.Now, Type = RoleType.Student });

        context.Credentials.Add(cred);
        context.SaveChanges();
    });
}


app.UseAuthorization();

app.MapControllers();

app.Run();
