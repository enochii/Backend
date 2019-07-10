using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBackend.Helper;
using NBackend.Models;
using System.Data.Entity;


namespace NBackend.Biz
{
    public class TeamBiz
    {
        public static object GetTeams(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);

            using (var context = new NBackendContext())
            {
                var teams = context.Teams.Where(a => a.secId == sec_id && a.courseId == course_id
                                                            && a.semester == semester && a.year == year);
                var list = new List<object>();

                foreach (var each_team in teams)
                {
                    var team_members = context.TeamStudents.Where(a => a.teamId == each_team.TeamId);
                    var member_list = new List<object>();
                    foreach (var team_member in team_members)
                    {
                        member_list.Add(new
                        {
                            student_id = team_member.studentId,
                            student_name = context.Users.Single(a => a.Id == team_member.studentId)
                        });
                    }
                    list.Add(new
                    {
                        team_id = each_team.TeamId,
                        team_name = each_team.team_name,
                        students = member_list
                    });
                }

                var data = new
                {
                    teams = list
                };


                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object GetTeamsByKeyWords(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);
            var key_words = body["key_word"];

            using (var context = new NBackendContext())
            {
                var teams = context.Teams.Where(a => a.secId == sec_id && a.courseId == course_id
                                                            && a.semester == semester && a.year == year && a.team_name.Contains(key_words));
                var list = new List<object>();

                foreach (var each_team in teams)
                {
                    var team_members = context.TeamStudents.Where(a => a.teamId == each_team.TeamId);
                    var member_list = new List<object>();
                    foreach (var team_member in team_members)
                    {
                        member_list.Add(new
                        {
                            student_id = team_member.studentId,
                            student_name = context.Users.Single(a => a.Id == team_member.studentId)
                        });
                    }
                    list.Add(new
                    {
                        team_id = each_team.TeamId,
                        team_name = each_team.team_name,
                        students = member_list
                    });
                }

                var data = new
                {
                    teams = list
                };


                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object GetItsTeams(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var student_id = int.Parse(body["student_id"]);

            using (var context = new NBackendContext())
            {
                var teams = context.TeamStudents.Where(a => a.studentId == student_id);
                var list = new List<object>();

                foreach (var each_team in teams)
                {
                    var team_members = context.TeamStudents.Where(a => a.teamId == each_team.teamId);
                    var member_list = new List<object>();
                    var team_info = context.Teams.Single(a => a.TeamId == each_team.teamId);
                    var course_info = context.Courses.Single(a => a.CourseId == team_info.courseId);
                    foreach (var team_member in team_members)
                    {
                        member_list.Add(new
                        {
                            student_id = team_member.studentId,
                            student_name = context.Users.Single(a => a.Id == team_member.studentId)
                        });
                    }
                    list.Add(new
                    {
                        year = team_info.year,
                        semester = team_info.semester,
                        sec_id = team_info.secId,
                        course_id = team_info.courseId,
                        course_name = course_info.course_name,
                        team_id = each_team.teamId,
                        team_name = team_info.team_name,
                        students = member_list
                    });
                }

                var data = new
                {
                    teams = list
                };


                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object PostTeam(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);
            var team_name = body["name"];

            using (var context = new NBackendContext())
            {
                context.Teams.Add(new Team
                {
                    secId = sec_id,
                    courseId = course_id,
                    semester = semester,
                    year = year,
                    team_name = team_name,
                });

                context.SaveChanges();

                var data = new
                {
                    team_id = context.Teams.Single(a => a.secId == sec_id && a.courseId == course_id
                                                   && a.semester == semester && a.year == year).TeamId
                };
                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object PostTeamAttendance(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var team_id = int.Parse(body["team_id"]);
            var student_id = int.Parse(body["student_id"]);

            using (var context = new NBackendContext())
            {
                context.TeamStudents.Add(new TeamStudent
                {
                    teamId = team_id,
                    studentId = student_id
                });
                context.SaveChanges();
                return Helper.JsonConverter.BuildResult(null);
            }
        }

    }
}