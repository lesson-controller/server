using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.Models;

namespace LessonControllerAPI.Models
{
    public class Student : Role<LessonControllerDb.API.Interfaces.IStudent>,
        API.Interfaces.IStudent
    {
        public Student(Users User, LessonControllerDb.API.Interfaces.IStudent StudentDb) : base(User, StudentDb)
        {
        }

        public List<StudentGroups> GetGroupsOfWhich()
            => Db.GetGroupsOfWhich(User.Id);
    }
}
