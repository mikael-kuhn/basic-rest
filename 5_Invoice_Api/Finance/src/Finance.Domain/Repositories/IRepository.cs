using System.Collections.Generic;

namespace Finance.Domain.Repositories
{
    public interface IRepository<T>
    {
        T Get(string id);
        void Delete(string id);
        Version Update(T instance);
        T Create(T instance);
        bool Exists(string id);
        string GetCurrentVersion(string id);
        IEnumerable<T> GetAll();
    }
}