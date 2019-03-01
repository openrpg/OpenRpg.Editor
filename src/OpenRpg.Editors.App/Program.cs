using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Blazor.Hosting;

namespace OpenRpg.Editors.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                .ConfigureServices(services => services.AddAutofac())
                .UseBlazorStartup<Startup>();
    }
}