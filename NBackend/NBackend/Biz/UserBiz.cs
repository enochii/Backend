using NBackend.Helper;
using NBackend.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace NBackend.Biz
{
    public class UserBiz
    {
        public const string TEACHER_EDU = "teacher_edu";
        public const string TEACHER_MANAGE = "teacher_manage";
        public const string STUDENT = "student";

        public static object ListToObj(DbSet<User> users)
        {
            //using (var ctx = new NBackendContext())
            {
                var list = new List<object>();

                foreach(var user in users)
                {
                    list.Add(new
                    {
                        user_id = user.Id,
                        user_name = user.user_name,
                        department = user.department,
                        password = user.password,
                        phone_number = user.phone_number,
                        mail = user.mail,
                        avatar = user.avatar
                    });
                }
                
                return list;

            }

                //return null;
        }

        public static object Register(object json)
        {
            Dictionary<string, string> body = JsonConverter.Decode(json);

            var user_id = int.Parse(body["user_id"]);
            var user_name = body["user_name"];
            var department = body["password"];
            var password = body["password"];
            var phone_number = body["phone_number"];
            var email = body["email"];
            var avatar = body["avatar"];
            var role = body["role"];

            //检查id是否已经存在
            var ctx = new NBackendContext();
            if(ctx.Users.Any(_user=>_user.Id == user_id))
            {
                return JsonConverter.Error(400, "该用户已经注册");
            }

            //根据role的值加入对应的表
            //需要插入两张表
            if (role.Equals(STUDENT))
            {
                var grade = int.Parse(body["grade"]);
                if (ctx.Students.Any(_user => _user.StudentId == user_id))
                {
                    return JsonConverter.Error(400, "该用户已经注册");
                }
                ctx.Students.Add(new Student
                {
                    StudentId = user_id,
                    grade = grade
                });
            }
            else
            {

                if (!role.Equals(TEACHER_EDU) && !role.Equals(TEACHER_MANAGE))
                {
                    return JsonConverter.Error(400, "角色字段值有误");
                }
                if (ctx.Teachers.Any(_user => _user.TeacherId == user_id))
                {
                    return JsonConverter.Error(400, "该用户已经注册");
                }
                var job_title = body["job_title"];
                ctx.Teachers.Add(new Teacher
                {
                    TeacherId = user_id,
                    job_title = job_title,
                    is_manager = role.Equals(TEACHER_MANAGE)
                });

            }
            var user = new User
            {
                Id = user_id,
                user_name = user_name,
                department = department,
                password = password,
                phone_number = phone_number,
                mail = email,
                avatar = avatar
            };
            ctx.Users.Add(user);

            return JsonConverter.BuildResult(null, 200, "ok no趴笨");
        }

        public static object Login(object json)
        {
            var body = JsonConverter.Decode(json);

            var user_id = int.Parse(body["user_id"]);
            var password = body["password"];

            NBackendContext ctx = new NBackendContext();

            //var user = ctx.Users

            return null;
        }
    }
}