using System.Collections.Generic;
using OpenRpg.Core.Common;
using OpenRpg.Data.Defaults.Queries.Common;
using OpenRpg.Data.Repositories;

namespace OpenRpg.Editor.Infrastructure.Extensions
{
    public static class IDataRepositoryExtensions
    {
        public static IEnumerable<T> GetAll<T>(this IDataRepository<T> repository) where T : IHasDataId
        { return repository.Find(new GetAllQuery<T>()); }
        
        public static int GetCount<T>(this IDataRepository<T> repository) where T : IHasDataId
        { return repository.Find(new GetCountQuery<T>()); }
    }
}