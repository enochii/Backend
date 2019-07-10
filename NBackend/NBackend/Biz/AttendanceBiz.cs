﻿using System;
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
        public static object GetAttendanceRecords(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);

            using (var context = new NBackendContext())
            {
                var course_time = (from each_course in context.Attentions
                                   group each_course by each_course.timeId);

                var list = new List<object>();

                foreach (var each_time in course_time)
                {
                    int present_num = context.Attentions.Where(a => a.timeId == each_time.Key 
                                                                && a.status == true).Count();
                    int absent_num = context.Attentions.Where(a => a.timeId == each_time.Key
                                                              && a.status == false).Count();
                    var students = context.Attentions.Where(a => a.timeId == each_time.Key);
                    var student_list = new List<object>();

                    foreach(var a_student in students)
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

        public static object PostAttendance(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);
            var time_id = int.Parse(body["time_id"]);

            using (var context = new NBackendContext())
            {
                var students = context.Takes.Where(a => a.secId == sec_id && a.courseId == course_id
                                                            && a.semester == semester && a.year == year);
                foreach(var a_student in students)
                {
                    context.Attentions.Add(new Attention
                    {
                        StudentId = a_student.StudentId,
                        secId = sec_id,
                        courseId = course_id,
                        semester = semester,
                        year = year,
                        timeId = time_id,
                        status = false
                    });
                }
                context.SaveChanges();
                return Helper.JsonConverter.BuildResult(null);
            }
        }

        public static object EditAttendanceRecords(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);
            var student_id = int.Parse(body["student_id"]);
            var time_id = int.Parse(body["time_id"]);
            var status = body["status"];

            using (var context = new NBackendContext())
            {
                var records = context.Attentions.Where(a => a.secId == sec_id && a.courseId == course_id
                                                            && a.semester == semester && a.year == year);
                foreach(var a_record in records)
                {
                    a_record.status = true;
                }
                context.SaveChanges();
                return Helper.JsonConverter.BuildResult(null);
            }
        }

    }
}