namespace Library.API;

public static class BuilderExtension
{
    public static IConfigurationBuilder HandleEnvironmentVariables(this ConfigurationManager configuration, (string key, bool def)[] variables)
    {
        foreach (var (key, def) in variables)
        {
            var init = configuration[key];
            configuration[key] = def ? "True" : "False";
            if (init != null)
            {
                switch (init.ToLowerInvariant())
                {
                    case "true":
                    case "t":
                    case "yes":
                    case "y":
                    case "1":
                        configuration[key] = "True";
                        break;

                    case "false":
                    case "f":
                    case "no":
                    case "n":
                    case "0":
                        configuration[key] = "False";
                        break;
                }
            }
        }

        return configuration;
    }
}
