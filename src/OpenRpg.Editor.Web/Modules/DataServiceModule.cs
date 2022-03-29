using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using OpenRpg.Core.Classes;
using OpenRpg.Core.Common;
using OpenRpg.Core.Races;
using OpenRpg.Data;
using OpenRpg.Data.InMemory;
using OpenRpg.Editor.Infrastructure.Pipelines;
using OpenRpg.Editor.Infrastructure.Pipelines.Typed;
using OpenRpg.Editor.Infrastructure.Services;
using OpenRpg.Items.Templates;
using OpenRpg.Localization;
using OpenRpg.Localization.Data.DataSources;
using OpenRpg.Localization.Data.Repositories;
using OpenRpg.Quests;
using Persistity.Core.Serialization;
using Persistity.Endpoints.Files;
using Persistity.Flow.Builders;
using Persistity.Serializers.Json;

namespace OpenRpg.Editor.Web.Modules
{
    public class DataServiceModule
    {
        public static void Setup(IServiceCollection services)
        {
            services.AddSingleton<ISerializer, JsonSerializer>();
            services.AddSingleton<IDeserializer, JsonDeserializer>();
            services.AddSingleton<ICloner, Cloner>();
            services.AddSingleton<PipelineBuilder>();

            RegisterCollectionDataPipeline<DefaultItemTemplate>(services, "Content/Data/item-templates.json");
            RegisterCollectionDataPipeline<DefaultRaceTemplate>(services, "Content/Data/race-templates.json");
            RegisterCollectionDataPipeline<DefaultClassTemplate>(services, "Content/Data/class-templates.json");
            RegisterCollectionDataPipeline<DefaultQuest>(services, "Content/Data/quest-templates.json");
            RegisterDataPipeline<LocaleDataset>(services, "Content/Locales/en-gb.lang.json");

            RegisterRepository(services);
            RegisterLocaleRespository(services);
        }

        public static void RegisterCollectionDataPipeline<T>(IServiceCollection services, string fileLocation)
        {
            RegisterDataPipeline<List<T>>(services, fileLocation);
        }
        
        public static void RegisterDataPipeline<T>(IServiceCollection services, string fileLocation)
        {
            var fileEndpoint = new FileEndpoint(fileLocation);
            services.AddSingleton<ILoadDataPipeline<T>>(x =>
                new LoadDataPipeline<T>(x.GetService<PipelineBuilder>(), x.GetService<IDeserializer>(), fileEndpoint));
            
            services.AddSingleton<ISaveDataPipeline<T>>(x =>
                new SaveDataPipeline<T>(x.GetService<PipelineBuilder>(), x.GetService<ISerializer>(), fileEndpoint));
        }
        
        public static void RegisterRepository(IServiceCollection services)
        {
            services.AddSingleton<IDataSource>(x => new InMemoryDataSource());
            services.AddSingleton<IRepository, DefaultRepository>();
        }

        public static void RegisterLocaleRespository(IServiceCollection services)
        {
            services.AddSingleton<ILocaleRepository>(x =>
            {
                var data = new LocaleDataset{LocaleCode = "en-gb", LocaleData = new Dictionary<string, string>()};
                var localeDataSource = new InMemoryLocaleDataSource(new [] { data });
                return new LocaleRepository(localeDataSource, data.LocaleCode);
            });
        }
    }
}