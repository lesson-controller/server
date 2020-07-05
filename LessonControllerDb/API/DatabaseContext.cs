using Microsoft.EntityFrameworkCore;
using LessonControllerDb.Models;

namespace LessonControllerDb.API
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql($"server=109.195.85.22;port=3306;database=lesson-controller;UserId=root;Password=Lomal31032000");
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
