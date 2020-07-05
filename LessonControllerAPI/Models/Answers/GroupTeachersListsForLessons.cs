using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.Models;

namespace LessonControllerAPI.Models.Answers
{
    public class GroupTeachersListsForLessons
    {
        public List<ByLesson> Lessons { get; set; }
        public class ByLesson
        {
            public int LessonId { get; set; }
            public List<Users> Teachers { get; set; }
            public List<Users> AvailableTeachers { get; set; }
            public ByLesson()
            {
                Teachers = new List<Users>();
            }
        }

        public GroupTeachersListsForLessons()
        {
            Lessons = new List<ByLesson>();
        }
    }
}
