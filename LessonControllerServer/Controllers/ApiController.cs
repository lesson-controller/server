using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LessonControllerAPI.Models;
using LessonControllerDb.API;
using LessonControllerDb.Models;
using LessonControllerDb.Modules;
using LessonControllerServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LessonControllerServer.Controllers
{
    public class ApiController : Controller
    {
        private LessonControllerDatabase Db => new LessonControllerDatabase();
        private Users DbUser => Db.GetUser(User.Identity.Name, null);
        private LessonControllerAPI.API.Interfaces.IStudent Student => new Student(DbUser, Db);
        private LessonControllerAPI.API.Interfaces.ITeacher Teacher => new Teacher(DbUser, Db);
        private LessonControllerAPI.API.Interfaces.IUniversity University => new University(DbUser, Db);


        #region Auth

        [HttpPost]
        public IActionResult Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
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

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            if (DbUser != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, DbUser.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, DbUser.Role)
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

        #region Teacher

        public JsonResult UpdateSheduleInfo(GroupShedules shedule)
        {
            var answer = new ApiAnswer<bool>();

            answer.Data = Teacher.UpdateSheduleInfo(shedule);

            return new JsonResult(answer);
        }

        #endregion

        #region University

        public JsonResult AddGroup(string Name)
        {
            var answer = new ApiAnswer<bool>();

            answer.Data = University.AddGroup(Name);

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

        public JsonResult AddLessonToGroup(int lessonId, int groupId)
        {
            var answer = new ApiAnswer<bool>();

            answer.Data = University.AddLessonToGroup(lessonId, groupId);

            return new JsonResult(answer);
        }

        [HttpPost]
        public JsonResult AddShedule(GroupShedules shedule)
        {
            var answer = new ApiAnswer<GroupShedules>();

            answer.Data = University.AddShedule(shedule);

            return new JsonResult(answer);
        }
        

        #endregion
    }
}