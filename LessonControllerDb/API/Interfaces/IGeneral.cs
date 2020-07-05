using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.Models;

namespace LessonControllerDb.API.Interfaces
{
    public interface IGeneral
    {
        Users GetUser(string login, string password);
        Users GetUserWithoutPassword(string login);
    }
}
