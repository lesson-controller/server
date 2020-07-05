using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerDb.API;
using LessonControllerDb.API.Interfaces;
using LessonControllerDb.Models;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace LessonControllerDb.Modules
{
    public class LessonControllerDatabase : API.Database,
        API.Interfaces.IGeneral,
        API.Interfaces.IStudent,
        API.Interfaces.ITeacher,
        API.Interfaces.IUniversity,
        API.Interfaces.IStudentAttendances
    {
        private DbContextOptionsBuilder<DatabaseContext> builder =
            new DbContextOptionsBuilder<DatabaseContext>()
                .UseMySql($"server=109.195.85.22;port=3306;database=lesson-controller;UserId=root;Password=Lomal31032000");
        private DbContextOptionsBuilder<DatabaseContext> testBuilder = 
            new DbContextOptionsBuilder<DatabaseContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
        private bool test;

        public LessonControllerDatabase(bool test)
        {
            this.test = test;
        }

        private DatabaseContext CreateDbContext() => test ? 
            new DatabaseContext(testBuilder.Options) : 
            new DatabaseContext(builder.Options);

        public StudentGroups AddGroup(string name, string discription)
        {
            
            using (var db = CreateDbContext())
            {
                var copy = db.StudentGroups.FirstOrDefault(x => x.Name == name);
                if (copy != null) return null;

                var data = new StudentGroups() { Name = name, Discription = discription };
                db.StudentGroups.Add(data);
                db.SaveChanges();
                return data;
            }
        }

        public bool AddLessonToGroup(int lessonId, int groupId)
        {
            using (var db = CreateDbContext())
            {
                var copy = db.GroupLessons.FirstOrDefault(x => x.LessonId == lessonId && x.GroupId == groupId);
                if (copy != null) return false;

                db.GroupLessons.Add(new GroupLessons() { LessonId = lessonId, GroupId = groupId });
                db.SaveChanges();
                return true;
            }
        }

        public GroupShedules AddShedule(GroupShedules shedule)
        {
            using (var db = CreateDbContext())
            {
                db.GroupShedules.Add(shedule);
                db.SaveChanges();
                return shedule;
            }
        }

        public bool AddStudentToGroup(int userId, int groupId)
        {
            using (var db = CreateDbContext())
            {
                var copy = db.StudentsGroupParticipations.FirstOrDefault(x => x.GroupId == groupId && x.UserId == userId);
                if (copy != null) return false;

                db.StudentsGroupParticipations.Add(new StudentsGroupParticipations() { GroupId = groupId, UserId = userId });
                db.SaveChanges();
                return true;
            }
        }

        public bool AddTeacherToGroup(int userId, int groupId, int lessonId)
        {
            using (var db = CreateDbContext())
            {
                var copy = db.TeachersGroupParticipations.FirstOrDefault(x => x.GroupId == groupId && x.UserId == userId && x.LessonId == lessonId);
                if (copy != null) return false;

                db.TeachersGroupParticipations.Add(new TeachersGroupParticipations() { GroupId = groupId, UserId = userId, LessonId = lessonId });
                db.SaveChanges();
                return true;
            }
        }

        public bool AssingStudentStatus(int userId, int groupId)
        {
            throw new NotImplementedException();
        }

        public bool AssingTeacherStatus(int userId, int lessonId, int groupId)
        {
            throw new NotImplementedException();
        }

        public List<StudentGroups> GetAllStudentsGroups()
        {
            using (var db = CreateDbContext())
            {
                return db.StudentGroups.ToList();
            }
        }

        public List<Users> GetAvailableTeachers(int lessonId)
        {
            using (var db = CreateDbContext())
            {
                return db.Users.ToList();
            }
        }

        public List<GroupShedules> GetGlobalShedule()
        {
            using (var db = CreateDbContext())
            {
                return db.GroupShedules.OrderBy(x => x.TimeStart).ToList();
            }
        }

        public StudentGroups GetGroupData(string groupName)
        {
            using (var db = CreateDbContext())
            {
                return db.StudentGroups.FirstOrDefault(x => x.Name == groupName);
            }
        }

        public List<GroupLessons> GetGroupLessons(int groupId)
        {
            using (var db = CreateDbContext())
            {
                return db.GroupLessons.Where(x => x.GroupId == groupId).ToList();
            }
        }

        public List<GroupShedules> GetGroupShedule(int groupId)
        {
            using (var db = CreateDbContext())
            {
                return db.GroupShedules.Where(x => x.GroupId == groupId).OrderBy(x => x.TimeStart).ToList();
            }
        }

        public List<StudentGroups> GetGroupsOfWhich(int userId)
        {
            using (var db = CreateDbContext())
            {
                return (from sgp in db.StudentsGroupParticipations
                        where sgp.UserId == userId
                        join g in db.StudentGroups on sgp.GroupId equals g.Id
                        select g).ToList();
            }
        }

        public List<StudentGroups> GetGroupsRunning(int teacherUserId)
        {
            using (var db = CreateDbContext())
            {
                return (from tgp in db.TeachersGroupParticipations
                        where tgp.UserId == teacherUserId
                        join g in db.StudentGroups on tgp.GroupId equals g.Id
                        select g).ToList();
            }
        }

        public List<Lessons> GetLessons(int? groupId)
        {
            using (var db = CreateDbContext())
            {
                if (groupId == null)
                    return db.Lessons.ToList();
                else
                    return (from gl in db.GroupLessons
                            join l in db.Lessons on gl.LessonId equals l.Id
                            select l).ToList();
            }
        }

        public List<GroupShedules> GetSheduleForStudent(int userId)
        {
            using (var db = CreateDbContext())
            {
                var groups = db.StudentsGroupParticipations.Where(x => x.UserId == userId).Select(x => x.GroupId).ToList();
                return db.GroupShedules.Where(x => groups.Contains(x.GroupId)).OrderBy(x => x.TimeStart).ToList();
            }
        }

        public List<StudentAttendances> GetStudentAttendances(int userId)
        {
            using (var db = CreateDbContext())
            {
                return db.StudentAttendances.Where(x => x.StudentUserId == userId).ToList();
            }
        }

        public List<StudentAttendances> GetStudentsAttendance(int sheduleId)
        {
            using (var db = CreateDbContext())
            {
                return db.StudentAttendances.Where(x => x.SheduleId == sheduleId).ToList();
            }
        }

        public List<StudentAttendances> GetStudentsAttendances(int? groupId = null)
        {
            using (var db = CreateDbContext())
            {
                if (groupId == null)
                    return db.StudentAttendances.ToList();
                else
                {
                    var shedules = db.GroupShedules.Where(x => x.GroupId == groupId).Select(x => x.Id);
                    return db.StudentAttendances.Where(x => shedules.Contains(x.SheduleId)).ToList();
                }
            }
        }

        public List<Users> GetStudentsList(int groupId)
        {
            using (var db = CreateDbContext())
            {
                return (from sgp in db.StudentsGroupParticipations
                        where sgp.GroupId == groupId
                        join u in db.Users on sgp.UserId equals u.Id
                        select u).ToList();
            }
        }

        public List<TeachersGroupParticipations> GetTeachersGroupParticipations(int groupId)
        {
            using (var db = CreateDbContext())
            {
                return db.TeachersGroupParticipations.Where(x => x.GroupId == groupId).ToList();
            }
        }

        public Users GetUser(string login, string password)
        {
            using(var db = CreateDbContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Login == login);
                if (user != null && SecurePasswordHasher.Verify(password, user.Password))
                    return user;
                return null;
            }
        }

        public List<Users> GetUsers()
        {
            using (var db = CreateDbContext())
            {
                return db.Users.ToList();
            }
        }

        public List<Users> GetUsersAvalilableToAddToGroup(int groupId)
        {
            using (var db = CreateDbContext())
            {
                var answer = new List<Users>();

                var users = db.Users.ToList();

                foreach (var x in users)
                {
                    var copy = db.StudentsGroupParticipations.FirstOrDefault(g => g.GroupId == groupId && g.UserId == x.Id);
                    if (copy == null)
                    {
                        answer.Add(x);
                    }
                }

                return answer;
            }
        }

        public Users GetUserWithoutPassword(string login)
        {
            using (var db = CreateDbContext())
            {
                return db.Users.FirstOrDefault(x => x.Login == login);
            }
        }

        public bool GroupControllCheck(int teacherUserId, int groupId, int lessonId)
        {
            using (var db = CreateDbContext())
            {
                return db.TeachersGroupParticipations.FirstOrDefault(x => x.GroupId == groupId && x.LessonId == lessonId && x.UserId == teacherUserId) != null;
            }
        }


        public bool UpdateSheduleInfo(GroupShedules shedule)
        {
            using (var db = CreateDbContext())
            {
                var copy = db.GroupShedules.FirstOrDefault(x => x.Id == shedule.Id);
                if (copy == null) return false;

                copy.Canceled = shedule.Canceled;
                copy.Textbook = shedule.Textbook;
                copy.TextbookLink = shedule.TextbookLink;
                copy.Topic = shedule.Topic;
                copy.Homework = shedule.Homework;

                db.SaveChanges();
                return true;
            }
        }

        public bool UpdateStudentAttendances(int sheduleId, int studentUserId, bool participate)
        {
            using (var db = CreateDbContext())
            {
                var copy = db.StudentAttendances.FirstOrDefault(x => x.SheduleId == sheduleId && x.StudentUserId == studentUserId);
                if (copy == null)
                {
                    db.StudentAttendances.Add(new StudentAttendances()
                    {
                        SheduleId = sheduleId,
                        StudentUserId = studentUserId,
                        Participate = participate
                    });                  
                }
                else
                {
                    copy.Participate = participate;
                }

                db.SaveChanges();
                return true;
            }
        }

        public Users CreateUser(Users data)
        {
            using (var db = CreateDbContext())
            {
                var copy = db.Users.FirstOrDefault(x => x.Login == data.Login);
                if (copy != null)
                    return null;
                data.Password = SecurePasswordHasher.Hash(data.Password);

                db.Users.Add(data);
                db.SaveChanges();

                return data;
            }
        }

        public Lessons CreateLesson(Lessons data)
        {
            using (var db = CreateDbContext())
            {
                var copy = db.Lessons.FirstOrDefault(x => x.Name == data.Name);
                if (copy != null)
                    return null;

                db.Lessons.Add(data);
                db.SaveChanges();

                return data;
            }
        }
    }
}
