using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace APICommonLibrary.Options;
public static class Extensions
{
	public static IServiceCollection AddDefaultOptions<T>(
		this IServiceCollection services,
		IConfiguration configuration) where T : class
	{
		var instance = Activator.CreateInstance(typeof(T))
			?? throw new Exception($"Cannot create instance of type {typeof(T)}.");
		var sectionName = Regex.Replace(typeof(T).Name, @"Options$", string.Empty);
		var props = typeof(T).GetProperties();

		IConfigurationSection section;
		try
		{
			section = configuration.GetRequiredSection(sectionName);
		}
		catch (Exception)
		{
			configuration[sectionName] = "";
			section = configuration.GetRequiredSection(sectionName);
		}

		var childrenSection = section.GetChildren();
		if (childrenSection.Any())
		{
			var propNvals = props.Join(
				childrenSection,
				p => p.Name,
				s => s.Key,
				(p, s) => (p, s)
				);

			foreach (var (prop, sec) in propNvals)
			{
				prop.SetValue(instance, Convert.ChangeType(sec.Value, prop.PropertyType));
			}
		}

		var results = new List<ValidationResult>();
		var isValid = Validator.TryValidateObject(instance, new ValidationContext(instance), results);
		if (!isValid)
		{
			var errors = string.Join("\n", results.Select(x => x.ErrorMessage));
			throw new Exception(errors);
		}

		foreach (var info in props)
		{
			var att = info.GetCustomAttribute<RequiredAttribute>();
			if (att == null)
			{
				configuration[$"{sectionName}:{info.Name}"] = info.GetValue(instance)?.ToString();
			}
		}

		services.AddOptions<T>()
			.Bind(configuration.GetRequiredSection(sectionName))
			.ValidateDataAnnotations()
			.ValidateOnStart();

		return services;
	}
}
