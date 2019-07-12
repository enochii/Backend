using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBackend.Helper;
using NBackend.Models;
using System.Data.Entity;

namespace NBackend.Biz
{
    public class AttendanceBiz
    {
        //获取某个班级某次课的出席记录
        public static object GetAttendanceRecords(object json)
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
                    var any_section = context.Sections.Where(a => a.SecId == sec_id && a.courseId == course_id
                                                && a.semester == semester && a.year == year);
                    if (!any_section.Any())
                    {
                        return Helper.JsonConverter.Error(400, "这个班有问题啊");
                    }

                    var course_time = (from each_course in context.Attentions
                                       group each_course by each_course.timeId);

                    var list = new List<object>();

                    foreach (var each_time in course_time)
                    {
                        int present_num = context.Attentions.Where(a => a.timeId == each_time.Key
                                                                    && (a.status == 1 || a.status == 4)).Count();
                        int absent_num = context.Attentions.Where(a => a.timeId == each_time.Key
                                                                  && (a.status == 2 || a.status == 3)).Count();
                        var students = context.Attentions.Where(a => a.timeId == each_time.Key);
                        var student_list = new List<object>();

                        foreach (var a_student in students)
                        {
                            var student_name = context.Users.Single(a => a.Id == a_student.StudentId).user_name.ToString();
                            student_list.Add(new
                            {
                                student_id = a_student.StudentId,
                                student_name,
                                status = a_student.status
                            });
                        }
                        list.Add(new
                        {
                            time_id = each_time.Key,
                            present_num,
                            absent_num,
                            students = student_list

                        });
                    }

                    var data = new
                    {
                        class_attendance = list
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

        //生成某个班级某次课的出席记录表
        public static object PostAttendance(object json)
        {
            try
            {
                var body = Helper.JsonConverter.Decode(json);
                var sec_id = int.Parse(body["sec_id"]);
                var course_id = int.Parse(body["course_id"]);
                var semester = body["semester"];
                var year = int.Parse(body["year"]);
                var time_id = int.Parse(body["time_id"]);

                using (var context = new NBackendContext())
                {
                    var any_section = context.Sections.Where(a => a.SecId == sec_id && a.courseId == course_id
                                                                && a.semester == semester && a.year == year);
                    if (!any_section.Any())
                    {
                        return Helper.JsonConverter.Error(400, "这个班有问题啊");
                    }
                    var any_records = context.Attentions.Where(a => a.secId == sec_id && a.courseId == course_id
                                                                && a.semester == semester && a.year == year && a.timeId == time_id);
                    if (any_records.Any())
                    {
                        return Helper.JsonConverter.Error(400, "已经生成过这次课的出席记录辽");
                    }

                    var students = context.Takes.Where(a => a.secId == sec_id && a.courseId == course_id
                                                                && a.semester == semester && a.year == year);
                    foreach (var a_student in students)
                    {
                        context.Attentions.Add(new Attention
                        {
                            StudentId = a_student.StudentId,
                            secId = sec_id,
                            courseId = course_id,
                            semester = semester,
                            year = year,
                            timeId = time_id,
                            status = 2
                        });
                    }
                    context.SaveChanges();
                    return Helper.JsonConverter.BuildResult(null);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "数据库中可能存在不一致现象，请检查");
            }
        }

        //编辑某次出席记录
        public static object EditAttendanceRecords(object json)
        {
            try
            {
                var body = Helper.JsonConverter.Decode(json);
                var sec_id = int.Parse(body["sec_id"]);
                var course_id = int.Parse(body["course_id"]);
                var semester = body["semester"];
                var year = int.Parse(body["year"]);
                var student_id = int.Parse(body["user_id"]);
                var time_id = int.Parse(body["time_id"]);
                var status = int.Parse(body["status"]);

                using (var context = new NBackendContext())
                {
                    var any_section = context.Sections.Where(a => a.SecId == sec_id && a.courseId == course_id
                                                && a.semester == semester && a.year == year);
                    if (!any_section.Any())
                    {
                        return Helper.JsonConverter.Error(400, "这个班有问题啊");
                    }

                    var records = context.Attentions.Where(a => a.secId == sec_id && a.courseId == course_id
                                                                && a.semester == semester && a.year == year && a.StudentId == student_id && a.timeId == time_id);
                    if (!records.Any())
                    {
                        return Helper.JsonConverter.Error(400, "这个班这次课没有出席表啊");
                    }
                    foreach (var a_record in records)
                    {
                        a_record.status = status;
                    }
                    context.SaveChanges();
                    return Helper.JsonConverter.BuildResult(null);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "数据库中可能存在不一致现象，请检查");
            }
        }

        public static object GetAttendanceSummary(object json, string token)
        {
            try
            {
                var body = Helper.JsonConverter.Decode(json);
                var semester = body["semester"];
                var year = int.Parse(body["year"]);
                var user_id = Helper.JwtManager.DecodeToken(token);

                using (var context = new NBackendContext())
                {
                    var total_records = context.Attentions.Where(a => a.StudentId == user_id && a.semester == semester && a.year == year);
                    var total_absent = total_records.Where(a => a.status == 2 || a.status == 3);

                    var data = new
                    {
                        total_attendance = total_records.Count(),
                        total_absent = total_absent.Count()
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
    }
}