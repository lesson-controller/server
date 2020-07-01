using System;
using System.Collections.Generic;
using System.Text;

namespace LessonControllerDb.Models
{
    public class StudentAttendances
    {
        public int Id { get; set; }
        public int SheduleId { get; set; }
        public int StudentUserId { get; set; }
        public bool Participate { get; set; }
    }
}
