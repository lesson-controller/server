using System;
using System.Collections.Generic;
using System.Text;

namespace LessonControllerAPI.Models.Answers
{
    public class AttendanceForDay
    {
        public GeneralData General { get; set; }
        public class GeneralData
        {
            public decimal Procent { get; set; }
        }

        public List<ForStudentsData> ForStudents { get; set; }
        public class ForStudentsData
        {
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public GeneralData General { get; set; }
            public class GeneralData
            {
                public int CountLessons { get; set; }
                public int CountParticipatingLessons { get; set; }
                public decimal Procent { get; set; }
            }
        }
    }
}
