using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LessonControllerAPI.Models;
using LessonControllerAPI.Models.Answers;
using LessonControllerDb.API;
using LessonControllerDb.Models;
using LessonControllerDb.Modules;
using LessonControllerServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LessonControllerServer.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ApiController : Controller
    {
        private LessonControllerDatabase Db => new LessonControllerDatabase(false);
        private Users DbUser => (User != null && User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name)) ?
            Db.GetUserWithoutPassword(User.Identity.Name) : null;
        private bool IsAdmin => DbUser.Role == "admin";
        private LessonControllerAPI.API.Interfaces.IStudent Student => new Student(DbUser, Db);
        private LessonControllerAPI.API.Interfaces.ITeacher Teacher => new Teacher(DbUser, Db);
        private LessonControllerAPI.API.Interfaces.IUniversity University => new University(DbUser, Db);


        #region Auth

        [HttpPost]
        public JsonResult Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return new JsonResult(new ApiAnswer<bool>() { Data = false, Error = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            var answer = new ApiAnswer<object>();

            answer.Data = response;

            return new JsonResult(answer);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = Db.GetUser(username, password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        #endregion

        #region User
        public JsonResult GetUserData()
        {
            var answer = new ApiAnswer<WebUserDataAnswer>();

            if (DbUser != null)
            {
                answer.Data = new WebUserDataAnswer()
                {
                    FirstName = DbUser.FirstName,
                    LastName = DbUser.LastName,
                    IsAdmin = this.IsAdmin,
                    studentGroups = Student.GetGroupsOfWhich(),
                    teacherGroups = Teacher.GetGroupsRunning()
                };
            }
            else
            {
                answer.Error = "Error";
            }
            

            return new JsonResult(answer);
        }

        #endregion

        #region Teacher


        #endregion

        #region University

        public JsonResult AddGroup(string name, string discription)
        {
            var answer = new ApiAnswer<bool>();

            answer.Data = University.AddGroup(name, discription) != null;

            return new JsonResult(answer);
        }

        public JsonResult AssingStudentStatus(int userId, int groupId)
        {
            var answer = new ApiAnswer<bool>();

            answer.Data = University.AssingStudentStatus(userId, groupId);

            return new JsonResult(answer);
        }

        public JsonResult AssingTeacherStatus(int userId, int lessonId, int groupId)
        {
            var answer = new ApiAnswer<bool>();

            answer.Data = University.AssingTeacherStatus(userId, lessonId, groupId);

            return new JsonResult(answer);
        }

        [HttpPost]
        public JsonResult AddLessonToGroup(int groupId, int lessonId)
        {
            var answer = new ApiAnswer<bool>();

            answer.Data = University.AddLessonToGroup(lessonId, groupId);

            return new JsonResult(answer);
        }

        [HttpPost]
        public JsonResult AddShedule([FromBody]WebAddShedule data)
        {
            var answer = new ApiAnswer<GroupShedules>();

            answer.Data = University.AddShedule(new GroupShedules()
            {
                GroupId = data.groupId,
                LessonId = data.lessonId,
                TimeStart = new DateTime(1970, 1, 1).AddSeconds(data.timeStart),
                TimeEnd = new DateTime(1970, 1, 1).AddSeconds(data.timeStart).AddMinutes(90),
                Canceled = false
            });

            return new JsonResult(answer);
        }

        public JsonResult GetAllStudentsGroups()
        {
            var answer = new ApiAnswer<List<StudentGroups>>();

            answer.Data = University.GetAllStudentsGroups();

            return new JsonResult(answer);
        }

        public JsonResult GetStudentsList(int groupId)
        {
            var answer = new ApiAnswer<List<Users>>();

            answer.Data = University.GetStudentsList(groupId);

            return new JsonResult(answer);
        }

        public JsonResult GetUsersAvalilableToAddToGroup(int groupId)
        {
            var answer = new ApiAnswer<List<Users>>();

            answer.Data = University.GetUsersAvalilableToAddToGroup(groupId);

            return new JsonResult(answer);
        }

        public JsonResult GetTeachersList(int groupId)
        {
            var answer = new ApiAnswer<GroupTeachersListsForLessons>();

            var data =  University.GetTeachersList(groupId);

            if (IsAdmin)
            {
                University.GetAvailableTeachers(ref data);
            }

            answer.Data = data;

            return new JsonResult(answer);
        }

        public JsonResult GetGroupData(string groupName)
        {
            var answer = new ApiAnswer<StudentGroups>();

            answer.Data = University.GetGroupData(groupName);

            return new JsonResult(answer);
        }

        public JsonResult GetLessons()
        {
            var answer = new ApiAnswer<List<Lessons>>();

            answer.Data = University.GetLessons();

            return new JsonResult(answer);
        }

        public JsonResult GetLessonsForGroup(int groupId)
        {
            var answer = new ApiAnswer<List<Lessons>>();

            answer.Data = University.GetLessons(groupId);

            return new JsonResult(answer);
        }

        [HttpPost]
        public JsonResult AddTeacherToGroup(int userId, int groupId, int lessonId)
        {
            var answer = new ApiAnswer<bool>();

            answer.Data = University.AddTeacherToGroup(userId, groupId, lessonId);

            return new JsonResult(answer);
        }

        [HttpPost]
        public JsonResult AddStudentToGroup(int userId, int groupId)
        {
            var answer = new ApiAnswer<bool>();

            answer.Data = University.AddStudentToGroup(userId, groupId);

            return new JsonResult(answer);
        }

        public JsonResult GetGroupShedule(int groupId)
        {
            var answer = new ApiAnswer<GroupShedulesData>();

            answer.Data = University.GetGroupSheduleData(groupId);

            return new JsonResult(answer);
        }

        [HttpPost]
        public JsonResult UpdateStudentAttendances(int sheduleId, int studentUserId, bool participate)
        {
            var answer = new ApiAnswer<bool>();

            answer.Data = University.UpdateStudentAttendances(sheduleId, studentUserId, participate);

            return new JsonResult(answer);
        }

        [HttpPost]
        public JsonResult UpdateSheduleInfo([FromBody] GroupShedules data)
        {
            var answer = new ApiAnswer<bool>();

            answer.Data = University.UpdateSheduleInfo(data);

            return new JsonResult(answer);
        }

        public JsonResult GetAttendanceForGroup(int groupId)
        {
            var answer = new ApiAnswer<AttendanceForGroup>();

            answer.Data = University.GetAttendanceForGroup(groupId);

            return new JsonResult(answer);
        }

        public JsonResult GetAttendanceForStudent(int userId, int? groupId)
        {
            var answer = new ApiAnswer<AttendanceForStudent>();

            answer.Data = University.GetAttendanceForStudent(userId, groupId);

            return new JsonResult(answer);
        }

        public JsonResult GetAttendanceForDay(int timeStartDay, int? groupId)
        {
            var answer = new ApiAnswer<AttendanceForDay>();

            answer.Data = University.GetAttendanceForDay(timeStartDay, groupId);

            return new JsonResult(answer);
        }

        public JsonResult GetAttendanceForMonth(int timeStartMonth, int? groupId)
        {
            var answer = new ApiAnswer<AttendanceForMonth>();

            answer.Data = University.GetAttendanceForMonth(timeStartMonth, groupId);

            return new JsonResult(answer);
        }

        #endregion
    }
}