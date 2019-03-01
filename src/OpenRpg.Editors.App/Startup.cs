using Autofac;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenRpg.Editors.App.Modules;

namespace OpenRpg.Editors.App
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        /*
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule<DataModule>();
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }*/

        public void ConfigureServices(IServiceCollection services)
        {
            DataServiceModule.Setup(services);
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
        
        
    }
}