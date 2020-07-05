using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.Models;

namespace LessonControllerAPI.Models
{
    public class Teacher : Role<LessonControllerDb.API.Interfaces.ITeacher>,
        API.Interfaces.ITeacher
    {       
        public Teacher(Users User, LessonControllerDb.API.Interfaces.ITeacher TeacherDb) : base(User, TeacherDb)
        {
        }

        public List<StudentGroups> GetGroupsRunning()
            => Db.GetGroupsRunning(User.Id);

        public bool UpdateSheduleInfo(GroupShedules shedule)
        {
            if (!Db.GroupControllCheck(User.Id, shedule.GroupId, shedule.LessonId))
            {
                return false;
            }

            return Db.UpdateSheduleInfo(shedule);
        }
    }
}
