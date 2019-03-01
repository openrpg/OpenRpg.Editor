using LazyData.Json;
using LazyData.Json.Handlers;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Mappings.Types.Primitives;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Registries;
using Microsoft.Extensions.DependencyInjection;
using OpenRpg.Core.Defaults.Conventions;
using OpenRpg.Data.DataManagers;
using OpenRpg.Editors.App.Services;
using OpenRpg.Items.Defaults.Conventions;
using OpenRpg.Localization.Databases;

namespace OpenRpg.Editors.App.Modules
{
    public class DataServiceModule
    {
        public static void Setup(IServiceCollection services)
        {
            services.AddSingleton<TypeAnalyzerConfiguration>();
            services.AddSingleton<MappingConfiguration>();

            var primitiveRegistry = new PrimitiveRegistry();
            primitiveRegistry.AddPrimitiveCheck(new BasicPrimitiveChecker());
            services.AddSingleton<IPrimitiveRegistry>(primitiveRegistry);

            services.AddSingleton<ITypeCreator, TypeCreator>();
            services.AddSingleton<ITypeAnalyzer, TypeAnalyzer>();
            services.AddSingleton<ITypeMapper, EverythingTypeMapper>();
            services.AddSingleton<IMappingRegistry, MappingRegistry>();
            services.AddSingleton<IJsonPrimitiveHandler, BasicJsonPrimitiveHandler>();
            services.AddSingleton<IJsonSerializer, JsonSerializer>();
            services.AddSingleton<IJsonDeserializer, JsonDeserializer>();
            services.AddSingleton<ICloner, Cloner>();

            RegisterFileManager<ConventionalItemTemplate>(services, "Content/Data/item-templates.json");
            RegisterFileManager<ConventionalRaceTemplate>(services, "Content/Data/race-templates.json");
            RegisterFileManager<ConventionalClassTemplate>(services, "Content/Data/class-templates.json");

            RegisterLocaleDatabase(services);
        }

        public static void RegisterFileManager<T>(IServiceCollection services, string fileLocation)
        {
            services.AddSingleton<IDataStoreManager<T>>(x =>
                new JsonDataStoreManager<T>(fileLocation, x.GetService<IJsonDeserializer>(), x.GetService<IJsonSerializer>()));

            services.AddSingleton(x =>
            {
                var fileManager = x.GetService<IDataStoreManager<T>>();
                var dataStoreTask = fileManager.LoadData();
                dataStoreTask.Wait();
                return dataStoreTask.Result;
            });
        }

        public static void RegisterLocaleDatabase(IServiceCollection services)
        {
            services.AddSingleton<ILocaleDataManager>(x =>
                new JsonLocaleDataManager("Content/Locales", x.GetService<IJsonDeserializer>(), x.GetService<IJsonSerializer>()));
            
            services.AddSingleton<ILocaleDatabase>(x =>
            {
                var fileManager = x.GetService<ILocaleDataManager>();
                var dataStoreTask = fileManager.LoadData();
                dataStoreTask.Wait();
                return dataStoreTask.Result;
            });
        }
    }
}