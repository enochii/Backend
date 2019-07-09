using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NBackend.Biz;
using NBackend.Models;

namespace NBackend.Controllers
{
    public class UsersController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/Users
        [HttpGet]
        [Route("api/users")]
        public object GetUsers(object json)
        {
            return UserBiz.getUsersByNameOrId(json);
        }

        //登录
        [HttpPost]
        [Route("api/login")]
        public object Login(object json)
        {
            return UserBiz.Login(json);
        }
        //注册
        [HttpPost]
        [Route("api/register")]
        public object Register(object json)
        {
            return UserBiz.Register(json);
        }

        //修改
        [HttpPost]
        [Route("api/users")]
        public object UpdateInfo(object json)
        {
            //try
            //{
                var token = Request.Headers.Authorization.Parameter;
            if(token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录？");
            }
            //}catch()

            return UserBiz.UpdateInfo(token, json);
        }

        //关注功能相关
        [HttpGet]
        [Route("api/followers")]
        public object GetFollowers(object json)
        {
            return UserBiz.getFollowers(json);
        }

        [HttpGet]
        [Route("api/following")]
        public object GetFollowing(object json)
        {
            return UserBiz.getFollowing(json);
        }

        [HttpPost]
        [Route("api/following")]
        public object PostFollowing(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return UserBiz.postFollow(token, json);
        }

        [HttpDelete]
        [Route("api/following")]
        public object DeleteFollowing(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return UserBiz.deleteFollow(token, json);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}