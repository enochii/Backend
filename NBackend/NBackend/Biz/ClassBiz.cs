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
        public static object get_time_info(int sec_id, int course_id, string semester, int year)
        {
            using (var context = new NBackendContext())
            {
                var time_info = (from section in context.Sections
                                 join multiSectionTime in context.MultiSectionsTimes on new { section.SecId, section.courseId, section.semester, section.year }
                                                                                    equals new { multiSectionTime.SecId, multiSectionTime.courseId, multiSectionTime.semester, multiSectionTime.year }
                                 join sectionTime in context.SectionTimes on multiSectionTime.section_timeId equals sectionTime.SectionTimeId
                                 where section.SecId == sec_id && section.courseId == course_id && section.semester == semester && section.year == year
                                 select new
                                 {
                                     year = section.year,
                                     semester = section.semester,
                                     course_id = section.courseId,
                                     sec_id = section.SecId,
                                     day = multiSectionTime.day,
                                     single_or_double = multiSectionTime.single_or_double,
                                     start_section = sectionTime.start_section,
                                     length = sectionTime.length
                                 });
                var list = new List<object>();
                foreach (var each_time_info in time_info)
                {
                    list.Add(new
                    {
                        day = each_time_info.day,
                        single_or_double = each_time_info.single_or_double,
                        start_section = each_time_info.start_section,
                        length = each_time_info.length
                    });
                }
                return list;

            }
        }

        public static object GetAllClasses()
        {
            using (var context = new NBackendContext())
            {
                var classes = context.Sections;
                var list = new List<object>();

                foreach (var a_class in classes)
                {
                    var a_course = (from each_course in context.Courses
                                    where each_course.CourseId == a_class.courseId
                                    select new
                                    {
                                        course_name = each_course.course_name,
                                        description = each_course.description
                                    });
                    var a_teacher = (from each_class in context.Sections
                                     join each_teaching in context.Teaches
                                          on new { each_class.SecId, each_class.courseId, each_class.semester, each_class.year }
                                          equals new { each_teaching.SecId, each_teaching.courseId, each_teaching.semester, each_teaching.year }
                                     join each_user in context.Users on each_teaching.TeacherId equals each_user.Id
                                     where each_class.SecId == a_class.SecId && each_class.courseId == a_class.courseId && each_class.semester == a_class.semester && each_class.year == a_class.year
                                     select new
                                     {
                                         teacher_name = each_user.user_name,
                                         avatar = each_user.avatar
                                     });
                    var the_course = a_course.Single();
                    var the_teacher = a_teacher.Single();


                    list.Add(new
                    {
                        sec_ID = a_class.SecId,
                        course_ID = a_class.courseId,
                        semester = a_class.semester,
                        year = a_class.year,
                        building = a_class.building,
                        room_number = a_class.room_numer,
                        time_slots = get_time_info(a_class.SecId, a_class.courseId, a_class.semester, a_class.year),
                        avatar = the_teacher.avatar,
                        user_name = the_teacher.teacher_name,
                        course_name = the_course.course_name,
                        course_description = the_course.description
                    });
                }

                var data = new
                {
                    classes = list
                };

                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object GetOneClass(object json)
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
                var a_course = (from each_course in context.Courses
                                where each_course.CourseId == the_class.courseId
                                select new
                                {
                                    course_name = each_course.course_name,
                                    description = each_course.description
                                });
                var a_teacher = (from each_class in context.Sections
                                 join each_teaching in context.Teaches
                                      on new { each_class.SecId, each_class.courseId, each_class.semester, each_class.year }
                                      equals new { each_teaching.SecId, each_teaching.courseId, each_teaching.semester, each_teaching.year }
                                 join each_user in context.Users on each_teaching.TeacherId equals each_user.Id
                                 where each_class.SecId == the_class.SecId && each_class.courseId == the_class.courseId && each_class.semester == the_class.semester && each_class.year == the_class.year
                                 select new
                                 {
                                     teacher_name = each_user.user_name,
                                     avatar = each_user.avatar
                                 });
                var the_course = a_course.Single();
                var the_teacher = a_teacher.Single();

                var students = context.Takes.Where(a => a.secId == sec_id && a.courseId == course_id
                                                 && a.semester == semester && a.year == year && a.validate_status == true);
                var data = new
                {
                    building = the_class.building,
                    room_number = the_class.room_numer,
                    time_slots = get_time_info(the_class.SecId, the_class.courseId, the_class.semester, the_class.year),
                    avatar = the_teacher.avatar,
                    user_name = the_teacher.teacher_name,
                    course_name = the_course.course_name,
                    course_description = the_course.description,
                    student_number = students.Count()
                };

                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object GetPartClass(object json, string token)
        {
            var body = Helper.JsonConverter.Decode(json);
            var year = int.Parse(body["year"]);
            var semester = body["semester"];
            var student_id = Helper.JwtManager.DecodeToken(token);

            using (var context = new NBackendContext())
            {
                var some_classes = (from each_class in context.Sections
                                    join each_taking in context.Takes
                                        on new { each_class.courseId, each_class.semester, each_class.year }
                                        equals new { each_taking.courseId, each_taking.semester, each_taking.year }
                                    where each_class.SecId == each_taking.secId && each_class.semester == semester && each_class.year == year && each_taking.StudentId == student_id
                                    select new
                                    {
                                        SecId = each_class.SecId,
                                        courseId = each_class.courseId,
                                        year = each_class.year,
                                        semester = each_class.semester
                                    });

                var list = new List<object>();

                foreach (var a_class in some_classes)
                {
                    var a_course = (from each_course in context.Courses
                                    where each_course.CourseId == a_class.courseId
                                    select new
                                    {
                                        course_name = each_course.course_name,
                                        description = each_course.description
                                    });
                    var a_teacher = (from each_class in context.Sections
                                     join each_teaching in context.Teaches
                                          on new { each_class.SecId, each_class.courseId, each_class.semester, each_class.year }
                                          equals new { each_teaching.SecId, each_teaching.courseId, each_teaching.semester, each_teaching.year }
                                     join each_user in context.Users on each_teaching.TeacherId equals each_user.Id
                                     where each_class.SecId == a_class.SecId && each_class.courseId == a_class.courseId && each_class.semester == a_class.semester && each_class.year == a_class.year
                                     select new
                                     {
                                         teacher_name = each_user.user_name,
                                         avatar = each_user.avatar
                                     });
                    var the_course = a_course.Single();
                    var the_teacher = a_teacher.Single();

                    list.Add(new
                    {
                        sec_id = a_class.SecId,
                        course_id = a_class.courseId,
                        //building = a_class.building,
                        //room_number = a_class.room_numer,
                        //section_time_id = a_class.section_timeId,
                        time_slots = get_time_info(a_class.SecId, a_class.courseId, a_class.semester, a_class.year),
                        avatar = the_teacher.avatar,
                        user_name = the_teacher.teacher_name,
                        course_name = the_course.course_name,
                        course_description = the_course.description
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
            var student_id = Helper.JwtManager.DecodeToken(token);

            using (var context = new NBackendContext())
            {
                var some_classes = context.Takes.Where(a => a.StudentId == student_id && a.validate_status == false);

                if (!some_classes.Any())
                {
                    return Helper.JsonConverter.BuildResult(null);
                }

                var list = new List<object>();

                foreach (var a_class in some_classes)
                {
                    var a_course = (from each_course in context.Courses
                                    where each_course.CourseId == a_class.courseId
                                    select new
                                    {
                                        course_name = each_course.course_name,
                                        description = each_course.description
                                    });
                    var a_teacher = (from each_class in context.Sections
                                     join each_teaching in context.Teaches
                                          on new { each_class.SecId, each_class.courseId, each_class.semester, each_class.year }
                                          equals new { each_teaching.SecId, each_teaching.courseId, each_teaching.semester, each_teaching.year }
                                     join each_user in context.Users on each_teaching.TeacherId equals each_user.Id
                                     where each_class.SecId == a_class.secId && each_class.courseId == a_class.courseId && each_class.semester == a_class.semester && each_class.year == a_class.year
                                     select new
                                     {
                                         teacher_name = each_user.user_name,
                                         avatar = each_user.avatar
                                     });
                    var the_course = a_course.Single();
                    var the_teacher = a_teacher.Single();

                    list.Add(new
                    {
                        year = a_class.year,
                        semester = a_class.semester,
                        sec_id = a_class.secId,
                        course_id = a_class.courseId,
                        //building = each_class.building,
                        //room_number = each_class.room_numer,
                        //section_time_id = each_class.section_timeId
                        time_slots = get_time_info(a_class.secId, a_class.courseId, a_class.semester, a_class.year),
                        avatar = the_teacher.avatar,
                        user_name = the_teacher.teacher_name,
                        course_name = the_course.course_name,
                        course_description = the_course.description
                    });
                }

                var data = new
                {
                    classes = list
                };

                return Helper.JsonConverter.BuildResult(data);

            }
        }

        public static object GetWaitingStudents(object json)
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
                if (!waiting_students.Any())
                {
                    return Helper.JsonConverter.BuildResult(null);
                }

                var students_info = (from student in waiting_students
                                     join info in context.Users on student.StudentId equals info.Id
                                     select new { student_id = student.StudentId, user_name = info.user_name, avatar = info.avatar }).ToArray();

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

        public static object GetOneClassDetails(object json)
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
                var list = new List<object>();
                foreach (var a_student in students)
                {
                    var user_info = context.Users.Single(a => a.Id == a_student.StudentId);
                    var student_info = context.Students.Single(a => a.StudentId == a_student.StudentId);
                    list.Add(new
                    {
                        student_id = a_student.StudentId,
                        student_name = user_info.user_name,
                        student_grade = student_info.grade
                    });
                }
                var data = new
                {
                    building = the_class.building,
                    room_number = the_class.room_numer,
                    //section_time_id = the_class.section_timeId,
                    time_slots = get_time_info(the_class.SecId, the_class.courseId, the_class.semester, the_class.year),
                    students = list
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
            var student_id = Helper.JwtManager.DecodeToken(token);

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

        public static object CreateClass(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);
            var building = body["building"];
            var room_number = body["room_number"];
            //var section_time_id = int.Parse(body["section_time_id"]);
            var avatar = body["avatar"];
            var start_week = int.Parse(body["start_week"]);
            var end_week = int.Parse(body["end_week"]);
            var sec_id = int.Parse(body["sec_id"]);

            using (var context = new NBackendContext())
            {
                var any_classes = context.Sections.Where(a => a.courseId == course_id
                                                            && a.semester == semester && a.year == year);
                context.Sections.Add(new Section
                {
                    SecId = sec_id,
                    courseId = course_id,
                    semester = semester,
                    year = year,
                    building = building,
                    room_numer = room_number,
                    //section_timeId = section_time_id,
                    avatar = avatar,
                    start_week = start_week,
                    end_week = end_week,
                });
                context.SaveChanges();
                var a_class = context.Sections.Where(a => a.courseId == course_id
                                                        && a.semester == semester && a.year == year).OrderByDescending(a => a.SecId);
                var the_class = a_class.First();
                return Helper.JsonConverter.BuildResult(new { the_class.SecId });
            }
        }

        public static object PermitApplication(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);
            var student_id = int.Parse(body["user_id"]);

            using (var context = new NBackendContext())
            {
                var a_class = context.Takes.Where(a => a.StudentId == student_id && a.validate_status == false);

                if (!a_class.Any())
                {
                    return Helper.JsonConverter.Error(401, "班级不存在");
                }
                else
                {
                    foreach (var one_class in a_class)
                    {
                        one_class.validate_status = true;
                    }
                    context.SaveChanges();
                    return Helper.JsonConverter.BuildResult(null);
                }
            }
        }

    }
}