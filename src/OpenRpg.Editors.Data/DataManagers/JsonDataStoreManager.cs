using System.IO;
using System.Threading.Tasks;
using LazyData;
using LazyData.Json;

namespace OpenRpg.Data.DataManagers
{
    public class JsonDataStoreManager<T> : JsonDataManager<DataStore<T>>, IDataStoreManager<T>
    {
        public string Filename { get; set; }

        public JsonDataStoreManager(string filename, IJsonDeserializer jsonDeserializer, IJsonSerializer jsonSerializer)
            : base(jsonDeserializer, jsonSerializer)
        {
            Filename = filename;
        }

        public override async Task<DataStore<T>> LoadData()
        {
            var jsonData = File.ReadAllText(Filename);
            var dataObject = new DataObject(jsonData);
            return JsonDeserializer.Deserialize<DataStore<T>>(dataObject);
        }

        public override async Task SaveData(DataStore<T> data)
        {
            var dataObject = JsonSerializer.Serialize(data);
            File.WriteAllText(Filename, dataObject.AsString);
        }
    }
}