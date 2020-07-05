using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.Models;

namespace LessonControllerDb.API.Interfaces
{
    public interface IUniversity
    {
        Users CreateUser(Users data);
        List<Users> GetUsers();
        Lessons CreateLesson(Lessons data);
        List<Lessons> GetLessons(int? groupId = null);
        List<Users> GetAvailableTeachers(int lessonId);
        List<GroupShedules> GetGlobalShedule();

        #region Group
        List<StudentGroups> GetAllStudentsGroups();
        StudentGroups AddGroup(string name, string discription);
        StudentGroups GetGroupData(string groupName);

        #region Student
        bool AddStudentToGroup(int userId, int groupId);
        List<Users> GetUsersAvalilableToAddToGroup(int groupId);
        List<Users> GetStudentsList(int groupId);
        #endregion

        #region Teacher
        bool AddTeacherToGroup(int userId, int groupId, int lessonId);
        List<TeachersGroupParticipations> GetTeachersGroupParticipations(int groupId);
        #endregion

        #region Lesson
        bool AddLessonToGroup(int lessonId, int groupId);
        List<GroupLessons> GetGroupLessons(int groupId);
        #endregion

        #region Shedule
        GroupShedules AddShedule(GroupShedules shedule);
        List<GroupShedules> GetGroupShedule(int groupId);
        bool UpdateSheduleInfo(GroupShedules shedule);
        List<GroupShedules> GetSheduleForStudent(int userId);
        #endregion

        #region Attendance
        bool UpdateStudentAttendances(int sheduleId, int studentUserId, bool participate);
        List<StudentAttendances> GetStudentsAttendance(int sheduleId);
        List<StudentAttendances> GetStudentAttendances(int userId);
        List<StudentAttendances> GetStudentsAttendances(int? groupId = null);
        #endregion

        #endregion

        bool AssingStudentStatus(int userId, int groupId);
        bool AssingTeacherStatus(int userId, int lessonId, int groupId);        
    }
}
