using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerAPI.API.Interfaces;
using LessonControllerDb.Models;

namespace LessonControllerAPI.Models
{
    public abstract class Role<Database> : IRole
    {
        protected Users User { get; set; }
        protected Database Db { get; set; }
        protected bool IsAdmin => User.Role == "admin";

        public Role(Users User, Database Db)
        {
            this.User = User;
            this.Db = Db;
        }
    }
}
