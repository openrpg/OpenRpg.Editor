using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

namespace OpenRpg.Editor.Desktop.Modules
{
    public class DataServiceModule
    {
        public static string GetContentDirectory()
        {
#if DEBUG
            return "../../../Content";
#else
            return"./Content";
#endif
        }
        
        public static void Setup(IServiceCollection services)
        {
            services.AddSingleton<ISerializer, JsonSerializer>();
            services.AddSingleton<IDeserializer, JsonDeserializer>();
            services.AddSingleton<ICloner, Cloner>();
            services.AddSingleton<PipelineBuilder>();

            var contentDir = GetContentDirectory();
            RegisterCollectionDataPipeline<DefaultItemTemplate>(services, $"{contentDir}/Data/item-templates.json");
            RegisterCollectionDataPipeline<DefaultRaceTemplate>(services, $"{contentDir}/Data/race-templates.json");
            RegisterCollectionDataPipeline<DefaultClassTemplate>(services, $"{contentDir}/Data/class-templates.json");
            RegisterCollectionDataPipeline<DefaultQuest>(services, $"{contentDir}/Data/quest-templates.json");
            RegisterDataPipeline<LocaleDataset>(services, $"{contentDir}/Locales/en-gb.lang.json");

            RegisterRepository(services);
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
        
        public static void RegisterRepository(IServiceCollection services)
        {
            services.AddSingleton<IDataSource>(x =>
            {
                var itemTemplateData = LoadPipelineData<List<DefaultItemTemplate>>(x);
                var raceTemplateData = LoadPipelineData<List<DefaultRaceTemplate>>(x);
                var classTemplateData = LoadPipelineData<List<DefaultClassTemplate>>(x);
                var questTemplateData = LoadPipelineData<List<DefaultQuest>>(x);
                var inMemoryDatabase = new Dictionary<Type, Dictionary<object, object>>();
                inMemoryDatabase.Add(typeof(IItemTemplate), itemTemplateData.ToDictionary(x => (object)x.Id, x => (object)x));
                inMemoryDatabase.Add(typeof(IRaceTemplate), raceTemplateData.ToDictionary(x => (object)x.Id, x => (object)x));
                inMemoryDatabase.Add(typeof(IClassTemplate), classTemplateData.ToDictionary(x => (object)x.Id, x => (object)x));
                inMemoryDatabase.Add(typeof(IQuest), questTemplateData.ToDictionary(x => (object)x.Id, x => (object)x));
                return new InMemoryDataSource(inMemoryDatabase);
            });

            services.AddSingleton<IRepository, DefaultRepository>();
        }

        public static void RegisterLocaleRespository(IServiceCollection services)
        {
            services.AddSingleton<ILocaleDataSource>(x =>
            {
                var data = LoadPipelineData<LocaleDataset>(x);
                return new InMemoryLocaleDataSource(new [] { data });
            });

            services.AddSingleton<ILocaleRepository>(x =>
            {
                var datasource = x.GetService<ILocaleDataSource>();
                var defaultLocale = (datasource as InMemoryLocaleDataSource).LocaleDatasets.First().Key;
                return new LocaleRepository(datasource, defaultLocale);
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