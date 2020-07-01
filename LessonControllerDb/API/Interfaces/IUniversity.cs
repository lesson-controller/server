using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.Models;

namespace LessonControllerDb.API.Interfaces
{
    public interface IUniversity
    {
        bool AddGroup(string Name);
        bool AssingStudentStatus(int userId, int groupId);
        bool AssingTeacherStatus(int userId, int lessonId, int groupId);
        bool AddLessonToGroup(int lessonId, int groupId);
        GroupShedules AddShedule(GroupShedules shedule);
        void UpdateStudentAttendances(int sheduleId, int studentUserId, bool participate);
    }
}
