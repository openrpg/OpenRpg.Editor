using System.Threading.Tasks;

namespace OpenRpg.Data.DataManagers
{
    public interface IDataSaver<T>
    {
        Task SaveData(T data);
    }
}