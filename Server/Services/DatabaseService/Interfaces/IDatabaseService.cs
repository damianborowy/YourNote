
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourNote.Server.Services.DatabaseService
{
    public interface IDatabaseService<T> where T : class
    {
        T Create(T obj);

        T Read(string id);

        IList<T> Read();

        T Update(T obj);

        bool Delete(string id);
    }
}