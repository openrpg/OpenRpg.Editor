namespace OpenRpg.Data.DataManagers
{
    public interface IDataManager<T> : IDataLoader<T>, IDataSaver<T>
    {}
}