using System.Collections.Generic;
using PetaPoco;

namespace $rootnamespace$.Business.Interface
{
    interface IRepository
    {
        T Single<T>(object primaryKey);

        IEnumerable<T> Query<T>();
        List<T> Fetch<T>();
        Page<T> PagedQuery<T>(long pageNumber, long itemsPerPage, string sql, params object[] args);
        int Insert(object itemToAdd);
        int Update(object itemToUpdate, object primaryKeyValue);
        int Delete<T>(object primaryKeyValue);
    }
}
