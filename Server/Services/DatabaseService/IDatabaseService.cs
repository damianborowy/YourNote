﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNote.Shared.Models;

namespace YourNote.Server.Services.DatabaseService
{
    interface IDatabaseService
    {
        bool CreateUser (User obj);
        IEnumerable<User> ReadUser(int? id = null);
        bool UpdateUser(User obj, int id);
        bool DeleteUser(int id);

        bool CreateNote(Note obj);
        IEnumerable<Note> ReadNote(int? id = null);
        bool UpdateNote(Note obj, int id);
        bool DeleteNote(int id);
    }
}
