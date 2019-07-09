using NBackend.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NBackend.Models;

namespace NBackend.Biz
{
    public class TwitterBiz
    {
        public static object postTwi(string token, object json)
        {
            Dictionary<string, string> body = JsonConverter.Decode(json);
            NBackendContext ctx = new NBackendContext();
            int user_id = JwtManager.DecodeToken(token);

            //var user_id = int.Parse(body["user_id"]);
            if(UserBiz.getUserById(ctx, user_id) == null)
            {
                return Helper.JsonConverter.Error(400, "你这个人是谁哦");
            }

            var content = body["content"];
            var time = body["content"];
            var image = body["image"];

            Twitter twi = new Twitter
            {
                //TwitterId = 1,
                userId = user_id,
                content = content,
                time = time,
                image = image
            };


            ctx.Twitters.Add(twi);
            try
            {
                ctx.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            var data = new
            {
                twitter_id = twi.TwitterId,
                user_id = twi.userId,
                image = twi.image,
                time = twi.time,
                content= twi.content,
            };

            return JsonConverter.BuildResult(data);
        }

        public static object deleteTwi(string token, object json)
        {
            int user_id = JwtManager.DecodeToken(token);
            var body = JsonConverter.Decode(json);
            int twi_id = int.Parse(body["twitter_id"]);

            NBackendContext ctx = new NBackendContext();
            var q = ctx.Twitters.Where(twi => twi.TwitterId == twi_id && twi.userId == user_id);
            if (!q.Any())
            {
                return Helper.JsonConverter.Error(400, "您没有发表改动态");
            }

            Twitter _twi = q.Single();
            ctx.Twitters.Remove(_twi);
            ctx.SaveChanges();

            return JsonConverter.BuildResult(null);
        }

        public static object getTwis(string token)
        {
            NBackendContext ctx = new NBackendContext();
            int user_id = JwtManager.DecodeToken(token);

            return ListToObj(getAllTwis(ctx, user_id));
        }

        public static object getTwis(object json)
        {
            NBackendContext ctx = new NBackendContext();
            Dictionary<string, string> body = JsonConverter.Decode(json);
            int user_id = int.Parse(body["user_id"]);

            return ListToObj(getSelfTwis(ctx, user_id));

        }

        private static List<Twitter> getAllTwis(NBackendContext ctx, int user_id)
        {
            var list = getSelfTwis(ctx, user_id);
            User user = UserBiz.getUserById(ctx, user_id);

            var q = ctx.Twitters.ToList().Join(user.following,
                twi => twi.userId,
                follow => follow.Id,
                (twi, follow) => twi
                );

            var list1 = q.ToList();
            foreach(var twi in list1)
            {
                list.Add(twi);
            }

            return list;
        }

        private static List<Twitter> getSelfTwis(NBackendContext ctx, int user_id)
        {
            var q = ctx.Twitters.Where(twi => twi.userId == user_id);

            return q.ToList();
        }

        private static List<object> ListToObj(List<Twitter> list)
        {
            List<object> ret = new List<object>();
            foreach(Twitter twi in list)
            {
                ret.Add(new
                {
                    twitter_id = twi.TwitterId,
                    user_id = twi.userId,
                    image = twi.image,
                    time = twi.time,
                    content = twi.content,
                });
            }

            return ret;
        }
    }
}