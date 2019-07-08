using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBackend.Helper;
using NBackend.Models;
using System.Data.Entity;


namespace NBackend.Biz
{
    public class ClassBiz
    {
        public static object GetAllClasses(string token)
        {
            using (var context = new NBackendContext())
            {
                var classes = context.Sections;
                var list = new List<object>();

                foreach (var a_class in classes)
                {
                    list.Add(new
                    {
                        sec_ID = a_class.SecId,
                        course_ID = a_class.courseId,
                        semester = a_class.semester,
                        year = a_class.year,
                        building = a_class.building,
                        room_number = a_class.room_numer,
                        section_time_ID = a_class.section_timeId
                    });
                }

                var data = new
                {
                    classes = list
                };

                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object GetOneClass(object json, string token)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);

            using (var context = new NBackendContext())
            {
                var a_class = context.Sections.Where(a => a.SecId == sec_id && a.courseId == course_id && a.semester == semester && a.year == year);
                var the_class = a_class.Single();

                var data = new
                {
                    building = the_class.building,
                    room_number = the_class.room_numer,
                    section_time_id = the_class.section_timeId
                };

                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object GetPartClass(object json, string token)
        {
            var body = Helper.JsonConverter.Decode(json);
            var year = int.Parse(body["year"]);
            var semester = body["semester"];

            using (var context = new NBackendContext())
            {
                var some_classes = context.Sections.Where(a => a.year == year && a.semester == semester);

                var list = new List<object>();

                foreach (var each_class in some_classes)
                {
                    list.Add(new
                    {
                        sec_id = each_class.SecId,
                        course_id = each_class.courseId,
                        building = each_class.building,
                        room_number = each_class.room_numer,
                        section_time_id = each_class.section_timeId
                    });
                }

                var data = new
                {
                    classes = list
                };

                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object GetWaitingClass(object json, string token)
        {
            var body = Helper.JsonConverter.Decode(json);
            var student_id = int.Parse(body["student_id"]);

            using (var context = new NBackendContext())
            {
                var some_classes = context.Takes.Where(a => a.StudentId == student_id && a.validate_status == false);
                var list = new List<object>();

                foreach (var each_class in some_classes)
                {
                    list.Add(new
                    {
                        year = each_class.year,
                        semester = each_class.semester,
                        sec_id = each_class.SecId,
                        course_id = each_class.courseId,
                        building = each_class.building,
                        room_number = each_class.room_numer,
                        section_time_id = each_class.section_timeId
                    });
                }

                var data = new
                {
                    classes = list
                };

                return Helper.JsonConverter.BuildResult(data);

            }
        }

        public static object GetWaitingStudents(object json, string token)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);

            using (var context = new NBackendContext())
            {
                var waiting_students = context.Takes.Where(a => a.secId == sec_id && a.courseId == course_id
                                                           && a.semester == semester && a.year == year && a.validate_status == false);
                var students_info = from student in waiting_students
                                    join info in context.Users on student.StudentId equals info.Id
                                    select new { student_id = student.StudentId, user_name = info.user_name, avatar = info.avatar };

                /*var list = new List<object>();

                foreach (var each_student in students_info)
                {
                    list.Add(new
                    {
                        student_id = each_student.student_id,
                        user_name = each_student.user_name,
                        avatar = each_student.avatar
                    });
                }*/

                var data = new
                {
                    students_info
                };

                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object GetOneClassDetails(object json, string token)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);

            using (var context = new NBackendContext())
            {
                var a_class = context.Sections.Where(a => a.SecId == sec_id && a.courseId == course_id && a.semester == semester && a.year == year);
                var the_class = a_class.Single();
                var students = context.Takes.Where(a => a.secId == sec_id && a.courseId == course_id
                                                       && a.semester == semester && a.year == year);

                var data = new
                {
                    building = the_class.building,
                    room_number = the_class.room_numer,
                    section_time_id = the_class.section_timeId,
                    students
                };

                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object JoinClass(object json, string token)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);
            var student_id = int.Parse(body["user_id"]);

            using (var context = new NBackendContext())
            {
                var a_class = context.Sections.Where(a => a.SecId == sec_id && a.courseId == course_id
                                                            && a.semester == semester && a.year == year);
                if (!a_class.Any())
                {
                    return Helper.JsonConverter.Error(401, "班级不存在");
                }
                else
                {
                    var the_class = a_class.Single();
                    var a_student = context.Students.Where(a => a.StudentId == student_id);
                    var the_student = a_student.Single();
                    context.Takes.Add(new Take
                    {
                        StudentId = the_student.StudentId,
                        secId = the_class.SecId,
                        courseId = the_class.courseId,
                        semester = the_class.semester,
                        year = the_class.year,
                        validate_status = false
                    });

                    context.SaveChanges();

                    return Helper.JsonConverter.BuildResult(null);

                }
            }
        }

        public static object CreateClass(object json, string token)
        {
            var body = Helper.JsonConverter.Decode(json);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);
            var building = body["building"];
            var room_number = int.Parse(body["room_number"]);
            var section_time_id = int.Parse(body["section_time_id"]);
            var avatar = body["avatar"];

            using (var context = new NBackendContext())
            {
                var any_classes = context.Sections.Where(a => a.courseId == course_id
                                                            && a.semester == semester && a.year == year);
                if (any_classes.Any())
                {
                    return Helper.JsonConverter.Error(401, "课程已经存在");
                }
                else
                {
                    context.Sections.Add(new Section
                    {
                        courseId = course_id,
                        semester = semester,
                        year = year,
                        building = building,
                        room_numer = room_number,
                        section_timeId = section_time_id,
                        avatar = avatar
                    });
                    var a_class = context.Sections.Where(a => a.courseId == course_id
                                                            && a.semester == semester && a.year == year);
                    var the_class = a_class.Single();
                    return Helper.JsonConverter.BuildResult(new {the_class.SecId });
                }
            }
        }

        public static object PermitApplication(object json, string token)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);
            var student_id = int.Parse(body["student_id"]);

            using (var context = new NBackendContext())
            {
                var a_class = context.Takes.Where(a => a.StudentId == student_id && a.validate_status == false);

                if (!a_class.Any())
                {
                    return Helper.JsonConverter.Error(401, "班级不存在");
                }
                else
                {
                    foreach(var one_class in a_class)
                    {
                        one_class.validate_status = true;
                    }
                    return Helper.JsonConverter.BuildResult(null);
                }
            }
        }

    }
}