using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBackend.Helper;
using NBackend.Models;
using System.Data.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NBackend.Biz
{
    public class ClassBiz
    {
        //在section表中找到某条记录
        public static Section getSection(NBackendContext ctx, int sec_id, int course_id, int year, string semester)
        {
            var q = ctx.Sections.Where(sec => sec.SecId == sec_id && sec.courseId == course_id
            && sec.year == year && sec.semester == semester
            );
            if (!q.Any())
            {
                return null;
            }
            return q.Single();
        }

        //获取某个班级的开课时间段
        public static object get_time_info(int sec_id, int course_id, string semester, int year)
        {
            using (var context = new NBackendContext())
            {
                var time_info = (from section in context.Sections
                                 join multiSectionTime in context.MultiSectionTimes on new { section.SecId, section.courseId, section.semester, section.year }
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

        //获取班级列表信息
        public static object GetAllClasses()
        {
            try
            {
                using (var context = new NBackendContext())
                {
                    var classes = context.Sections;
                    var list = new List<object>();
                    if (!classes.Any())
                    {
                        return Helper.JsonConverter.Error(400, "现在还没有已经创建的班级");
                    }

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
                                             avatar = each_class.avatar
                                         });
                        if ((!a_course.Any()) || (!a_teacher.Any()))
                            continue;
                        var the_course = a_course.Single();
                        var the_teacher = a_teacher.Single();

                        list.Add(new
                        {
                            sec_id = a_class.SecId,
                            course_id = a_class.courseId,
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
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "数据库中可能存在不一致现象，请检查");
            }


        }

        //学生获取一个特定班级的信息，包括学生人数
        public static object GetOneClass(object json, string token)
        {
            try
            {
                var body = Helper.JsonConverter.Decode(json);
                var sec_id = int.Parse(body["sec_id"]);
                var course_id = int.Parse(body["course_id"]);
                var semester = body["semester"];
                var year = int.Parse(body["year"]);
                var user_id = Helper.JwtManager.DecodeToken(token);

                using (var context = new NBackendContext())
                {
                    var a_class = context.Sections.Where(a => a.SecId == sec_id && a.courseId == course_id && a.semester == semester && a.year == year);
                    if (!a_class.Any())
                    {
                        return Helper.JsonConverter.Error(400, "不存在这个班级");
                    }

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
                                         avatar = each_class.avatar
                                     });

                    var the_course = a_course.Single();
                    var the_teacher = a_teacher.Single();

                    var students = context.Takes.Where(a => a.secId == sec_id && a.courseId == course_id
                                                     && a.semester == semester && a.year == year);
                    int if_join = 0;
                    var this_student = students.Where(a => a.StudentId == user_id);
                    if (this_student.Any())
                    {
                        if_join = 1;
                        if (this_student.Single().validate_status == true)
                            if_join = 2;
                    }
                    var data = new
                    {
                        sec_id,
                        course_id,
                        semester,
                        year,
                        building = the_class.building,
                        room_number = the_class.room_numer,
                        time_slots = get_time_info(the_class.SecId, the_class.courseId, the_class.semester, the_class.year),
                        avatar = the_teacher.avatar,
                        user_name = the_teacher.teacher_name,
                        course_name = the_course.course_name,
                        course_description = the_course.description,
                        student_number = students.Where(a => a.validate_status == true).Count(),
                        status = if_join
                    };
                    return Helper.JsonConverter.BuildResult(data);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "请检查数据输入和字段名称，检查数据库表一致性");
            }
        }

        //学生获取所有参与的班级信息，教师获取所有教过的班级信息
        public static object GetMyClass(object json, string token)
        {
            try
            {
                var user_id = Helper.JwtManager.DecodeToken(token);

                using (var context = new NBackendContext())
                {
                    int student_or_teacher = 0;
                    if (context.Users.Where(a => a.Id == user_id).Any())
                    {
                        if (context.Users.Single(a => a.Id == user_id).role == "student")
                            student_or_teacher = 1;
                        else
                            student_or_teacher = 2;
                    }
                    else
                        return Helper.JsonConverter.Error(400, "这个人有问题啊");

                    var list = new List<object>();

                    if (student_or_teacher == 1)
                    {
                        var some_classes = (from each_class in context.Sections
                                            join each_taking in context.Takes
                                                on new { each_class.courseId, each_class.semester, each_class.year }
                                                equals new { each_taking.courseId, each_taking.semester, each_taking.year }
                                            where each_class.SecId == each_taking.secId && each_taking.StudentId == user_id && each_taking.validate_status == true
                                            select new
                                            {
                                                SecId = each_class.SecId,
                                                courseId = each_class.courseId,
                                                year = each_class.year,
                                                semester = each_class.semester,
                                                score = each_taking.score
                                            });
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
                                                 avatar = each_class.avatar
                                             });
                            var the_course = a_course.Single();
                            var the_teacher = a_teacher.Single();

                            list.Add(new
                            {
                                sec_id = a_class.SecId,
                                course_id = a_class.courseId,
                                semester = a_class.semester,
                                year = a_class.year,
                                score = a_class.score,
                                time_slots = get_time_info(a_class.SecId, a_class.courseId, a_class.semester, a_class.year),
                                avatar = the_teacher.avatar,
                                user_name = the_teacher.teacher_name,
                                course_name = the_course.course_name,
                                course_description = the_course.description
                            });
                        }
                    }
                    else if (student_or_teacher == 2)
                    {
                        var a_teacher = (from each_class in context.Sections
                                         join each_teaching in context.Teaches
                                              on new { each_class.SecId, each_class.courseId, each_class.semester, each_class.year }
                                              equals new { each_teaching.SecId, each_teaching.courseId, each_teaching.semester, each_teaching.year }
                                         join each_user in context.Users on each_teaching.TeacherId equals each_user.Id
                                         where each_teaching.TeacherId == user_id
                                         select new
                                         {
                                             year = each_class.year,
                                             semester = each_class.semester,
                                             course_id = each_class.courseId,
                                             sec_id = each_class.SecId,
                                             teacher_name = each_user.user_name,
                                             avatar = each_class.avatar
                                         });
                        foreach (var a_class in a_teacher)
                        {
                            var a_course = (from each_course in context.Courses
                                            where each_course.CourseId == a_class.course_id
                                            select new
                                            {
                                                course_name = each_course.course_name,
                                                description = each_course.description
                                            });

                            var the_course = a_course.Single();
                            list.Add(new
                            {
                                sec_id = a_class.sec_id,
                                course_id = a_class.course_id,
                                semester = a_class.semester,
                                year = a_class.year,
                                time_slots = get_time_info(a_class.sec_id, a_class.course_id, a_class.semester, a_class.year),
                                avatar = a_class.avatar,
                                user_name = a_class.teacher_name,
                                course_name = the_course.course_name,
                                course_description = the_course.description
                            });
                        }

                    }

                    var data = new
                    {
                        classes = list
                    };
                    return Helper.JsonConverter.BuildResult(data);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "数据库中可能存在不一致现象，请检查");
            }
        }

        //获取某个学生某个学期参加的班级信息
        public static object GetPartClass(object json, string token)
        {
            try
            {
                var body = Helper.JsonConverter.Decode(json);
                var year = int.Parse(body["year"]);
                var semester = body["semester"];
                var student_id = Helper.JwtManager.DecodeToken(token);

                using (var context = new NBackendContext())
                {
                    var any_student = context.Users.Where(a => a.Id == student_id);
                    if (!any_student.Any())
                    {
                        return Helper.JsonConverter.Error(400, "这个人有问题");
                    }

                    var list = new List<object>();

                    if (any_student.Single().role == "student")
                    {
                        var some_classes = (from each_class in context.Sections
                                            join each_taking in context.Takes
                                                on new { each_class.courseId, each_class.semester, each_class.year }
                                                equals new { each_taking.courseId, each_taking.semester, each_taking.year }
                                            where each_class.SecId == each_taking.secId && each_class.semester == semester &&
                                                    each_class.year == year && each_taking.StudentId == student_id && each_taking.validate_status == true
                                            select new
                                            {
                                                SecId = each_class.SecId,
                                                courseId = each_class.courseId,
                                                year = each_class.year,
                                                semester = each_class.semester,
                                                score = each_taking.score,
                                                building = each_class.building,
                                                room_number = each_class.room_numer
                                            });

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
                                                 avatar = each_class.avatar
                                             });
                            var the_course = a_course.Single();
                            var the_teacher = a_teacher.Single();

                            var time_slots = (from section in context.Sections
                                              join multiSectionTime in context.MultiSectionTimes on new { section.SecId, section.courseId, section.semester, section.year }
                                                                                                 equals new { multiSectionTime.SecId, multiSectionTime.courseId, multiSectionTime.semester, multiSectionTime.year }
                                              join sectionTime in context.SectionTimes on multiSectionTime.section_timeId equals sectionTime.SectionTimeId
                                              where section.SecId == a_class.SecId && section.courseId == a_class.courseId && section.semester == a_class.semester && section.year == a_class.year
                                              select new
                                              {
                                                  day = multiSectionTime.day,
                                                  single_or_double = multiSectionTime.single_or_double,
                                                  start_section = sectionTime.start_section,
                                                  length = sectionTime.length
                                              });
                            //var time_slots = get_time_info(a_class.SecId, a_class.courseId, a_class.semester, a_class.year);
                            foreach (var a_time_slot in time_slots)
                            {
                                list.Add(new
                                {
                                    user_name = the_teacher.teacher_name,
                                    course_name = the_course.course_name,
                                    day = int.Parse(a_time_slot.day),
                                    single_or_double = a_time_slot.single_or_double,
                                    start_section = a_time_slot.start_section,
                                    length = a_time_slot.length,
                                    building = a_class.building,
                                    room_number = a_class.room_number
                                    //course_description = the_course.description
                                });

                            }
                        }
                    }
                    else
                    {
                        var some_classes = (from each_class in context.Sections
                                            join each_teach in context.Teaches
                                                on new { each_class.courseId, each_class.semester, each_class.year }
                                                equals new { each_teach.courseId, each_teach.semester, each_teach.year }
                                            where each_class.SecId == each_teach.SecId && each_class.semester == semester &&
                                                    each_class.year == year && each_teach.TeacherId == student_id
                                            select new
                                            {
                                                SecId = each_class.SecId,
                                                courseId = each_class.courseId,
                                                year = each_class.year,
                                                semester = each_class.semester,
                                                building = each_class.building,
                                                room_number = each_class.room_numer
                                            });

                        foreach (var a_class in some_classes)
                        {
                            var a_course = (from each_course in context.Courses
                                            where each_course.CourseId == a_class.courseId
                                            select new
                                            {
                                                course_name = each_course.course_name,
                                                description = each_course.description
                                            });
                            var the_course = a_course.Single();
                            var time_slots = (from section in context.Sections
                                              join multiSectionTime in context.MultiSectionTimes on new { section.SecId, section.courseId, section.semester, section.year }
                                                                                                 equals new { multiSectionTime.SecId, multiSectionTime.courseId, multiSectionTime.semester, multiSectionTime.year }
                                              join sectionTime in context.SectionTimes on multiSectionTime.section_timeId equals sectionTime.SectionTimeId
                                              where section.SecId == a_class.SecId && section.courseId == a_class.courseId && section.semester == a_class.semester && section.year == a_class.year
                                              select new
                                              {
                                                  day = multiSectionTime.day,
                                                  single_or_double = multiSectionTime.single_or_double,
                                                  start_section = sectionTime.start_section,
                                                  length = sectionTime.length
                                              });
                            //var time_slots = get_time_info(a_class.SecId, a_class.courseId, a_class.semester, a_class.year);
                            foreach (var a_time_slot in time_slots)
                            {
                                list.Add(new
                                {
                                    user_name = any_student.Single().user_name,
                                    course_name = the_course.course_name,
                                    day = int.Parse(a_time_slot.day),
                                    single_or_double = a_time_slot.single_or_double,
                                    start_section = a_time_slot.start_section,
                                    length = a_time_slot.length,
                                    building = a_class.building,
                                    room_number = a_class.room_number
                                });

                            }
                        }
                    }

                    var data = new
                    {
                        classes = list
                    };

                    return Helper.JsonConverter.BuildResult(data);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "输入字段可能存在差错，数据库中可能存在不一致现象，请检查");
            }
        }

        //学生获取正在申请进入的班级
        public static object GetWaitingClass(string token)
        {
            try
            {
                var student_id = Helper.JwtManager.DecodeToken(token);

                using (var context = new NBackendContext())
                {
                    var any_student = context.Users.Where(a => a.Id == student_id && a.role == "student");
                    if (!any_student.Any())
                    {
                        return Helper.JsonConverter.Error(400, "这个人有问题");
                    }

                    var some_classes = context.Takes.Where(a => a.StudentId == student_id && a.validate_status == false);
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
                                             avatar = each_class.avatar
                                         });
                        var the_course = a_course.Single();
                        var the_teacher = a_teacher.Single();

                        list.Add(new
                        {
                            year = a_class.year,
                            semester = a_class.semester,
                            sec_id = a_class.secId,
                            course_id = a_class.courseId,
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
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "数据库中可能存在不一致现象，请检查");
            }
        }

        //教师查看待审核的学生
        public static object GetWaitingStudents(object json)
        {
            try
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

                    var students_info = (from student in waiting_students
                                         join info in context.Users on student.StudentId equals info.Id
                                         select new { student_id = student.StudentId, user_name = info.user_name, avatar = info.avatar }).ToArray();

                    var data = new
                    {
                        students_info
                    };
                    return Helper.JsonConverter.BuildResult(data);

                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "数据库中可能存在不一致现象，请检查");
            }
        }

        //教师获取一个班级的详细信息，包括学生列表
        public static object GetOneClassDetails(object json, string token)
        {
            try
            {
                var body = Helper.JsonConverter.Decode(json);
                var sec_id = int.Parse(body["sec_id"]);
                var course_id = int.Parse(body["course_id"]);
                var semester = body["semester"];
                var year = int.Parse(body["year"]);
                var teacher_id = Helper.JwtManager.DecodeToken(token);

                using (var context = new NBackendContext())
                {
                    var any_teacher = context.Users.Where(a => a.Id == teacher_id && (a.role == "teacher_manage" || a.role == "teacher_edu"));
                    if (!any_teacher.Any())
                    {
                        return Helper.JsonConverter.Error(400, "这个人有问题");
                    }

                    var a_class = context.Sections.Where(a => a.SecId == sec_id && a.courseId == course_id && a.semester == semester && a.year == year);
                    if (!a_class.Any())
                    {
                        return Helper.JsonConverter.Error(400, "不存在这个班级");
                    }

                    var the_class = a_class.Single();
                    var students = context.Takes.Where(a => a.secId == sec_id && a.courseId == course_id
                                                           && a.semester == semester && a.year == year && a.validate_status == true);
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
                        sec_id,
                        course_id,
                        semester,
                        year,
                        building = the_class.building,
                        room_number = the_class.room_numer,
                        //section_time_id = the_class.section_timeId,
                        time_slots = get_time_info(the_class.SecId, the_class.courseId, the_class.semester, the_class.year),
                        students = list,
                        student_number = list.Count(),
                        teacher_name = any_teacher.Single().user_name
                    };

                    return Helper.JsonConverter.BuildResult(data);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "数据库中可能存在不一致现象，请检查");
            }
        }

        //学生申请加入班级
        public static object JoinClass(object json, string token)
        {
            try
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
                        return Helper.JsonConverter.Error(400, "该班级不存在");
                    }
                    else
                    {
                        var the_class = a_class.Single();
                        var a_student = context.Students.Where(a => a.StudentId == student_id);
                        if (!a_student.Any())
                        {
                            return Helper.JsonConverter.Error(400, "不存在这个学生");
                        }

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
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "数据库中可能存在不一致现象，请检查");
            }
        }

        //教师创建班级
        public static object CreateClass(object json)
        {
            try
            {
                //取数据
                var jObject = new JObject();
                jObject = JObject.Parse(json.ToString());
                JArray jlist = JArray.Parse(jObject["time_slots"].ToString());
                var course_id = int.Parse(jObject["course_id"].ToString());
                var semester = jObject["semester"].ToString();
                var year = int.Parse(jObject["year"].ToString());
                var building = jObject["building"].ToString();
                var room_number = jObject["room_number"].ToString();
                var avatar = jObject["avatar"].ToString();
                var start_week = int.Parse(jObject["start_week"].ToString());
                var end_week = int.Parse(jObject["end_week"].ToString());
                var user_id = int.Parse(jObject["user_id"].ToString());

                //与数据库交互
                using (var context = new NBackendContext())
                {
                    //验证数据
                    var any_course = context.Courses.Where(a => a.CourseId == course_id);
                    if (!any_course.Any())
                    {
                        return Helper.JsonConverter.Error(400, "不存在这门课程");
                    }

                    if (semester == "Spring")
                    {
                        if (int.Parse(DateTime.Now.Month.ToString()) > 6)
                        {
                            return Helper.JsonConverter.Error(400, "这都下半学期啦");
                        }
                    }

                    var any_teacher = context.Users.Where(a => a.Id == user_id && a.role != "student");
                    if (!any_teacher.Any())
                    {
                        return Helper.JsonConverter.Error(400, "不存在这个老师");
                    }

                    if (avatar == null)
                        avatar = "https://view.moezx.cc/images/2018/06/12/31133259.jpg";
                    if (start_week < 1 || end_week > 17)
                        return Helper.JsonConverter.Error(410, "开始周数或者结束周数不合理");
                    //插入班级
                    var the_class = new Section
                    {
                        courseId = course_id,
                        semester = semester,
                        year = year,
                        building = building,
                        room_numer = room_number,
                        avatar = avatar,
                        start_week = start_week,
                        end_week = end_week,
                    };

                    for (int i = 0; i < jlist.Count(); i++)
                    {
                        var day = jlist[i]["day"].ToString();
                        var start_section = int.Parse(jlist[i]["start_section"].ToString());
                        var length = int.Parse(jlist[i]["length"].ToString());
                        var single_or_double = int.Parse(jlist[i]["single_or_double"].ToString());

                        var any_time_slot = context.SectionTimes.Where(a => a.start_section == start_section && a.length == length);
                        if (!any_time_slot.Any())
                        {
                            return Helper.JsonConverter.Error(400, "不存在这个时间方案");
                        }
                        var the_time_slot = any_time_slot.Single();
                    }

                    context.Sections.Add(the_class);
                    context.SaveChanges();

                    //插入multisectiontime表
                    for (int i = 0; i < jlist.Count(); i++)
                    {
                        var day = jlist[i]["day"].ToString();
                        var start_section = int.Parse(jlist[i]["start_section"].ToString());
                        var length = int.Parse(jlist[i]["length"].ToString());
                        var single_or_double = int.Parse(jlist[i]["single_or_double"].ToString());

                        var any_time_slot = context.SectionTimes.Where(a => a.start_section == start_section && a.length == length);
                        if (!any_time_slot.Any())
                        {
                            return Helper.JsonConverter.Error(400, "不存在这个时间方案");
                        }
                        var the_time_slot = any_time_slot.Single();

                        context.MultiSectionTimes.Add(new MultiSectionTimes
                        {
                            SecId = the_class.SecId,
                            courseId = the_class.courseId,
                            semester = the_class.semester,
                            year = the_class.year,
                            section_timeId = the_time_slot.SectionTimeId,
                            day = day,
                            single_or_double = single_or_double
                        });
                    }
                    //插入teach表
                    context.Teaches.Add(new Teach
                    {
                        TeacherId = user_id,
                        SecId = the_class.SecId,
                        courseId = the_class.courseId,
                        semester = the_class.semester,
                        year = the_class.year
                    });
                    context.SaveChanges();
                    return Helper.JsonConverter.BuildResult(new { the_class.SecId });
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "数据库中可能存在不一致现象，请检查");
            }
        }

        //教师允许学生加入班级
        public static object PermitApplication(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);
            var student_id = int.Parse(body["user_id"]);
            var status = int.Parse(body["status"]);

            using (var context = new NBackendContext())
            {
                var a_student = context.Takes.Where(a => a.StudentId == student_id && a.secId == sec_id && a.courseId == course_id
                                                    && a.year == year && a.semester == semester);

                if (!a_student.Any())
                {
                    return Helper.JsonConverter.Error(401, "学生在出席记录中不存在");
                }
                else
                {
                    if (status == 1)
                        a_student.Single().validate_status = true;
                    else
                        context.Takes.Remove(a_student.Single());
                    context.SaveChanges();
                    return Helper.JsonConverter.BuildResult(null);
                }
            }
        }

        //获取开设的所有课程id
        public static object GetCourses()
        {
            try
            {
                using (var context = new NBackendContext())
                {
                    var some_courses = context.Courses;
                    var list = new List<object>();

                    foreach (var each_course in some_courses)
                    {
                        list.Add(new
                        {
                            course_id = each_course.CourseId,
                            course_name = each_course.course_name
                        });
                    }

                    var data = new
                    {
                        courses = list
                    };

                    return Helper.JsonConverter.BuildResult(data);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "数据库中可能存在不一致现象，请检查");
            }

        }

        //获取一个班级的所有学生
        public static object GetStudents(object json)
        {
            try
            {
                var body = Helper.JsonConverter.Decode(json);
                var sec_id = int.Parse(body["sec_id"]);
                var course_id = int.Parse(body["course_id"]);
                var semester = body["semester"];
                var year = int.Parse(body["year"]);

                using (var context = new NBackendContext())
                {
                    var some_students = context.Takes.Where(a => a.secId == sec_id && a.courseId == course_id
                                                                && a.semester == semester && a.year == year && a.validate_status == true);
                    var list = new List<object>();

                    foreach (var each_student in some_students)
                    {
                        var user_info = context.Users.Where(a => a.Id == each_student.StudentId);
                        if (!user_info.Any())
                        {
                            return Helper.JsonConverter.Error(400, "这个人有问题");
                        }
                        var the_user_info = user_info.Single();
                        list.Add(new
                        {
                            user_id = the_user_info.Id,
                            user_name = the_user_info.user_name,
                            department = the_user_info.department,
                            grade = context.Students.Single(a => a.StudentId == each_student.StudentId).grade,
                            avatar = the_user_info.avatar
                        });
                    }

                    var data = new
                    {
                        students = list
                    };

                    return Helper.JsonConverter.BuildResult(data);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "数据库中可能存在不一致现象，请检查");
            }
        }

        //获取所有教师id
        public static object GetTeachersId()
        {
            using (var context = new NBackendContext())
            {
                var some_teachers = context.Teachers;
                var list = new List<object>();

                foreach (var each_teacher in some_teachers)
                {
                    list.Add(new
                    {
                        teacher_id = each_teacher.TeacherId
                    });
                }

                var data = new
                {
                    teachers_id = list
                };

                return Helper.JsonConverter.BuildResult(data);
            }
        }
    }
}