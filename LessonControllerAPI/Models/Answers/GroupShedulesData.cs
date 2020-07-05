using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.Models;

namespace LessonControllerAPI.Models.Answers
{
    public class GroupShedulesData
    {
        public List<SheduleData> Shedules { get; set; }
        public class SheduleData
        {
            public GroupShedules Data { get; set; }
            public List<StudentAttendance> StudentsAttendances { get; set; }
            public bool SubjectToChange { get; set; }
            public class StudentAttendance
            {
                public int UserId { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public bool Attendance { get; set; }
            }
        }        
    }
}
