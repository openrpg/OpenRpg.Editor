using System.Threading.Tasks;
using LazyData.Json;

namespace OpenRpg.Data.DataManagers
{
    public abstract class JsonDataManager<T> : IDataManager<T>
    {
        public IJsonDeserializer JsonDeserializer { get; }
        public IJsonSerializer JsonSerializer { get; }

        public JsonDataManager(IJsonDeserializer jsonDeserializer, IJsonSerializer jsonSerializer)
        {
            JsonDeserializer = jsonDeserializer;
            JsonSerializer = jsonSerializer;
        }

        public abstract Task<T> LoadData();
        public abstract Task SaveData(T data);
    }
}