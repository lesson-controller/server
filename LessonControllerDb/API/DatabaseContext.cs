using Microsoft.EntityFrameworkCore;
using LessonControllerDb.Models;

namespace LessonControllerDb.API
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }
        public DatabaseContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Lessons> Lessons { get; set; }
        public DbSet<StudentGroups> StudentGroups { get; set; }
        public DbSet<StudentsGroupParticipations> StudentsGroupParticipations { get; set; }
        public DbSet<TeachersGroupParticipations> TeachersGroupParticipations { get; set; }
        public DbSet<GroupShedules> GroupShedules { get; set; }
        public DbSet<GroupLessons> GroupLessons { get; set; }
        public DbSet<StudentAttendances> StudentAttendances { get; set; }
    }
}
