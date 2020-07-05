using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.Models;

namespace LessonControllerDb.API.Interfaces
{
    public interface IUniversity
    {
        bool AddGroup(string name, string discription);
        bool AssingStudentStatus(int userId, int groupId);
        bool AssingTeacherStatus(int userId, int lessonId, int groupId);
        bool AddLessonToGroup(int lessonId, int groupId);
        GroupShedules AddShedule(GroupShedules shedule);
        bool UpdateStudentAttendances(int sheduleId, int studentUserId, bool participate);
        bool AddStudentToGroup(int userId, int groupId);
        List<Users> GetUsersAvalilableToAddToGroup(int groupId);
        List<Users> GetTeachersAvalilableToAddToGroup(int groupId);
        List<StudentGroups> GetAllStudentsGroups();
        List<Users> GetStudentsList(int groupId);
        List<TeachersGroupParticipations> GetTeachersGroupParticipations(int groupId);
        List<Users> GetUsers();
        StudentGroups GetGroupData(string groupName);
        List<Lessons> GetLessons(int? groupId = null);
        List<GroupLessons> GetGroupLessons(int groupId);
        List<Users> GetAvailableTeachers(int lessonId);
        bool AddTeacherToGroup(int userId, int groupId, int lessonId);
        List<GroupShedules> GetGroupShedule(int groupId);
        List<StudentAttendances> GetStudentsAttendance(int sheduleId);
        List<StudentAttendances> GetStudentAttendances(int userId);
        List<StudentAttendances> GetStudentsAttendances(int? groupId = null);
        bool UpdateSheduleInfo(GroupShedules shedule);
        List<GroupShedules> GetSheduleForStudent(int userId);
        List<GroupShedules> GetGlobalShedule();
    }
}
