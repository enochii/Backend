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

namespace NBackend.Controllers
{
    public class DiscussionsController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        [AllowAnonymous]
        [HttpPost]
        [Route("api/discussions")]
        public object GetDisscussions(object json)
        {
            return Biz.DiscussionBiz.GetDiscussions(json);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/discussion_summary")]
        public object GetDiscussionSummary(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录啊");
            }
            return Biz.DiscussionBiz.GetDiscussionSummary(json, token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/one_discussion")]
        public object GetOneDiscussion(object json)
        {
            //String token = Request.Headers.Authorization.Parameter;
            return Biz.DiscussionBiz.GetOneDiscussion(json);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/discussion")]
        public object PostDiscussion(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录啊");
            }
            return Biz.DiscussionBiz.PostDiscussion(json);
        }


        // GET: api/Discussions
        public IQueryable<Discussion> GetDiscussions()
        {
            return db.Discussions;
        }

        // GET: api/Discussions/5
        [ResponseType(typeof(Discussion))]
        public async Task<IHttpActionResult> GetDiscussion(int id)
        {
            Discussion discussion = await db.Discussions.FindAsync(id);
            if (discussion == null)
            {
                return NotFound();
            }

            return Ok(discussion);
        }

        // PUT: api/Discussions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDiscussion(int id, Discussion discussion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != discussion.DisscussionId)
            {
                return BadRequest();
            }

            db.Entry(discussion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscussionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Discussions
        [ResponseType(typeof(Discussion))]
        public async Task<IHttpActionResult> PostDiscussion(Discussion discussion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Discussions.Add(discussion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = discussion.DisscussionId }, discussion);
        }

        // DELETE: api/Discussions/5
        [ResponseType(typeof(Discussion))]
        public async Task<IHttpActionResult> DeleteDiscussion(int id)
        {
            Discussion discussion = await db.Discussions.FindAsync(id);
            if (discussion == null)
            {
                return NotFound();
            }

            db.Discussions.Remove(discussion);
            await db.SaveChangesAsync();

            return Ok(discussion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DiscussionExists(int id)
        {
            return db.Discussions.Count(e => e.DisscussionId == id) > 0;
        }

        [AllowAnonymous]
        [HttpOptions]
        [Route("api/discussions")]
        [Route("api/one_discussion")]
        [Route("api/discussion")]
        [Route("api/discussion_summary")]
        public object Options()
        {
            return null;
        }
    }
}