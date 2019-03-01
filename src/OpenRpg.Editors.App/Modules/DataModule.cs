using Autofac;
using LazyData.Json;
using LazyData.Json.Handlers;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Mappings.Types.Primitives;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Registries;
using LazyData.Serialization;
using OpenRpg.Data;
using OpenRpg.Data.DataManagers;
using OpenRpg.Items.Defaults;

namespace OpenRpg.Editors.App.Modules
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TypeAnalyzerConfiguration>().SingleInstance();
            builder.RegisterType<MappingConfiguration>().SingleInstance();

            var primitiveRegistry = new PrimitiveRegistry();
            primitiveRegistry.AddPrimitiveCheck(new BasicPrimitiveChecker());
            builder.RegisterInstance(primitiveRegistry).As<IPrimitiveRegistry>().SingleInstance();
            
            builder.RegisterType<TypeCreator>().As<ITypeCreator>().SingleInstance();
            builder.RegisterType<TypeAnalyzer>().As<ITypeAnalyzer>().SingleInstance();
            builder.RegisterType<EverythingTypeMapper>().As<ITypeMapper>().SingleInstance();
            builder.RegisterType<MappingRegistry>().As<IMappingRegistry>().SingleInstance();
            
            builder.RegisterType<BasicJsonPrimitiveHandler>().As<IJsonPrimitiveHandler>().SingleInstance();
            builder.RegisterType<JsonSerializer>().As<IJsonSerializer, ISerializer>().SingleInstance();
            builder.RegisterType<JsonDeserializer>().As<IJsonDeserializer, IDeserializer>().SingleInstance();

            builder.RegisterType<JsonDataStoreManager<StackableItemTemplate>>().As<IDataManager<StackableItemTemplate>>()
                .WithParameter(new TypedParameter(typeof(string), "Content/item-templates.json"))
                .SingleInstance();
            
            builder.Register(c =>
                {
                    var fileManager = c.Resolve<IDataManager<StackableItemTemplate>>();
                    var dataStoreTask = fileManager.LoadData();
                    dataStoreTask.Wait();
                    return dataStoreTask.Result;
                })
                .As<DataStore<StackableItemTemplate>>()
                .SingleInstance();
        }
    }
}