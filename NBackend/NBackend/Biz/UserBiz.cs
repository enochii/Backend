using NBackend.Helper;
using NBackend.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Net.Http;

namespace NBackend.Biz
{
    public class UserBiz
    {
        public const string TEACHER_EDU = "teacher_edu";
        public const string TEACHER_MANAGE = "teacher_manage";
        public const string STUDENT = "student";

        public static object UpdateInfo(string token, object json)
        {
            try
            {
                Dictionary<string, string> body = JsonConverter.Decode(json);

                NBackendContext ctx = new NBackendContext();

                int id = Helper.JwtManager.DecodeToken(token);

                User user = getUserById(ctx, id);
                if (user == null)
                {
                    return JsonConverter.Error(400, "用户不存在");
                }

                var user_name = body["user_name"];
                var department = body["department"];
                //var password = body["password"];
                var phone_number = body["phone_number"];
                var email = body["email"];
                var avatar = body["avatar"];
                var role = body["role"];

                user.user_name = user_name;
                user.department = department;
                //user.password = password;
                user.phone_number = phone_number;
                user.mail = email;
                user.avatar = avatar;
                user.role = role;

                int grade = -1;
                string job_title;
                if (user.role.Equals("student"))
                {
                    grade = int.Parse(body["grade"]);
                    var q = ctx.Students.Where(stu => stu.StudentId == user.Id);
                    if (!q.Any())
                    {
                        return JsonConverter.Error(400, "没有这个人");
                    }
                    else
                    {
                        var stu = q.Single();
                        stu.grade = grade;
                    }
                }
                else
                {
                    job_title = body["job_title"];
                    var q = ctx.Teachers.Where(tea => tea.TeacherId == user.Id);
                    if (!q.Any())
                    {
                        return JsonConverter.Error(400, "用户不存在");
                    }
                    else
                    {
                        var tea = q.Single();
                        tea.job_title = job_title;
                    }
                }

                ctx.SaveChanges();

                int following_num = user.following.Count();
                int followers_num = user.followers.Count();

                var data =
                 new
                 {
                     user_id = user.Id,
                     user_name = user.user_name,
                     department = user.department,
                     phone_number = user.phone_number,
                     email = user.mail,
                     avatar = user.avatar,
                     role = user.role,

                     following = following_num,
                     follower = followers_num,
                 };

                return JsonConverter.BuildResult(data);
            }
            catch(Exception e)
            {
                return Helper.JsonConverter.Error(400, "更新信息出错");
            }
        }

        public static object ListToObj(List<User> users)
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
            try
            {
                Dictionary<string, string> body = JsonConverter.Decode(json);

                var user_id = int.Parse(body["user_id"]);
                var user_name = body["user_name"];
                var department = body["department"];
                var password = body["password"];
                var phone_number = body["phone_number"];
                var email = body["email"];
                var avatar = body["avatar"];
                var role = body["role"];

                //检查id是否已经存在
                var ctx = new NBackendContext();
                if (ctx.Users.Any(_user => _user.Id == user_id))
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

                string encoded_password = UserHelper.Encrypt(user_id, password);
                var user = new User
                {
                    Id = user_id,
                    user_name = user_name,
                    department = department,
                    password = encoded_password,
                    phone_number = phone_number,
                    mail = email,
                    avatar = avatar,
                    role = role
                };
                ctx.Users.Add(user);
                //ctx.SaveChanges();
                ValidationHelper.safeSaveChanges(ctx);

                return JsonConverter.BuildResult(null, 200, "ok");
            }
            catch(Exception e)
            {
                return Helper.JsonConverter.Error(400 , "注册失败，请检查表单字段或者用户id是否正确输入");
            }
        }

        public static object Login(object json)
        {
            try
            {
                var body = JsonConverter.Decode(json);

                var user_id = int.Parse(body["user_id"]);
                var password = body["password"];

                NBackendContext ctx = new NBackendContext();

                var q = ctx.Users.Where(_user => _user.Id == user_id);
                if (!q.Any())
                {
                    return JsonConverter.Error(400, "用户id不正确");
                }
                //
                User user = q.Single();

                //待修改
                string encoded_password = UserHelper.Encrypt(user_id, password);
                if (!user.password.Equals(password) && !user.password.Equals(encoded_password))
                {
                    return JsonConverter.Error(400, "密码错误");
                }

                int following_num = user.following.Count();
                int followers_num = user.followers.Count();

                var token = Helper.JwtManager.GenerateToken(user_id);

                var data = new
                {
                    user_id = user_id,
                    user_name = user.user_name,
                    department = user.department,
                    phone_number = user.phone_number,
                    email = user.mail,
                    avatar = user.avatar,
                    role = user.role,
                    token = token,
                    following = following_num,
                    follower = followers_num,
                };

                return JsonConverter.BuildResult(data, 200, "ok");
            }
            catch
            {
                return Helper.JsonConverter.Error(400, "登陆失败，请确认您的密码或者id");
            }
        }

        public static object getUsersByNameOrId(object json)
        {
            var body = JsonConverter.Decode(json);

            var list = new List<object>();
            NBackendContext ctx = new NBackendContext();
            if (body.ContainsKey("user_id"))
            {
                int id = int.Parse(body["user_id"]);

                var user = getUserById(ctx, id);
                if(user == null)
                {
                    return JsonConverter.Error(400, "火星用户！");
                }

                int grade = -1;
                string job_title = "";

                if(!getGradeOrTitle(ctx, user,ref grade, ref job_title))
                {
                    return JsonConverter.Error(400, "用户信息有误");
                }

                var data = new
                {
                    role = user.role,
                    user_id = user.Id,
                    user_name = user.user_name,
                    department = user.department,
                    phone_number = user.phone_number,
                    email = user.mail,
                    avatar = user.avatar,
                    job_title = job_title,
                    grade = grade,
                    following = user.following.Count(),
                    follower = user.followers.Count(),
                };

                return JsonConverter.BuildResult(data, 200, "ok");
                
            }
            else if (body.ContainsKey("user_name") && !body["user_name"].Equals(""))
            {
                string name = body["user_name"];
                var q = ctx.Users.Where(_user => _user.user_name == name);


                var _users = q.ToList();

                return ListToObj(ctx, _users);
                
            }
            else
            {
                //无字段，暂时返回所有用户？
                var data = ListToObj(ctx.Users.ToList());
            }

            return list;
        }

        private static object followHelper(bool followers, object json)
        {
            Dictionary<string, string> body = JsonConverter.Decode(json);
            NBackendContext ctx = new NBackendContext();

            int id = int.Parse(body["user_id"]);
            User user = getUserById(ctx, id);

            var _users = followers ? user.followers : user.following;
            var users = _users.ToList();
            
            return ListToObj(ctx, users);
        }

        public static object getFollowers(object json)
        {
            return followHelper(true, json);
        }

        public static object getFollowing(object json)
        {
            return followHelper(false, json);
        }

        private static bool containsUser(List<User> users, User user)
        {
            foreach(var _user in users)
            {
                if(user.Id == _user.Id)
                {
                    return true;
                }
            }
            return false;
        }

        public static object postFollow(string token, object json)
        {
            try
            {
                Dictionary<string, string> body = JsonConverter.Decode(json);
                NBackendContext ctx = new NBackendContext();

                int following_id = int.Parse(body["user_id"]);
                int user_id = Helper.JwtManager.DecodeToken(token);
                User user = getUserById(ctx, user_id);
                User following = getUserById(ctx, following_id);

                if(user==null || following == null)
                {
                    return Helper.JsonConverter.Error(400, "用户信息有误");
                }

                if(user.Id == following.Id)
                {
                    return Helper.JsonConverter.Error(400, "请不要follow自己");
                }

                if (containsUser(user.following.ToList(), following))
                {
                    return Helper.JsonConverter.Error(400, "你已经follow过该用户");
                }
                if (containsUser(following.followers.ToList(), user))
                {
                    return Helper.JsonConverter.Error(400, "follow信息有误");
                }

                user.following.Add(following);
                following.followers.Add(user);

                ctx.SaveChanges();
                return Helper.JsonConverter.BuildResult(null);
            }
            catch(Exception e)
            {
                return Helper.JsonConverter.Error(400, "关注失败");
            }
        }

        public static object deleteFollow(string token, object json)
        {
            try
            {
                Dictionary<string, string> body = JsonConverter.Decode(json);
                NBackendContext ctx = new NBackendContext();

                int following_id = int.Parse(body["user_id"]);
                int user_id = Helper.JwtManager.DecodeToken(token);
                User user = getUserById(ctx, user_id);
                User following = getUserById(ctx, following_id);

                if (!containsUser(user.following.ToList(), following))
                {
                    return Helper.JsonConverter.Error(400, "你并没有follow");
                }
                if (!containsUser(following.followers.ToList(), user))
                {
                    return Helper.JsonConverter.Error(400, "follow信息有误");//表不一致
                }
                user.following.Remove(following);
                following.followers.Remove(user);

                ctx.SaveChanges();
                return Helper.JsonConverter.BuildResult(null);
            }
            catch(Exception e)
            {
                return Helper.JsonConverter.Error(400, "取消关注失败");
            }
        }

        //通过id和context获取用户
        public static User getUserById(NBackendContext ctx, int id)
        {
            var q = ctx.Users.Where(_user => _user.Id == id);

            return q.Any() ? q.Single() : null;
        }

        //通过用户获取grade或者job_title
        private static bool getGradeOrTitle(NBackendContext ctx, User user, ref int grade, ref string job_title)
        {
            //根据用户的类型去分发逻辑
            if (user.role.Equals(STUDENT))
            {
                var q = ctx.Students.Where(_stu => _stu.StudentId == user.Id);
                if (!q.Any())
                {
                    return false;
                }
                var stu = q.Single();
                grade = stu.grade;
            }
            else
            {
                var q = ctx.Teachers.Where(_tea => _tea.TeacherId == user.Id);
                if (!q.Any())
                {
                    return false;
                }
                var tea = q.Single();
                job_title = tea.job_title;
            }

            return true;
        }

        private static object ListToObj(NBackendContext ctx, List<User> _users)
        {
            var users = new List<object>();

            foreach (var user in _users)
            {
                int grade = -1;
                string job_title = "";

                if (!getGradeOrTitle(ctx, user, ref grade, ref job_title))
                {
                    return JsonConverter.Error(400, "用户信息有误");
                }

                users.Add(new
                {
                    role = user.role,
                    user_id = user.Id,
                    user_name = user.user_name,
                    department = user.department,
                    phone_number = user.phone_number,
                    email = user.mail,
                    avatar = user.avatar,
                    grade = grade,
                    job_title = job_title,
                });

            }
            var data = new
            {
                users = users,
            };
            return JsonConverter.BuildResult(data, 200, "ok");
        }

        //获取用户的关注列表，包括ers和ing
        public static object getFolowInfo(string token)
        {
            NBackendContext ctx = new NBackendContext();
            try
            {
                int user_id = JwtManager.DecodeToken(token);
                User user = getUserById(ctx, user_id);

                var data = new
                {
                    following = user.following.Count(),
                    follower = user.followers.Count(),
                };

                return JsonConverter.BuildResult(data);
            }
            catch(Exception e)
            {
                return JsonConverter.Error(400, "获取关注信息出错");
            }
        }
    }
}