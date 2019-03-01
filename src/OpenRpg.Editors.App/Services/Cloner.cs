using LazyData.Json;

namespace OpenRpg.Editors.App.Services
{
    public class Cloner : ICloner
    {
        public IJsonSerializer Serializer { get; }
        public IJsonDeserializer Deserializer { get; }

        public Cloner(IJsonSerializer serializer, IJsonDeserializer deserializer)
        {
            Serializer = serializer;
            Deserializer = deserializer;
        }

        public T Clone<T>(T source) where T : new()
        {
            var data = Serializer.Serialize(source);
            return Deserializer.Deserialize<T>(data);
        }
    }
}