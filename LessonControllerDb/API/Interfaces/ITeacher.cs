using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.Models;

namespace LessonControllerDb.API.Interfaces
{
    public interface ITeacher : IStudentAttendances
    {
        bool UpdateSheduleInfo(GroupShedules shedule);
        /// <summary>
        /// Список идентификаторов групп под управлением
        /// </summary>
        /// <returns></returns>
        //List<int> GetListOfGroupsRunning(int? lessonId);

        bool GroupControllCheck(int teacherUserId, int groupId, int lessonId);
    }
}
