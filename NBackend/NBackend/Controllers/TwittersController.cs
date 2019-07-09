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
    public class TwittersController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        [HttpPost]
        [Route("api/twitter")]
        public object PostTwi(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return TwitterBiz.postTwi(token, json);
        }

        // GET: api/Twitters
        [HttpPost]
        [Route("api/twitters")]
        public object GetTwitters(object json)
        {
            
            object data;

            if (json == null)
            {
                var token = Request.Headers.Authorization.Parameter;
                data = TwitterBiz.getTwis(token);
            }
            else
            {
                data = TwitterBiz.getTwis(json);
            }
            return Helper.JsonConverter.BuildResult(data);

            return db.Twitters;
        }


        //
        [HttpDelete]
        [Route("api/twitters")]
        public object DeleteTwitters(object json)
        {
            var token = Request.Headers.Authorization.Parameter;
            return TwitterBiz.deleteTwi(token, json);
        }
        // GET: api/Twitters/5
        [ResponseType(typeof(Twitter))]
        public async Task<IHttpActionResult> GetTwitter(int id)
        {
            Twitter twitter = await db.Twitters.FindAsync(id);
            if (twitter == null)
            {
                return NotFound();
            }

            return Ok(twitter);
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TwitterExists(int id)
        {
            return db.Twitters.Count(e => e.TwitterId == id) > 0;
        }

        [AllowAnonymous]
        [HttpOptions]
        [Route("api/twitters")]
        public object Options()
        {
            return null;
        }
    }
}