using System;
using System.Collections.Generic;
using System.Text;
using LessonControllerAPI.Models.Answers;
using LessonControllerDb.Models;
using System.Linq;

namespace LessonControllerAPI.Models
{
    public class University : Role<LessonControllerDb.API.Interfaces.IUniversity>,
        API.Interfaces.IUniversity
    {
        public University(Users User, LessonControllerDb.API.Interfaces.IUniversity UniversityDb) : base(User, UniversityDb)
        {
        }

        public StudentGroups AddGroup(string name, string discription)
            => Db.AddGroup(name, discription);

        public bool AddLessonToGroup(int lessonId, int groupId)
            => Db.AddLessonToGroup(lessonId, groupId);

        public GroupShedules AddShedule(GroupShedules shedule)
            => Db.AddShedule(shedule);

        public bool AddStudentToGroup(int userId, int groupId)
            => Db.AddStudentToGroup(userId, groupId);

        public bool AddTeacherToGroup(int userId, int groupId, int lessonId)
            => Db.AddTeacherToGroup(userId, groupId, lessonId);

        public bool AssingStudentStatus(int userId, int groupId)
            => Db.AssingStudentStatus(userId, groupId);

        public bool AssingTeacherStatus(int userId, int lessonId, int groupId)
            => Db.AssingTeacherStatus(userId, lessonId, groupId);

        public List<StudentGroups> GetAllStudentsGroups()
            => Db.GetAllStudentsGroups();

        public AttendanceForDay GetAttendanceForDay(int timeStartDay, int? groupId)
        {
            var answer = new AttendanceForDay();

            var shedule = (groupId == null) ? Db.GetGlobalShedule() : Db.GetGroupShedule((int)groupId);
            var dateStartDay = new DateTime(1970, 1, 1).AddSeconds(timeStartDay);
            var validShedule = shedule.Where(x => !x.Canceled && x.TimeStart.AddHours(3) >= dateStartDay && x.TimeStart.AddHours(3) < dateStartDay.AddDays(1));
            var sheduleByGroup = validShedule.GroupBy(x => x.GroupId);

            var forStudents = new List<AttendanceForDay.ForStudentsData>();

            foreach (var gs in sheduleByGroup)
            {
                var students = Db.GetStudentsList(gs.Key);
                var attendance = Db.GetStudentsAttendances(gs.Key);

                foreach (var s in students)
                {
                    var contains = true;
                    var sudentAttendance = forStudents.FirstOrDefault(x => x.UserId == s.Id);
                    if (sudentAttendance == null)
                    {
                        sudentAttendance = new AttendanceForDay.ForStudentsData()
                        {
                            UserId = s.Id,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            General = new AttendanceForDay.ForStudentsData.GeneralData()
                        };
                        contains = false;
                    }
                    sudentAttendance.General.CountLessons += gs.Count();
                    sudentAttendance.General.CountParticipatingLessons += (from p in attendance
                                                                          where p.Participate && s.Id == p.StudentUserId 
                                                                          && gs.FirstOrDefault(x => x.Id == p.SheduleId) != null
                                                                          select 1).Count();
                    sudentAttendance.General.Procent = sudentAttendance.General.CountParticipatingLessons / sudentAttendance.General.CountLessons;

                    if (!contains)
                        forStudents.Add(sudentAttendance);
                }
            }

            var allCountLessons = (forStudents.Count > 0) ? forStudents.Sum(x => x.General.CountLessons) : 0;
            var allCountParticipatingLessons = (forStudents.Count > 0) ? forStudents.Sum(x => x.General.CountParticipatingLessons) : 0;

            answer.General = new AttendanceForDay.GeneralData()
            {
                Procent = (allCountLessons > 0) ? (allCountParticipatingLessons / allCountLessons) : 1
            };
            answer.ForStudents = forStudents;
            
            return answer;
        }

        public AttendanceForGroup GetAttendanceForGroup(int groupId)
        {
            var answer = new AttendanceForGroup();

            var shedule = Db.GetGroupShedule(groupId);
            var attendance = Db.GetStudentsAttendances(groupId);
            double countStudents = Db.GetStudentsList(groupId).Count;

            var validShedule = shedule.Where(x => !x.Canceled).ToList();
            double participating = (from s in validShedule
                                 from p in attendance
                                 where p.Participate && s.Id == p.SheduleId
                                 select 1).Count();

            answer.General = new AttendanceForGroup.GeneralData()
            {
                Procent = (validShedule.Count() > 0 && countStudents > 0) ? (decimal)(participating / (validShedule.Count() * countStudents)) : 1
            };

            return answer;
        }

        public AttendanceForMonth GetAttendanceForMonth(int timeStartMonth, int? groupId)
        {
            var answer = new AttendanceForMonth();

            var shedule = (groupId == null) ? Db.GetGlobalShedule() : Db.GetGroupShedule((int)groupId);
            var dateStartMonth = new DateTime(1970, 1, 1).AddSeconds(timeStartMonth);
            var validShedule = shedule.Where(x => !x.Canceled && x.TimeStart.AddHours(3) >= dateStartMonth && x.TimeStart.AddHours(3) < dateStartMonth.AddMonths(1));
            var sheduleByGroup = validShedule.GroupBy(x => x.GroupId);

            var forStudents = new List<AttendanceForDay.ForStudentsData>();

            foreach (var gs in sheduleByGroup)
            {
                var students = Db.GetStudentsList(gs.Key);
                var attendance = Db.GetStudentsAttendances(gs.Key);

                foreach (var s in students)
                {
                    var contains = true;
                    var sudentAttendance = forStudents.FirstOrDefault(x => x.UserId == s.Id);
                    if (sudentAttendance == null)
                    {
                        sudentAttendance = new AttendanceForDay.ForStudentsData()
                        {
                            UserId = s.Id,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            General = new AttendanceForDay.ForStudentsData.GeneralData()
                        };
                        contains = false;
                    }
                    sudentAttendance.General.CountLessons += gs.Count();
                    sudentAttendance.General.CountParticipatingLessons += (from p in attendance
                                                                           where p.Participate && s.Id == p.StudentUserId
                                                                           && gs.FirstOrDefault(x => x.Id == p.SheduleId) != null
                                                                           select 1).Count();
                    sudentAttendance.General.Procent = sudentAttendance.General.CountParticipatingLessons / sudentAttendance.General.CountLessons;

                    if (!contains)
                        forStudents.Add(sudentAttendance);
                }
            }

            double allCountLessons = (forStudents.Count > 0) ? forStudents.Sum(x => x.General.CountLessons) : 0;
            double allCountParticipatingLessons = (forStudents.Count > 0) ? forStudents.Sum(x => x.General.CountParticipatingLessons) : 0;

            answer.General = new AttendanceForMonth.GeneralData()
            {
                Procent = (allCountLessons > 0) ? (decimal)(allCountParticipatingLessons / allCountLessons) : 1
            };

            return answer;
        }

        public AttendanceForStudent GetAttendanceForStudent(int userId, int? groupId)
        {
            var answer = new AttendanceForStudent();

            var shedule = Db.GetSheduleForStudent(userId);
            var attendance = Db.GetStudentAttendances(userId);

            double countValidShedules = shedule.Where(x => !x.Canceled).Count();
            answer.General = new AttendanceForStudent.GeneralData()
            {
                Procent = (countValidShedules > 0) ? (decimal)(attendance.Where(x => x.Participate).Count() / countValidShedules) : 1
            };

            answer.ForDays = new List<AttendanceForStudent.ForDaysData>();

            if (shedule.Count > 0)
            {
                var dateStart = shedule.Min(x => x.TimeStart);
                var dateEnd = DateTime.Now;
                var timeStart = (int)(dateStart - new DateTime(1970, 1, 1)).TotalSeconds;
                timeStart = timeStart - (timeStart % 86400);
                dateStart = new DateTime(1970, 1, 1).AddSeconds(timeStart);
                //var timeEnd = (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds + 86400;

                for (var i = dateStart; i < dateEnd; i = i.AddDays(1))
                {
                    var validShedule = shedule.Where(x => x.TimeStart.AddHours(3) >= i && x.TimeStart.AddHours(3) <= i.AddDays(1)).ToList();
                    countValidShedules = validShedule.Where(x => !x.Canceled).Count();
                    double participating = (from s in validShedule
                                         join p in attendance on s.Id equals p.SheduleId
                                         where p.Participate
                                         select 1).Count();

                    if (countValidShedules > 0)
                    {
                        var dataForDay = new AttendanceForStudent.ForDaysData()
                        {
                            General = new AttendanceForStudent.ForDaysData.GeneralData()
                            {
                                Procent = (decimal)(participating / countValidShedules)
                            },
                            TimeStartDay = (int)(i - new DateTime(1970, 1, 1)).TotalSeconds
                        };

                        answer.ForDays.Add(dataForDay);
                    }
                    
                }
            }

            return answer;
        }

        public void GetAvailableTeachers(ref GroupTeachersListsForLessons data)
        {
            foreach(var l in data.Lessons)
            {
                var availableTeachers = Db.GetAvailableTeachers(l.LessonId);
                l.AvailableTeachers = new List<Users>();

                foreach(var t in availableTeachers)
                {
                    if (l.Teachers.FirstOrDefault(x => x.Id == t.Id) == null)
                    {
                        l.AvailableTeachers.Add(t);
                    }
                }
            }
        }

        public StudentGroups GetGroupData(string groupName)
            => Db.GetGroupData(groupName);

        public GroupShedulesData GetGroupSheduleData(int groupId)
        {
            var answer = new GroupShedulesData();

            var shedules = Db.GetGroupShedule(groupId);
            var teachersGroupParticipations = Db.GetTeachersGroupParticipations(groupId);

            var studentList = new List<Users>();
            var isLoadStudentList = false;

            answer.Shedules = new List<GroupShedulesData.SheduleData>();

            foreach(var s in shedules)
            {
                var sheduleData = new GroupShedulesData.SheduleData() { Data = s };

                // Если я могу управлять успеваймостью данного урока
                if (IsAdmin || teachersGroupParticipations.FirstOrDefault(x => x.LessonId == s.LessonId && x.UserId == User.Id) != null)
                {
                    sheduleData.SubjectToChange = true;

                    if (!isLoadStudentList)
                        studentList = Db.GetStudentsList(groupId);

                    var studentsAttendances = Db.GetStudentsAttendance(s.Id);

                    sheduleData.StudentsAttendances = new List<GroupShedulesData.SheduleData.StudentAttendance>();

                    foreach(var student in studentList)
                    {
                        var attendance = studentsAttendances.FirstOrDefault(x => x.StudentUserId == student.Id);
                        sheduleData.StudentsAttendances.Add(new GroupShedulesData.SheduleData.StudentAttendance()
                        {
                            UserId = student.Id,
                            FirstName = student.FirstName,
                            LastName = student.LastName,
                            Attendance = (attendance == null) ? false : attendance.Participate
                        });
                    }
                }
                
                answer.Shedules.Add(sheduleData);
            }

            return answer;
        }

        public List<Lessons> GetLessons(int? groupId)
            => Db.GetLessons(groupId);

        public List<Users> GetStudentsList(int groupId)
            => Db.GetStudentsList(groupId);

        public GroupTeachersListsForLessons GetTeachersList(int groupId)
        {
            var answer = new GroupTeachersListsForLessons();

            var teachersGroupParticipations = Db.GetTeachersGroupParticipations(groupId);
            var users = Db.GetUsers();
            var groupLessons = Db.GetGroupLessons(groupId);

            foreach (var l in groupLessons)
                answer.Lessons.Add(new GroupTeachersListsForLessons.ByLesson() { LessonId = l.LessonId });

            foreach (var t in teachersGroupParticipations)
            {
                var u = users.FirstOrDefault(x => x.Id == t.UserId);
                if (answer.Lessons.FirstOrDefault(x => x.LessonId == t.LessonId) == null)
                    answer.Lessons.Add(new GroupTeachersListsForLessons.ByLesson() { LessonId = t.LessonId });
                answer.Lessons.FirstOrDefault(x => x.LessonId == t.LessonId).Teachers.Add(u);
            }

            return answer;
        }

        public List<Users> GetUsersAvalilableToAddToGroup(int groupId)
            => Db.GetUsersAvalilableToAddToGroup(groupId);

        public bool UpdateSheduleInfo(GroupShedules shedule)
            => Db.UpdateSheduleInfo(shedule);

        public bool UpdateStudentAttendances(int sheduleId, int studentUserId, bool participate)
            => Db.UpdateStudentAttendances(sheduleId, studentUserId, participate);
    }
}
