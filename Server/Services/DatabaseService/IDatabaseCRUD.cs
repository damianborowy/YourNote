using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNote.Shared.Models;

namespace YourNote.Server.Services.DatabaseService
{
    public interface IDatabaseCRUD<T> where T : class 
    {
        bool Create(T obj);
        T Read(int id);
        IList<T> Read(); 
        bool Update(int id, T obj);
        bool Delete(T obj);

    }
}
