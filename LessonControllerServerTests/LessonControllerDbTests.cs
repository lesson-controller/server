using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LessonControllerServer.Controllers;
using LessonControllerDb.Modules;
using System.Linq;

namespace LessonControllerServerTests
{
    [TestClass]
    public class LessonControllerDbTests
    {
        private LessonControllerDatabase Db = new LessonControllerDatabase(true);

        [TestMethod]
        public void GetLessons()
        {
            var name = "test";
            Db.CreateLesson(new LessonControllerDb.Models.Lessons() { Name = name });

            var lessons = Db.GetLessons(null);

            Assert.AreEqual(true, lessons.FirstOrDefault(x => x.Name == name) != null);
        }

        [TestMethod]
        public void AddGroup_And_GetAllStudentsGroups()
        {
            var name = "test";
            Db.AddGroup(name, "disc");

            var groups = Db.GetAllStudentsGroups();

            Assert.AreEqual(true, groups.FirstOrDefault(x => x.Name == name) != null);
        }

        [TestMethod]
        public void GetAvailableTeachers()
        {
            var name = "test";
            var lesson = Db.CreateLesson(new LessonControllerDb.Models.Lessons() { Name = name });
            Db.CreateUser(new LessonControllerDb.Models.Users() { Login = name, Password = name, FirstName = name, LastName = name, Role = null });

            var availableTeachers = Db.GetAvailableTeachers(lesson.Id);

            Assert.AreEqual(true, availableTeachers.FirstOrDefault(x => x.Login == name) != null);
        }

        [TestMethod]
        public void GetGroupData()
        {
            var name = "test";
            Db.AddGroup(name, "disc");

            var data = Db.GetGroupData(name);

            Assert.AreEqual(true, data != null);
        }

        [TestMethod]
        public void AddStudentToGroup()
        {
            var name = "test";
            var group = Db.AddGroup(name, "disc");
            var user = Db.CreateUser(new LessonControllerDb.Models.Users() { Login = name, Password = name, FirstName = name, LastName = name, Role = null });
            Db.AddStudentToGroup(user.Id, group.Id);

            var students = Db.GetStudentsList(group.Id);

            Assert.AreEqual(true, students.FirstOrDefault(x => x.Login == name) != null);
        }

        [TestMethod]
        public void GetUsersAvalilableToAddToGroup()
        {
            var name = "test";
            var group = Db.AddGroup(name, "disc");
            var user = Db.CreateUser(new LessonControllerDb.Models.Users() { Login = name, Password = name, FirstName = name, LastName = name, Role = null });

            var avalilableToAddToGroup = Db.GetUsersAvalilableToAddToGroup(group.Id);

            Assert.AreEqual(true, avalilableToAddToGroup.FirstOrDefault(x => x.Login == name) != null);
        }

        [TestMethod]
        public void AddTeacherToGroup()
        {
            var name = "test";
            var group = Db.AddGroup(name, "disc");
            var user = Db.CreateUser(new LessonControllerDb.Models.Users() { Login = name, Password = name, FirstName = name, LastName = name, Role = null });
            var lesson = Db.CreateLesson(new LessonControllerDb.Models.Lessons() { Name = name });

            Db.AddTeacherToGroup(user.Id, group.Id, lesson.Id);
            var teachers = Db.GetTeachersGroupParticipations(group.Id);

            Assert.AreEqual(true, teachers.FirstOrDefault(x => x.UserId == user.Id && x.GroupId == group.Id && x.LessonId == lesson.Id) != null);
        }

        [TestMethod]
        public void AddLessonToGroup()
        {
            var name = "test";
            var group = Db.AddGroup(name, "disc");
            var lesson = Db.CreateLesson(new LessonControllerDb.Models.Lessons() { Name = name });
            Db.AddLessonToGroup(lesson.Id, group.Id);

            var groupLessons = Db.GetGroupLessons(group.Id);

            Assert.AreEqual(true, groupLessons.FirstOrDefault(x => x.LessonId == lesson.Id) != null);
        }

        [TestMethod]
        public void AddShedule()
        {
            var name = "test";
            var group = Db.AddGroup(name, "disc");
            var lesson = Db.CreateLesson(new LessonControllerDb.Models.Lessons() { Name = name });
            var timeStart = DateTime.Now;
            var createdShedule = Db.AddShedule(new LessonControllerDb.Models.GroupShedules()
            {
                GroupId = group.Id,
                LessonId = lesson.Id,
                TimeStart = timeStart,
                TimeEnd = timeStart.AddMinutes(90)
            });

            var shedule = Db.GetGroupShedule(group.Id);

            Assert.AreEqual(true, shedule.FirstOrDefault(x => x.Id == createdShedule.Id) != null);
        }

        [TestMethod]
        public void UpdateSheduleInfo()
        {
            var name = "test";
            var group = Db.AddGroup(name, "disc");
            var lesson = Db.CreateLesson(new LessonControllerDb.Models.Lessons() { Name = name });
            var timeStart = DateTime.Now;
            var sheduleData = new LessonControllerDb.Models.GroupShedules()
            {
                GroupId = group.Id,
                LessonId = lesson.Id,
                TimeStart = timeStart,
                TimeEnd = timeStart.AddMinutes(90)
            };
            Db.AddShedule(sheduleData);

            sheduleData.Textbook = name;
            sheduleData.TextbookLink = name;
            sheduleData.Topic = name;
            sheduleData.Homework = name;
            Db.UpdateSheduleInfo(sheduleData);
            var shedule = Db.GetGroupShedule(group.Id);

            Assert.AreEqual(true, shedule.FirstOrDefault(x => 
            x.Id == sheduleData.Id &&
            x.Textbook == name &&
            x.TextbookLink == name &&
            x.Topic == name &&
            x.Homework == name) != null);
        }

        [TestMethod]
        public void GetSheduleForStudent()
        {
            var name = "test";
            var user = Db.CreateUser(new LessonControllerDb.Models.Users() { Login = name, Password = name, FirstName = name, LastName = name, Role = null });
            var group = Db.AddGroup(name, "disc");
            var lesson = Db.CreateLesson(new LessonControllerDb.Models.Lessons() { Name = name });
            var timeStart = DateTime.Now;
            var createdShedule = Db.AddShedule(new LessonControllerDb.Models.GroupShedules()
            {
                GroupId = group.Id,
                LessonId = lesson.Id,
                TimeStart = timeStart,
                TimeEnd = timeStart.AddMinutes(90)
            });

            Db.AddStudentToGroup(user.Id, group.Id);
            var shedule = Db.GetSheduleForStudent(user.Id);

            Assert.AreEqual(true, shedule.FirstOrDefault(x => x.Id == createdShedule.Id) != null);
        }

        [TestMethod]
        public void UpdateStudentAttendances()
        {
            var name = "test";
            var user = Db.CreateUser(new LessonControllerDb.Models.Users() { Login = name, Password = name, FirstName = name, LastName = name, Role = null });
            var group = Db.AddGroup(name, "disc");
            var lesson = Db.CreateLesson(new LessonControllerDb.Models.Lessons() { Name = name });
            var timeStart = DateTime.Now;
            var createdShedule = Db.AddShedule(new LessonControllerDb.Models.GroupShedules()
            {
                GroupId = group.Id,
                LessonId = lesson.Id,
                TimeStart = timeStart,
                TimeEnd = timeStart.AddMinutes(90)
            });
            Db.AddStudentToGroup(user.Id, group.Id);

            Db.UpdateStudentAttendances(createdShedule.Id, user.Id, true);
            var attendance = Db.GetStudentsAttendance(createdShedule.Id).FirstOrDefault(x => x.StudentUserId == user.Id);

            Assert.AreEqual(true, attendance != null && attendance.Participate);
        }

        [TestMethod]
        public void GetStudentAttendances()
        {
            var name = "test";
            var user = Db.CreateUser(new LessonControllerDb.Models.Users() { Login = name, Password = name, FirstName = name, LastName = name, Role = null });
            var group = Db.AddGroup(name, "disc");
            var lesson = Db.CreateLesson(new LessonControllerDb.Models.Lessons() { Name = name });
            var timeStart = DateTime.Now;
            var createdShedule = Db.AddShedule(new LessonControllerDb.Models.GroupShedules()
            {
                GroupId = group.Id,
                LessonId = lesson.Id,
                TimeStart = timeStart,
                TimeEnd = timeStart.AddMinutes(90)
            });
            Db.AddStudentToGroup(user.Id, group.Id);

            Db.UpdateStudentAttendances(createdShedule.Id, user.Id, true);
            var attendance = Db.GetStudentAttendances(user.Id);

            Assert.AreEqual(true, attendance != null && attendance.Count == 1 && attendance.FirstOrDefault(x => x.SheduleId == createdShedule.Id).Participate == true);
        }

        [TestMethod]
        public void GetStudentsAttendances()
        {
            var name = "test";
            var name2 = "test2";
            var user1 = Db.CreateUser(new LessonControllerDb.Models.Users() { Login = name, Password = name, FirstName = name, LastName = name, Role = null });
            var user2 = Db.CreateUser(new LessonControllerDb.Models.Users() { Login = name2, Password = name, FirstName = name, LastName = name, Role = null });
            var group = Db.AddGroup(name, "disc");
            var lesson = Db.CreateLesson(new LessonControllerDb.Models.Lessons() { Name = name });
            var timeStart = DateTime.Now;
            var createdShedule = Db.AddShedule(new LessonControllerDb.Models.GroupShedules()
            {
                GroupId = group.Id,
                LessonId = lesson.Id,
                TimeStart = timeStart,
                TimeEnd = timeStart.AddMinutes(90)
            });
            Db.AddStudentToGroup(user1.Id, group.Id);
            Db.AddStudentToGroup(user2.Id, group.Id);

            Db.UpdateStudentAttendances(createdShedule.Id, user1.Id, true);
            Db.UpdateStudentAttendances(createdShedule.Id, user2.Id, false);
            var attendance = Db.GetStudentsAttendances(group.Id);

            Assert.AreEqual(true, 
                attendance != null && 
                attendance.Count == 2 && 
                attendance.FirstOrDefault(x => x.SheduleId == createdShedule.Id && x.StudentUserId == user1.Id).Participate == true &&
                attendance.FirstOrDefault(x => x.SheduleId == createdShedule.Id && x.StudentUserId == user2.Id).Participate == false);
        }
    }
}
