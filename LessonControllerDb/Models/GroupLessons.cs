using System;
using System.Collections.Generic;
using System.Text;

namespace LessonControllerDb.Models
{
    /// <summary>
    /// Список актуальных занятий для каждой группы
    /// </summary>
    public class GroupLessons
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public int GroupId { get; set; }
    }
}
