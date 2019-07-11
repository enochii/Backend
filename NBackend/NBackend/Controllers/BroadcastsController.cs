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
using NBackend.Models;
using NBackend.Biz;

namespace NBackend.Controllers
{
    public class BroadcastsController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        [AllowAnonymous]
        [HttpPost]
        [Route("api/class_work")]
        public object GetClassWork(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录？");
            }
            return Biz.BroadcastBiz.GetAllHomework(json);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/broadcasts")]
        public object PostBroadcast(object json)
        {
            string token = Request.Headers.Authorization.Parameter;

            return BroadcastBiz.postBroadcast(token, json);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/class_broadcasts")]
        public object GetClassBroadcast(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return BroadcastBiz.getBroadcasts(token, json, false);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/all_broadcasts")]
        public object GetAllBroadcast()
        {
            var token = Request.Headers.Authorization.Parameter;

            return BroadcastBiz.getBroadcasts(token, null, true);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/broadcast_info")]
        public object GetBroadInfo(object json)
        {
            string token = Request.Headers.Authorization.Parameter;
            return BroadcastBiz.getBroastInfo(token, json);
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("api/broadcasts")]
        public object DeleteBroadcast(object json)
        {
            string token = Request.Headers.Authorization.Parameter;
            return BroadcastBiz.deleteBroadcast(token, json);
        }

        //// GET: api/Broadcasts
        //public IQueryable<Broadcast> GetBroadcasts()
        //{
        //    return db.Broadcasts;
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BroadcastExists(int id)
        {
            return db.Broadcasts.Count(e => e.BroadcastId == id) > 0;
        }

        [AllowAnonymous]
        [HttpOptions]
        [Route("api/class_work")]
        [Route("api/broadcasts")]
        [Route("api/class_broadcasts")]
        [Route("api/all_broadcasts")]
        [Route("api/broadcast_info")]
        public object Options()
        {
            return null;
        }
    }
}