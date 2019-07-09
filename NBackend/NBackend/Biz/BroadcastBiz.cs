using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBackend.Helper;
using NBackend.Models;
using System.Data.Entity;

namespace NBackend.Biz
{
    public class BroadcastBiz
    {
        public static object GetAllHomework(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);

            using (var context = new NBackendContext())
            {
                var homework = context.Broadcasts.Where(a => a.secId == sec_id && a.courseId == course_id
                                                 && a.semester == semester && a.year == year);
                var list = new List<object>();

                foreach (var each_homework in homework)
                {
                    list.Add(new
                    {
                        broadcast_ID = each_homework.BroadcastId,
                        type = each_homework.type,
                        content = each_homework.content,
                        scope = each_homework.scope,
                        publish_time = each_homework.publish_time,
                        start_time = each_homework.start_time,
                        end_time = each_homework.end_time
                    });
                }
                var data = new
                {
                    classes = list
                };

                return Helper.JsonConverter.BuildResult(data);
            }
        }

    }
}