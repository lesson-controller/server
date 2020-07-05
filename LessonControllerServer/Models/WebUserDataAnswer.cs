using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LessonControllerDb.Models;

namespace LessonControllerServer.Models
{
    public class WebUserDataAnswer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public List<StudentGroups> studentGroups { get; set; }
        public List<StudentGroups> teacherGroups { get; set; }
    }
}
