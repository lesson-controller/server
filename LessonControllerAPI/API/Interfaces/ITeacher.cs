using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.Models;

namespace LessonControllerAPI.API.Interfaces
{
    public interface ITeacher
    {
        bool UpdateSheduleInfo(GroupShedules shedule);
    }
}
