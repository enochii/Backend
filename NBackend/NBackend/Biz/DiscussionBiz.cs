using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBackend.Helper;
using NBackend.Models;
using System.Data.Entity;
namespace NBackend.Biz
{
    public class DiscussionBiz
    {
        public static object GetDiscussions(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);

            using (var context = new NBackendContext())
            {
                var discussions = context.Disscussions.Where(a => a.courseId == course_id && a.secId == sec_id
                                                            && a.semester == semester && a.year == year && a.comments != null);
                var list = new List<object>();

                foreach (var a_discussion in discussions)
                {
                    var user_info = context.Users.Single(a => a.Id == a_discussion.userId);

                    list.Add(new
                    {
                        discussion_id = a_discussion.DisscussionId,
                        user_id = user_info.Id,
                        user_name = user_info.user_name,
                        role = user_info.role,
                        content = a_discussion.content,
                        time = a_discussion.time,
                        //question_id = a_discussion.Disscussion_DisscussionId
                    });
                }
                var data = new
                {
                    questions = list
                };

                return Helper.JsonConverter.BuildResult(data);

            }
        }

        public static object GetOneDiscussion(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var discussion_id = int.Parse(body["discussion_id"]);

            using (var context = new NBackendContext())
            {
                var the_discussion = context.Disscussions.Single(a => a.DisscussionId == discussion_id);
                var replys = the_discussion.comments;
                var list = new List<object>();

                foreach (var each_reply in replys)
                {
                    list.Add(new
                    {
                        discussion_id = each_reply.DisscussionId,
                        user_id = each_reply.userId,
                        user_name = context.Users.Single(a => a.Id == each_reply.userId),
                        content = each_reply.content,
                        time = each_reply.time,
                        question_id = the_discussion.DisscussionId
                    });
                }

                var data = new
                {
                    discussion_id = the_discussion.DisscussionId,
                    user_id = the_discussion.userId,
                    user_name = context.Users.Single(a=>a.Id==the_discussion.userId),
                    content = the_discussion.content,
                    time = the_discussion.time,
                    replys = list
                };

                return Helper.JsonConverter.BuildResult(data);
            }
        }

        public static object PostDiscussion(object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            var sec_id = int.Parse(body["sec_id"]);
            var course_id = int.Parse(body["course_id"]);
            var semester = body["semester"];
            var year = int.Parse(body["year"]);
            var user_id = int.Parse(body["user_id"]);
            var content = body["content"];
            var time = body["time"];
            var question_id = int.Parse(body["question_id"]);
            var discussion_id = int.Parse(body["discussion_id"]);

            using (var context = new NBackendContext())
            {
                if (question_id == 0)
                {
                    context.Disscussions.Add(new Disscussion
                    {
                        secId = sec_id,
                        courseId = course_id,
                        semester = semester,
                        year = year,
                        userId = user_id,
                        content = content,
                        time = time,
                        DisscussionId = discussion_id
                    });

                }
                else
                {
                    context.Disscussions.Single(a => a.DisscussionId == question_id).comments.Add(new Disscussion
                    {
                        secId = sec_id,
                        courseId = course_id,
                        semester = semester,
                        year = year,
                        userId = user_id,
                        content = content,
                        time = time,
                        comments = null,
                        //DisscussionId = discussion_id
                    });
                }
                context.SaveChanges();
                return Helper.JsonConverter.BuildResult(null);
            }
        }
    }
}