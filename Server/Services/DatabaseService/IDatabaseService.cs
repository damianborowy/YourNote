using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNote.Shared.Models;

namespace YourNote.Server.Services.DatabaseService
{
    public interface IDatabaseService<T> where T : class 
    {
        bool Create(T obj);
        T Read(int id);
        IList<T> Read(); 
        bool Update(T obj);
        bool Delete(int id);

    }
}
