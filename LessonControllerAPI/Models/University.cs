using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.Models;

namespace LessonControllerAPI.Models
{
    public class University : Role<LessonControllerDb.API.Interfaces.IUniversity>,
        API.Interfaces.IUniversity
    {
        public University(Users User, LessonControllerDb.API.Interfaces.IUniversity UniversityDb) : base(User, UniversityDb)
        {
        }

        public bool AddGroup(string Name)
            => Db.AddGroup(Name);

        public bool AddLessonToGroup(int lessonId, int groupId)
            => Db.AddLessonToGroup(lessonId, groupId);

        public GroupShedules AddShedule(GroupShedules shedule)
            => Db.AddShedule(shedule);

        public bool AssingStudentStatus(int userId, int groupId)
            => Db.AssingStudentStatus(userId, groupId);

        public bool AssingTeacherStatus(int userId, int lessonId, int groupId)
            => Db.AssingTeacherStatus(userId, lessonId, groupId);

        public void UpdateStudentAttendances(int sheduleId, int studentUserId, bool participate)
            => Db.UpdateStudentAttendances(sheduleId, studentUserId, participate);
    }
}
