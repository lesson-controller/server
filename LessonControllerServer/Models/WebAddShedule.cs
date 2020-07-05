using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LessonControllerServer.Models
{
    public class WebAddShedule
    {
        public int groupId { get; set; }
        public int lessonId { get; set; }
        public int timeStart { get; set; }
    }
}
