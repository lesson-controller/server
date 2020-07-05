using System;
using System.Collections.Generic;
using System.Text;

namespace LessonControllerAPI.Models.Answers
{
    public class AttendanceForStudent
    {
        public GeneralData General { get; set; }
        public class GeneralData
        {
            public decimal Procent { get; set; }
        }

        public List<ForDaysData> ForDays { get; set; }
        public class ForDaysData
        {
            public int TimeStartDay { get; set; }

            public GeneralData General { get; set; }
            public class GeneralData
            {
                public decimal Procent { get; set; }
            }
        }
    }
}
