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
        //获取某个班的作业
        const int TYPE_JOB = 1, TYPE_ACTIVITY = 2;
        const int SCOPE_CLASS = 1, SCOPE_GOLBAL = 2;

        

        public static object GetAllHomework(object json)
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
                    works = list
                };

                return Helper.JsonConverter.BuildResult(data);
            }
        }

        //创建广播
        public static object postBroadcast(string token, object json)
        {
            try
            {
                var body = JsonConverter.Decode(json);
                NBackendContext ctx = new NBackendContext();

                int teacher_id = JwtManager.DecodeToken(token);
                User user = UserBiz.getUserById(ctx, teacher_id);
                if (user == null)
                {
                    return Helper.JsonConverter.Error(400, "你还没登录？");
                }

                int type = int.Parse(body["type"]);
                int scope = int.Parse(body["scope"]);

                string start_time = body["start_time"];
                string end_time = body["end_time"];
                string published_time = body["published_time"];
                string content = body["content"];

                //k
                int sec_id, course_id, year;
                string semester;

                if (scope == SCOPE_CLASS)
                {
                    if (!user.role.Equals("teacher_edu"))
                    {
                        return Helper.JsonConverter.Error(400, "你没有权限呢");
                    }
                    sec_id = int.Parse(body["sec_id"]);
                    course_id = int.Parse(body["course_id"]);
                    year = int.Parse(body["year"]);
                    semester = body["semester"];
                }
                else
                {
                    if (!user.role.Equals("teacher_manage"))
                    {
                        return Helper.JsonConverter.Error(400, "你没有权限呢");
                    }
                    //默认班级
                    sec_id = 1;
                    course_id = 2;
                    year = 2019;
                    semester = "Spring";
                }
                Broadcast broadcast = new Broadcast
                {
                    secId = sec_id,
                    courseId = course_id,
                    year = year,
                    semester = semester,

                    scope = scope,
                    type = type,
                    start_time = start_time,
                    publish_time = published_time,
                    end_time = end_time,
                    content = content
                };

                ctx.TeacherBroadcasts.Add(new TeacherBroadcast
                {
                    teacherId = teacher_id,
                    broadcastId = broadcast.BroadcastId
                });

                ctx.Broadcasts.Add(broadcast);
                ValidationHelper.safeSaveChanges(ctx);

                var data = new
                {
                    broadcast_id = broadcast.BroadcastId
                };
                return JsonConverter.BuildResult(data);
            }
            catch(Exception e)
            {
                return JsonConverter.Error(400, "创建广播失败");
            }

        }

        //获取班级的所有广播
        private static List<object> getBroadcastsOfClass(string token, object json)
        {
            try
            {
                int user_id = JwtManager.DecodeToken(token);
                //做验证

                var body = JsonConverter.Decode(json);
                int sec_id = int.Parse(body["sec_id"]);
                int course_id = int.Parse(body["course_id"]);
                int year = int.Parse(body["year"]);
                string semester = body["semester"];

                NBackendContext ctx = new NBackendContext();

                return ListToObj(_getBroadcastsOfClass(ctx, sec_id, course_id, year, semester));
            }
            catch(Exception e)
            {
                
                return null;
            }
            

        }

        //获取用户所有班级的所有考试
        public static List<Broadcast> _getAllExamsOfAllClass(NBackendContext ctx, string token)
        {
            int user_id = Helper.JwtManager.DecodeToken(token);

            //获取用户的所有班级
            var q = ctx.Takes.Where(take => take.StudentId == user_id).Select(take => take.Section);

            //var secs = q.ToList();
            var q1 = ctx.Broadcasts.Join(q, bro => bro.Section, sec => sec,
                (bro, sec) => bro
                );

            //.Join(ctx.Courses, bro=>bro.Course, course=>course,
            //    (bro, course) => new {bro, course.}

            List<Broadcast> all_sec_exams = new List<Broadcast>();

            return all_sec_exams;
        }

        //
        public static List<object> getAllBroadcasts(string token)
        {
            NBackendContext ctx = new NBackendContext();

            var bros = _getAllExamsOfAllClass(ctx, token);

            return ListToObj(bros);
        }

        //将列表转化为json
        private static List<object> ListToObj(List<Broadcast> broadcasts)
        {
            List<object> list = new List<object>();

            foreach(Broadcast broadcast in broadcasts)
            {
                list.Add(new
                {
                    broadcast.Course.course_name,
                    broadcast_id = broadcast.BroadcastId,
                    broadcast.content,
                    broadcast.type,
                    broadcast.scope,
                    sec_id = broadcast.secId,
                    course_id = broadcast.courseId,
                    broadcast.semester,
                    broadcast.year,
                    broadcast.publish_time,
                    broadcast.start_time,
                    broadcast.end_time,
                });
            }

            return list;
        }

        private static List<Broadcast> _getBroadcastsOfClass(NBackendContext ctx, int sec_id, int course_id, int year, string semester)
        {
            Section sec = Biz.ClassBiz.getSection(ctx, sec_id, course_id, year, semester);

            if (sec == null)
            {
                return null;
            }

            var q = ctx.Broadcasts.ToList().Where(broadcast => broadcast.Section.Equals(sec));
            var broadcasts = q.ToList();

            return broadcasts;
        }
      
        private static List<Broadcast> _getGlobalBroadcasts(NBackendContext ctx)
        {

            var q = ctx.Broadcasts.Where(bro => bro.scope == SCOPE_GOLBAL);
            return q.ToList();
        }

        //获取全局广播
        private static List<object> getGlobalBroadcasts()
        {
            NBackendContext ctx = new NBackendContext();

            return ListToObj(_getGlobalBroadcasts(ctx));
        }

        //获取全部广播/班级广播
        public static object getBroadcasts(string token, object json, bool all)
        {
            List<object> class_bros = null;

            try
            {
                if (all)
                {
                    var global_bros = getGlobalBroadcasts();//全局广播
                    class_bros = getAllBroadcasts(token);//所有班级的广播
                    foreach (object bro in global_bros)
                    {
                        class_bros.Add(bro);
                    }
                }
                else
                {
                    class_bros = getBroadcastsOfClass(token, json);
                    if (class_bros == null)
                    {
                        return JsonConverter.Error(400, "嘻嘻，前端哥哥姐姐填写的字段有问题");
                    }
                }

                return JsonConverter.BuildResult(new { broadcasts = class_bros });
            }
            catch(Exception e)
            {
                return JsonConverter.Error(404, "查找广播资源时出错");
            }
        }
 

        //删除广播
        public static object deleteBroadcast(string token, object json)
        {
            try
            {
                var body = JsonConverter.Decode(json);
                int broadcast_id = int.Parse(body["broadcast_id"]);
                int user_id = JwtManager.DecodeToken(token);

                NBackendContext ctx = new NBackendContext();

                var q = ctx.TeacherBroadcasts.Where(tb => tb.broadcastId == broadcast_id && tb.teacherId == user_id);
                var q1 = ctx.Broadcasts.Where(bro => bro.BroadcastId == broadcast_id);

                if (!q.Any())
                {
                    return JsonConverter.Error(400, "该广播不存在或者你没有创建过该广播(＾Ｕ＾)ノ~ＹＯ");
                }
                var _tb = q.Single();
                var broadcast = q1.Single();
                ctx.TeacherBroadcasts.Remove(_tb);
                ctx.Broadcasts.Remove(broadcast);

                ctx.SaveChanges();
                return JsonConverter.BuildResult(null);
            }
            catch(Exception e)
            {
                return JsonConverter.Error(400, e.Message);
            }
        }
        //获取广播的具体信息
        public static object getBroastInfo(string token, object json)
        {
            try
            {
                var body = JsonConverter.Decode(json);
                int broadcast_id = int.Parse(body["broadcast_id"]);

                NBackendContext ctx = new NBackendContext();
                var q = ctx.Broadcasts.Where(bro => bro.BroadcastId == broadcast_id);

                if (!q.Any())
                {
                    return JsonConverter.Error(400, "该广播不存在！");
                }

                var broadcast = q.Single();

                object data;
                if (broadcast.scope == SCOPE_CLASS)
                {
                    data = new
                    {
                        broadcast_id,
                        broadcast.content,
                        broadcast.type,
                        broadcast.scope,
                        sec_id = broadcast.secId,
                        course_id = broadcast.courseId,
                        broadcast.semester,
                        broadcast.year,
                        broadcast.start_time,
                        broadcast.end_time,
                        broadcast.publish_time,
                    };
                }
                else
                {
                    data = new
                    {
                        broadcast_id,
                        broadcast.content,
                        broadcast.type,
                        broadcast.scope,

                        broadcast.start_time,
                        broadcast.end_time,
                        broadcast.publish_time,
                    };

                }

                return JsonConverter.BuildResult(data);
            }
            catch(Exception e)
            {
                return JsonConverter.Error(400, "请检查输入字段格式或者值");
            }
        }
    }
}