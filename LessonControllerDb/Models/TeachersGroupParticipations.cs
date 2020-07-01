using System;
using System.Collections.Generic;
using System.Text;

namespace LessonControllerDb.Models
{
    public class TeachersGroupParticipations
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
}
