using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourNote.Server.Services.DatabaseService
{
    interface IDatabaseService
    {
        bool Create<T>(T obj);
        T Read<T>(int? id = null);
        bool Update<T>(T obj);
        bool Delete<T>(int id);
    }
}
