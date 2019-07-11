using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBackend.Helper;
using NBackend.Models;
using System.Data.Entity;

namespace NBackend.Biz
{
    public class CoursewareBiz
    {
        //获取某个班级的课件
        public static object GetAllCoursewares(object json)
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

                var coursewares = context.CourseWares.Where(a => a.secId == sec_id && a.courseId == course_id
                                                            && a.semester == semester && a.year == year);
                var list = new List<object>();

                foreach (var the_courseware in coursewares)
                {
                    list.Add(new
                    {
                        //sec_id = the_courseware.secId,
                        //course_id = the_courseware.courseId,
                        //semester = the_courseware.semester,
                        //year = the_courseware.year,
                        courseware_id = the_courseware.CourseWareId,
                        name = the_courseware.name,
                        location = the_courseware.location
                    });
                }
                var data = new
                {
                    courswares = list
                };

                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object PostCourseware(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);
            var name = body["name"];
            var location = body["location"];

            using (var context = new NBackendContext())
            {
                var any_class = context.Sections.Where(a => a.SecId == sec_id && a.courseId == course_id
                                                            && a.semester == semester && a.year == year);
                if (!any_class.Any())
                {
                    return Helper.JsonConverter.Error(400, "这个班不存在啊");
                }

                var new_courseware = new CourseWare
                {
                    secId = sec_id,
                    courseId = course_id,
                    semester = semester,
                    year = year,
                    name = name,
                    location = location
                };
                context.CourseWares.Add(new_courseware);
                context.SaveChanges();

                return Helper.JsonConverter.BuildResult(new
                {
                    courseware_id = new_courseware.CourseWareId
                });
            }
        }

        public static object DeleteCourseware(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            //var sec_id = int.Parse(body["sec_id"]);
            //var course_id = int.Parse(body["course_id"]);
            //var semester = body["semester"];
            //var year = int.Parse(body["year"]);
            var courseware_id = int.Parse(body["courseware_id"]);

            using (var context = new NBackendContext())
            {
                var any_courseware = context.CourseWares.Where(a => a.CourseWareId==courseware_id);
                if (!any_courseware.Any())
                {
                    return Helper.JsonConverter.Error(400, "这个课件有问题");
                }
                var the_courseware = any_courseware.Single();
                context.CourseWares.Remove(the_courseware);
                context.SaveChanges();
                return Helper.JsonConverter.BuildResult(null);
            }
        }

    }
}