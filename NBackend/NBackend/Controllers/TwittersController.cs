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
        [Route("api/twitters")]
        public object PostTwi(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return TwitterBiz.postTwi(token, json);
        }

        // GET: api/Twitters
        [HttpGet]
        [Route("api/twitters")]
        public IQueryable<Twitter> GetTwitters()
        {
            return db.Twitters;
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

        // PUT: api/Twitters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTwitter(int id, Twitter twitter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != twitter.TwitterId)
            {
                return BadRequest();
            }

            db.Entry(twitter).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TwitterExists(id))
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

        // POST: api/Twitters
        [ResponseType(typeof(Twitter))]
        public async Task<IHttpActionResult> PostTwitter(Twitter twitter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Twitters.Add(twitter);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = twitter.TwitterId }, twitter);
        }

        // DELETE: api/Twitters/5
        [ResponseType(typeof(Twitter))]
        public async Task<IHttpActionResult> DeleteTwitter(int id)
        {
            Twitter twitter = await db.Twitters.FindAsync(id);
            if (twitter == null)
            {
                return NotFound();
            }

            db.Twitters.Remove(twitter);
            await db.SaveChangesAsync();

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
    }
}