using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Ocelot.DependencyInjection;

namespace Gateway.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddOcelot($"Routes/{hostingContext.HostingEnvironment.EnvironmentName}", hostingContext.HostingEnvironment);
                })
                .UseStartup<Startup>();
    }
}
