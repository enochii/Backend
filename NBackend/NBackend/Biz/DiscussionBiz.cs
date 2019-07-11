using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBackend.Helper;
using NBackend.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace NBackend.Biz
{
    public class DiscussionBiz
    {
        public static object GetDiscussions(object json)
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
                    var a_class = context.Sections.Where(a => a.SecId == sec_id && a.courseId == course_id && a.semester == semester && a.year == year);
                    if (!a_class.Any())
                    {
                        return Helper.JsonConverter.Error(400, "不存在这个班级");
                    }

                    var discussions = context.Discussions.Where(a => a.courseId == course_id && a.secId == sec_id
                                                                && a.semester == semester && a.year == year && a.is_comment == false);
                    var list = new List<object>();

                    if (discussions.Any())
                    {
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
                                avatar = user_info.avatar
                                //question_id = a_discussion.Discussion_DisscussionId
                            });
                        }
                    }
                    var data = new
                    {
                        questions = list
                    };
                    return Helper.JsonConverter.BuildResult(data);
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "输入字段格式可能存在问题，或者数据库中可能存在不一致现象，请检查");
            }
        }

        public static object GetOneDiscussion(object json)
        {
            try
            {
                var body = Helper.JsonConverter.Decode(json);
                var discussion_id = int.Parse(body["discussion_id"]);

                using (var context = new NBackendContext())
                {
                    var a_discussion = context.Discussions.Where(a => a.DisscussionId == discussion_id);
                    if (!a_discussion.Any())
                    {
                        return Helper.JsonConverter.Error(400, "这个讨论不存在");
                    }
                    var the_discussion = a_discussion.Single();
                    var replys = the_discussion.comments.ToList();
                    var list = new List<object>();

                    if (the_discussion.is_comment != true)
                    {
                        foreach (var each_reply in replys)
                        {
                            list.Add(new
                            {
                                discussion_id = each_reply.DisscussionId,
                                user_id = each_reply.userId,
                                user_name = context.Users.Single(a => a.Id == each_reply.userId).user_name,
                                role = context.Users.Single(a => a.Id == each_reply.userId).role,
                                content = each_reply.content,
                                time = each_reply.time,
                                avatar = context.Users.Single(a => a.Id == each_reply.userId).avatar,
                                question_id = the_discussion.DisscussionId
                            });
                        }
                    }

                    var data = new
                    {
                        discussion_id = the_discussion.DisscussionId,
                        user_id = the_discussion.userId,
                        user_name = context.Users.Single(a => a.Id == the_discussion.userId).user_name,
                        role = context.Users.Single(a => a.Id == the_discussion.userId).role,
                        avatar = context.Users.Single(a => a.Id == the_discussion.userId).avatar,
                        content = the_discussion.content,
                        time = the_discussion.time,
                        replys = list
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

        public static object PostDiscussion(object json)
        {
            try
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
                //var discussion_id = int.Parse(body["discussion_id"]);

                using (var context = new NBackendContext())
                {
                    var any_user = context.Users.Where(a => a.Id == user_id);
                    if (!any_user.Any())
                    {
                        return Helper.JsonConverter.Error(400, "这个人有问题");
                    }

                    if (time.Count() > 20 || time.Count() < 16)
                        return Helper.JsonConverter.Error(400, "这个时间格式有问题");

                    Discussion new_discussion = new Discussion();
                    if (question_id == 0)
                    {
                        new_discussion = new Discussion
                        {
                            secId = sec_id,
                            courseId = course_id,
                            semester = semester,
                            year = year,
                            userId = user_id,
                            content = content,
                            time = time,
                            is_comment = false,
                        };
                        context.Discussions.Add(new_discussion);

                    }
                    else
                    {
                        var any_discussion = context.Discussions.Where(a => a.DisscussionId == question_id);
                        if (!any_discussion.Any())
                        {
                            return Helper.JsonConverter.Error(400, "这个问题不对啊");
                        }

                        new_discussion = new Discussion
                        {
                            secId = sec_id,
                            courseId = course_id,
                            semester = semester,
                            year = year,
                            userId = user_id,
                            content = content,
                            time = time,
                            is_comment = true
                        };
                        any_discussion.Single().comments.Add(new_discussion);
                    }

                    // 写数据库 
                    context.SaveChanges();

                    return Helper.JsonConverter.BuildResult(new
                    {
                        discussion_id = new_discussion.DisscussionId
                    });
                }
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
                return Helper.JsonConverter.Error(410, "数据库中可能存在不一致现象，请检查");
            }
        }

        public static object GetDiscussionSummary(object json, string token)
        {
            try
            {
                var body = Helper.JsonConverter.Decode(json);
                var year = int.Parse(body["year"]);
                var semester = body["semester"];
                var user_id = Helper.JwtManager.DecodeToken(token);

                using (var context = new NBackendContext())
                {
                    var some_discussions = context.Discussions.Where(a => a.userId == user_id && a.semester == semester && a.year == year);
                    var some_courses = (from each_discussions in some_discussions
                                        group each_discussions by each_discussions.courseId into dgroups
                                        select new
                                        {
                                            course_id = dgroups.Key,
                                            //course_name = some_discussions.First(a=>a.courseId==dgroups.Key).n,
                                            course_discussion_num = some_discussions.Where(a => a.courseId == dgroups.Key).Count()
                                        } into discussion_courses
                                        orderby discussion_courses.course_discussion_num descending
                                        select discussion_courses);
                    var data = new
                    {
                        total_discussions = some_discussions.Count(),
                        total_courses = some_courses.Count(),
                        max_course_name = context.Courses.Single(a => a.CourseId == some_courses.FirstOrDefault().course_id).course_name
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
