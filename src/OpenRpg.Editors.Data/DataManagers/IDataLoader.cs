using System.Threading.Tasks;

namespace OpenRpg.Data.DataManagers
{
    public interface IDataLoader<T>
    {
        Task<T> LoadData();
    }
}