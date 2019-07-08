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

            var user_id = body["user_id"];
            var user_name = body["user_name"];
            var department = body["password"];
            var password = body["password"];
            var phone_number = body["phone_number"];
            var email = body["email"];
            var avatar = body["avatar"];
            var role = body["role"];

            //检查id是否已经存在
            var ctx = new NBackendContext();
            

            //根据role的值加入对应的表


            return null;
        }
    }
}