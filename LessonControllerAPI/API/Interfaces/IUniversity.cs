using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerAPI.Models.Answers;
using LessonControllerDb.Models;

namespace LessonControllerAPI.API.Interfaces
{
    public interface IUniversity
    {
        StudentGroups AddGroup(string name, string discription);
        bool AssingStudentStatus(int userId, int groupId);
        bool AssingTeacherStatus(int userId, int lessonId, int groupId);
        bool AddLessonToGroup(int lessonId, int groupId);
        GroupShedules AddShedule(GroupShedules shedule);
        bool UpdateStudentAttendances(int sheduleId, int studentUserId, bool participate);
        bool AddStudentToGroup(int userId, int groupId);
        List<Users> GetUsersAvalilableToAddToGroup(int groupId);
        List<StudentGroups> GetAllStudentsGroups();
        List<Users> GetStudentsList(int groupId);
        GroupTeachersListsForLessons GetTeachersList(int groupId);
        void GetAvailableTeachers(ref GroupTeachersListsForLessons data);
        StudentGroups GetGroupData(string groupName);
        List<Lessons> GetLessons(int? groupId = null);
        bool AddTeacherToGroup(int userId, int groupId, int lessonId);
        GroupShedulesData GetGroupSheduleData(int groupId);
        bool UpdateSheduleInfo(GroupShedules shedule);
        AttendanceForStudent GetAttendanceForStudent(int userId, int? groupId);
        AttendanceForGroup GetAttendanceForGroup(int groupId);
        AttendanceForDay GetAttendanceForDay(int timeStartDay, int? groupId);
        AttendanceForMonth GetAttendanceForMonth(int timeStartMonth, int? groupId);
    }
}
