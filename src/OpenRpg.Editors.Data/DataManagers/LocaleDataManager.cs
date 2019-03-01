using System.IO;
using System.Threading.Tasks;
using LazyData;
using LazyData.Json;
using OpenRpg.Localization.Databases;
using OpenRpg.Localization.Repositories;

namespace OpenRpg.Data.DataManagers
{
    public class JsonLocaleDataManager : JsonDataManager<ILocaleDatabase>, ILocaleDataManager
    {
        public string LocaleDir { get; set; }

        public JsonLocaleDataManager(string localeDir, IJsonDeserializer jsonDeserializer,
            IJsonSerializer jsonSerializer) : base(jsonDeserializer, jsonSerializer)
        {
            LocaleDir = localeDir;
        }

        public override async Task<ILocaleDatabase> LoadData()
        {
            var database = new LocaleDatabase();
            var localeFiles = Directory.GetFiles(LocaleDir);
            foreach (var localeFile in localeFiles)
            {
                var fileData = File.ReadAllText(localeFile);
                var repository = JsonDeserializer.Deserialize<LocaleRepository>(new DataObject(fileData));
                database.AddRepository(repository);
            }
            return database;
        }

        public override Task SaveData(ILocaleDatabase data)
        {
            foreach (var repository in data.GetAllRepositories())
            {
                var outputFilename = $"{LocaleDir}/{repository.LocaleCode}.lang.json";
                var dataObject = JsonSerializer.Serialize(repository);
                File.WriteAllText(outputFilename, dataObject.AsString);
            }

            return Task.CompletedTask;
        }
    }
}