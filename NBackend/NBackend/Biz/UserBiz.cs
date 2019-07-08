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
    }
}