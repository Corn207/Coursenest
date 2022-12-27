using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.SetBasePath(builder.Environment.ContentRootPath)
	.AddJsonFile("Configurations/authentication.api.json", optional: false, reloadOnChange: true)
	.AddJsonFile("Configurations/identity.api.json", optional: false, reloadOnChange: true)
	.AddJsonFile("Configurations/library.api.json", optional: false, reloadOnChange: true)
	.AddJsonFile("Configurations/userdata.api.json", optional: false, reloadOnChange: true)
	.AddJsonFile("Configurations/global.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
	{
		//options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = new TokenValidationParameters()
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
			ValidateIssuerSigningKey = true
		};
	});

var app = builder.Build();

await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
