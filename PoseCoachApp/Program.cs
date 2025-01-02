using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace PoseCoachApp;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        
        var serviceProvider = ConfigureServices();

        ApplicationConfiguration.Initialize();
        Application.Run(new Form1(serviceProvider.GetRequiredService<ICoach>()));
    }

    private static ServiceProvider  ConfigureServices()
    {
        var services = new ServiceCollection();
        
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (apiKey == null)
        {
            throw new Exception("Please set the OPENAI_API_KEY environment variable.");
        }
        
        services.AddOpenAIChatCompletion("gpt-4o",apiKey);
        services.AddSingleton<ICoach, ChatGPTCoach>();
        
        services.AddSingleton<Form1>();


        return services.BuildServiceProvider();

    }
}