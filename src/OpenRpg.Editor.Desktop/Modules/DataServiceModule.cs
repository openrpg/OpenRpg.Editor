using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenRpg.Core.Classes;
using OpenRpg.Core.Common;
using OpenRpg.Core.Races;
using OpenRpg.Data.Defaults;
using OpenRpg.Editor.Infrastructure.Pipelines;
using OpenRpg.Editor.Infrastructure.Pipelines.Typed;
using OpenRpg.Editor.Infrastructure.Services;
using OpenRpg.Items.Templates;
using OpenRpg.Localization;
using OpenRpg.Localization.Repositories;
using OpenRpg.Quests;
using Persistity.Core.Serialization;
using Persistity.Endpoints.Files;
using Persistity.Flow.Builders;
using Persistity.Serializers.Json;
using JsonSerializer = Persistity.Serializers.Json.JsonSerializer;

namespace OpenRpg.Editor.Desktop.Modules
{
    public class DataServiceModule
    {
        public static void Setup(IServiceCollection services)
        {
            services.AddSingleton<ISerializer, JsonSerializer>();
            services.AddSingleton<IDeserializer, JsonDeserializer>();
            services.AddSingleton<ICloner, Cloner>();
            services.AddSingleton<PipelineBuilder>();

            var content = File.ReadAllText("Content/Data/item-templates.json");
            var races = JsonConvert.DeserializeObject<List<DefaultRaceTemplate>>(content);
            
            RegisterCollectionDataPipeline<DefaultItemTemplate>(services, "Content/Data/item-templates.json");
            RegisterCollectionDataPipeline<DefaultRaceTemplate>(services, "Content/Data/race-templates.json");
            RegisterCollectionDataPipeline<DefaultClassTemplate>(services, "Content/Data/class-templates.json");
            RegisterCollectionDataPipeline<DefaultQuest>(services, "Content/Data/quest-templates.json");
            RegisterDataPipeline<LocaleDataset>(services, "Content/Locales/en-gb.lang.json");

            RegisterRepository<DefaultItemTemplate>(services);
            RegisterRepository<DefaultRaceTemplate>(services);
            RegisterRepository<DefaultClassTemplate>(services);
            RegisterRepository<DefaultQuest>(services);

            RegisterLocaleRespository(services);
        }

        public static void RegisterCollectionDataPipeline<T>(IServiceCollection services, string fileLocation)
        { RegisterDataPipeline<List<T>>(services, fileLocation); }
        
        public static void RegisterDataPipeline<T>(IServiceCollection services, string fileLocation)
        {
            var fileEndpoint = new FileEndpoint(fileLocation);
            services.AddSingleton<ILoadDataPipeline<T>>(x =>
                new LoadDataPipeline<T>(x.GetService<PipelineBuilder>(), x.GetService<IDeserializer>(), fileEndpoint));
            
            services.AddSingleton<ISaveDataPipeline<T>>(x =>
                new SaveDataPipeline<T>(x.GetService<PipelineBuilder>(), x.GetService<ISerializer>(), fileEndpoint));
        }
        
        public static void RegisterRepository<T>(IServiceCollection services)
            where T : IHasDataId
        {
            services.AddSingleton<InMemoryDataRepository<T>>(x =>
            {
                var data = LoadPipelineData<List<T>>(x);
                return new InMemoryDataRepository<T>(data);
            });
        }

        public static void RegisterLocaleRespository(IServiceCollection services)
        {
            services.AddSingleton<LocaleRepository>(x =>
            {
                var data = LoadPipelineData<LocaleDataset>(x);
                return new LocaleRepository(data);
            });
        }
        
        public static T LoadPipelineData<T>(IServiceProvider services)
        {
            var loadPipeline = services.GetService<ILoadDataPipeline<T>>();
            if (loadPipeline == null)
            { throw new Exception($"Cant find load pipeline for {typeof(T).Name}"); }
            
            var wrapperTask = Task.Run(async () => (T)await loadPipeline.Execute());
            return wrapperTask.Result;
        }
    }
}