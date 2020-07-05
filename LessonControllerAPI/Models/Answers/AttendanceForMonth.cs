using System;
using System.Collections.Generic;
using System.Text;

namespace LessonControllerAPI.Models.Answers
{
    public class AttendanceForMonth
    {
        public GeneralData General { get; set; }
        public class GeneralData
        {
            public decimal Procent { get; set; }
        }
    }
}
