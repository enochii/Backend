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
    public class BroadcastsController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        [HttpGet]
        [Route("api/class_work")]
        public object GetClassWork(object json)
        {
            //String token = Request.Headers.Authorization.Parameter;
            return Biz.BroadcastBiz.GetAllHomework(json);
        }
        // GET: api/Broadcasts
        public IQueryable<Broadcast> GetBroadcasts()
        {
            return db.Broadcasts;
        }

        // GET: api/Broadcasts/5
        [ResponseType(typeof(Broadcast))]
        public async Task<IHttpActionResult> GetBroadcast(int id)
        {
            Broadcast broadcast = await db.Broadcasts.FindAsync(id);
            if (broadcast == null)
            {
                return NotFound();
            }

            return Ok(broadcast);
        }

        // PUT: api/Broadcasts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBroadcast(int id, Broadcast broadcast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != broadcast.BroadcastId)
            {
                return BadRequest();
            }

            db.Entry(broadcast).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BroadcastExists(id))
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

        // POST: api/Broadcasts
        [ResponseType(typeof(Broadcast))]
        public async Task<IHttpActionResult> PostBroadcast(Broadcast broadcast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Broadcasts.Add(broadcast);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = broadcast.BroadcastId }, broadcast);
        }

        // DELETE: api/Broadcasts/5
        [ResponseType(typeof(Broadcast))]
        public async Task<IHttpActionResult> DeleteBroadcast(int id)
        {
            Broadcast broadcast = await db.Broadcasts.FindAsync(id);
            if (broadcast == null)
            {
                return NotFound();
            }

            db.Broadcasts.Remove(broadcast);
            await db.SaveChangesAsync();

            return Ok(broadcast);
        }

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
    }
}