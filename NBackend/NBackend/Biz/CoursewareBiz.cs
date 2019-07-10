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

    }
}