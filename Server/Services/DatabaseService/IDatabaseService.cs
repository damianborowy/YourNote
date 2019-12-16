using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNote.Shared.Models;

namespace YourNote.Server.Services.DatabaseService
{
    public interface IDatabaseService<T> where T : class 
    {
        T Create(T obj);
        T Read(int id);
        IList<T> Read(); 
        T Update(T obj);
        bool Delete(int id);

    }
}
