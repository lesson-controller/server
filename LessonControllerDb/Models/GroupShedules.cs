using System;
using System.Collections.Generic;
using System.Text;

namespace LessonControllerDb.Models
{
    public class GroupShedules
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int LessonId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public bool Canceled { get; set; }
    }
}
