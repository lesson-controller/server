using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.API.Interfaces;
using LessonControllerDb.Models;

namespace LessonControllerDb.Modules
{
    public class LessonControllerDatabase : API.Database,
        API.Interfaces.IGeneral,
        API.Interfaces.IStudent,
        API.Interfaces.ITeacher,
        API.Interfaces.IUniversity,
        API.Interfaces.IStudentAttendances
    {
        public LessonControllerDatabase()
        {

        }

        public void AddGroup(string Name)
        {
            throw new NotImplementedException();
        }

        public void AddLessonToGroup(int lessonId, int groupId)
        {
            throw new NotImplementedException();
        }

        public void AddShedule(GroupShedules shedule)
        {
            throw new NotImplementedException();
        }

        public bool AssingStudentStatus(int userId, int groupId)
        {
            throw new NotImplementedException();
        }

        public bool AssingTeacherStatus(int userId, int lessonId, int groupId)
        {
            throw new NotImplementedException();
        }

        public Users GetUser(string login, string password)
        {
            throw new NotImplementedException();
        }

        public bool GroupControllCheck(int teacherUserId, int groupId, int lessonId)
        {
            throw new NotImplementedException();
        }

        public void UpdateSheduleInfo(GroupShedules shedule)
        {
            throw new NotImplementedException();
        }

        public void UpdateStudentAttendances(int sheduleId, int studentUserId, bool participate)
        {
            throw new NotImplementedException();
        }

        bool IUniversity.AddGroup(string Name)
        {
            throw new NotImplementedException();
        }

        bool IUniversity.AddLessonToGroup(int lessonId, int groupId)
        {
            throw new NotImplementedException();
        }

        GroupShedules IUniversity.AddShedule(GroupShedules shedule)
        {
            throw new NotImplementedException();
        }

        bool ITeacher.UpdateSheduleInfo(GroupShedules shedule)
        {
            throw new NotImplementedException();
        }
    }
}
