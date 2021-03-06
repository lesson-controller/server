﻿using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.Models;

namespace LessonControllerDb.API.Interfaces
{
    public interface IStudent : IStudentAttendances
    {
        List<StudentGroups> GetGroupsOfWhich(int userId);
    }
}
