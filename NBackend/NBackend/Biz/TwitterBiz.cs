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
                userId = user_id,
                content = content,
                time = time,
                image = image
            };

            ctx.Twitters.Add(twi);
            ctx.SaveChanges();

            return JsonConverter.BuildResult(twi);
        }
    }
}