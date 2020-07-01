using System;
using System.Collections.Generic;
using System.Text;

namespace LessonControllerDb.Models
{
    public class StudentsGroupParticipations
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
}
