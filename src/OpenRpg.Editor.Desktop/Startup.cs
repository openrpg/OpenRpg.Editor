using Microsoft.Extensions.DependencyInjection;
using OpenRpg.Editor.Desktop.Modules;
using OpenRpg.Editor.Web;
using Photino.Blazor;

namespace OpenRpg.Editor.Desktop
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            DataServiceModule.Setup(services);
        }

        public void Configure(DesktopApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}