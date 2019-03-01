using System.Collections.Generic;

namespace OpenRpg.Data
{
    public class DataStore<T>
    {
        public ICollection<T> Data { get; set; } = new List<T>();
    }
}